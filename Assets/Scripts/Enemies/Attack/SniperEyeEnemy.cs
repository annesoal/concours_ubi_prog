using System.Collections;
using System.Collections.Generic;
using Grid;
using Grid.Interface;
using UnityEngine;
namespace Enemies.Attack
{
    public class SniperEyeEnemy : AttackingEnemy
    {
        
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
            Cell cell = TilingGrid.grid.GetCell(position);
            return cell.HasObjectOfTypeOnTop(TypeTopOfCell.Obstacle);
        }
        
        public override bool ChoseToAttack()
        {
            if (path == null || path.Count == 0)
            {
                Debug.Log("Dans le early return de chose to attack");
                return true;
            }
            
            List<Cell> cellsInRadius =
                TilingGrid.grid.GetCellsInRadius(TilingGrid.LocalToGridPosition(transform.position), Range);
            return (ChoseAttack(cellsInRadius));
        }

    }
}