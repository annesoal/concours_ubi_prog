using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Grid.Interface;
using UnityEngine;
using Utils;

namespace Enemies.Attack
{
    public class SniperEyeEnemy : AttackingEnemy
    {

        [SerializeField] public  GameObject _bullet;
        [SerializeField] public Transform _bulletStartPosition;
        
        ShootingUtility shootingUtility;
         
        public static int SniperHealth;
        public static int SniperAttack;
        public static int SniperRange;
        public static int SniperMoveRatio;

        private int _attack = SniperAttack;
        private int _health = SniperHealth;
        private int _range = SniperRange;
        private int _moveRatio = SniperMoveRatio;
        
        public override int MoveRatio { get => _moveRatio; set => _moveRatio = value; }
        public override int Health { get => _health; set => _health =  value ; }
        public override int AttackDamage { get => _attack; set => _attack = value; }
        public int Range
        {
            get => _range;
            set => _range = value;
        }

        public new void Start()
        {
            base.Start();

            if (IsServer)
            {
                 shootingUtility = gameObject.AddComponent<ShootingUtility>();
                 shootingUtility.ObjectToFire = _bullet;
                 shootingUtility.TimeToFly = 0.3f;
                 shootingUtility.Angle = 0.9f;
            }
           
        }

        public SniperEyeEnemy()
        {
            ennemyType = EnnemyType.Flying;
        }
        
        protected override bool IsValidCell(Cell toCheck)
        {
            Cell updatedCell = TilingGrid.grid.GetCell(toCheck.position);
            bool isValidBlockType = (updatedCell.type & BlockType.EnemyWalkable) > 0;
            bool hasNoEnemy = !TilingGrid.grid.HasTopOfCellOfType(updatedCell, TypeTopOfCell.Enemy);
            return isValidBlockType && hasNoEnemy; 
        }
        
        protected override IEnumerator MoveEnemy(Vector3 direction)
        {
            if (CellHasObstacle(direction))
                direction += new Vector3(0.0f, 0.7f, 0.0f);
            
            yield return StartCoroutine(base.MoveEnemy(direction));
        }

        private bool CellHasObstacle(Vector3 direction)
        {
            Vector2Int position = TilingGrid.LocalToGridPosition(direction);
            Cell updatedCell = TilingGrid.grid.GetCell(position);
            return updatedCell.HasObjectOfTypeOnTop(TypeTopOfCell.Obstacle);
        }
        
        public override AttackingInfo ChoseToAttack()
        {
            Vector2Int v = TilingGrid.LocalToGridPosition(transform.position);
            List<Cell> cellsInRadius =
                TilingGrid.grid.GetCellsInRadius(v, Range);

            return ChoseAttack(cellsInRadius);
        }

        protected override IEnumerator AttackAnimation(AttackingInfo infos)
        {
             if (!IsServer) yield break;

             RotationAnimation rotationAnimation = new();
             hasFinishedMoveAnimation = false;
             StartCoroutine(rotationAnimation.TurnObjectTo(this.gameObject,
                 infos.toKill.gameObject.transform.position));
             yield return new WaitUntil(rotationAnimation.HasMoved);
             animator.SetBool("Attack", true);
             float currentTime = 0.0f;
             float timeToSoot = 0.1f;
             bool hasShot = false;
             while (timeToMove > currentTime)
             {
                 currentTime += Time.deltaTime;
                 if (timeToMove >= timeToSoot && !hasShot)
                 {
                     if (IsServer)
                     {
                         hasShot = true;
                         shootingUtility.FireBetween(_bulletStartPosition.position,
                         infos.toKill.gameObject.transform.position);
                         
                     }
                 }
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
    }
}