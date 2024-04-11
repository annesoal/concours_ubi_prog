using System.Collections;
using Grid;
using Grid.Interface;
using UnityEngine;
using Random = System.Random;
namespace Enemies.Attack
{
    public class SniperEyeEnemy : AttackingEnemy
    {

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
                direction += new Vector3(0.0f, 0.5f, 0.0f);
            
            yield return StartCoroutine(base.MoveEnemy(direction));
        }

        private bool CellHasObstacle(Vector3 direction)
        {
            Vector2Int position = TilingGrid.LocalToGridPosition(direction);
            Cell cell = TilingGrid.grid.GetCell(position);
            return cell.HasObjectOfTypeOnTop(TypeTopOfCell.Obstacle);
        }

    }
}