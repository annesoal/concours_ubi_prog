using System;
using System.Collections;
using System.Collections.Generic;
using Enemies.Basic;
using Grid;
using Grid.Interface;
using UnityEngine;
using UnityEngine.Assertions;
using Random = System.Random;

namespace Enemies
{
    public abstract class AttackingEnemy : BasicEnemy
    {
        public abstract int AttackDamage { get; set; }

        protected override EnemyChoicesInfo BackendMove()
        {
             Assert.IsTrue(IsServer);
                        
            if (HasReachedTheEnd())
            {
                Player.Health--;
                CleanUp();
                return new EnemyChoicesInfo()
                {
                    hasReachedEnd = true,
                };
            }

            if (!IsTimeToMove() || isStupefiedState > 0)
            {
                return new EnemyChoicesInfo() { hasMoved = false };
            }

            var attackInfo = ChoseToAttack();
            if ( attackInfo.hasAttacked )
            {
                return new EnemyChoicesInfo()
                {
                    attack = attackInfo,
                };

            }
            
            if (!TryMoveOnNextCell())
            {
                hasPath = false;
                if (!MoveSides())
                {
                    return new EnemyChoicesInfo();
                }
                else
                {
                    return new EnemyChoicesInfo()
                    {
                        hasMoved = true, 
                        destination = TilingGrid.GridPositionToLocal(cell.position),
                    };
                }
            }
            return new EnemyChoicesInfo()
            {
                hasMoved = true,
                destination = TilingGrid.GridPositionToLocal(cell.position),
            };
        }

        public abstract AttackingInfo ChoseToAttack();
        public AttackingInfo ChoseAttack(List<Cell> cellsInRadius)
        {
            foreach (var aCell in cellsInRadius)
            {
     
                if (TowerIsAtRange(aCell))
                {
                    hasPath = false;
                    var attackedObjectInfo = AttackTower(aCell.GetTower());
                    var tower = attackedObjectInfo.Item2.GetComponent<BaseTower>();
                    int remainingHealth = tower.Damage(AttackDamage);
                    bool hasKilled = remainingHealth <= 0;
                    if (hasKilled)
                    {
                        tower.Clean();
                    }
                    return new AttackingInfo()
                    {
                        shouldKill = hasKilled,
                        hasAttacked = true,
                        toKill = attackedObjectInfo.Item2,
                        isTower = attackedObjectInfo.Item1,
                    };
                }
            }

            return new AttackingInfo();
        }

        private bool TowerIsAtRange(Cell aCell)
        {

            Cell updatedCell = TilingGrid.grid.GetCell(aCell.position);
      
            // non walkable building are towers or obstacle.
            return TilingGrid.grid.HasTopOfCellOfType(updatedCell, TypeTopOfCell.Building) &&
                   updatedCell.HasNonWalkableBuilding();
        }
        

        
        protected (bool, GameObject) AttackTower(BaseTower toAttack)
        {
            
            return (true, toAttack.gameObject);

        }
        protected virtual IEnumerator AttackAnimation(AttackingInfo infos)
        {
            if (!IsServer) yield break;
            
            hasFinishedMoveAnimation = false;
            animator.SetBool("Attack", true);
            float currentTime = 0.0f;

            while (timeToMove > currentTime)
            {
                currentTime += Time.deltaTime;
                yield return null;
            }
            
            animator.SetBool("Attack", false);
            if (infos.shouldKill)
            {
                if (infos.isTower)
                {
                    infos.toKill.gameObject.GetComponent<BaseTower>().DestroyThis();
                }
                else
                {
                    infos.toKill.gameObject.GetComponent<Obstacle>().DestroyThis();
                }
            }
            hasFinishedMoveAnimation = true;
        }

        // Peut detruire obstacle et tower, tous les cells avec obstacles `solides` sont valides 
        public override bool PathfindingInvalidCell(Cell cellToCheck)
        {
            return false;
        }

        protected override bool IsValidCell(Cell toCheck)
        {
            Cell updatedCell = TilingGrid.grid.GetCell(toCheck.position);
            bool hasObstacleOnTop = updatedCell.HasTopOfCellOfType(TypeTopOfCell.Obstacle);
            return base.IsValidCell(toCheck) && !hasObstacleOnTop;
        }


        public override void MoveCorroutine(EnemyChoicesInfo infos)
        {
            if (infos.attack.hasAttacked)
            {
                StartCoroutine(AttackAnimation(infos.attack));
            }
            else
            {
                base.MoveCorroutine(infos);
            }
        }
    }
}
