using System;
using System.Collections.Generic;
using Enemies;
using Grid;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using Utils;

public class IAManager : MonoBehaviour
{
    public static IAManager Instance{ get; private set; }


    private void Awake()
    {
        Instance = this;
    }
    
        public static void MoveEnemies(int totalEnergy)
        {
            List<GameObject> enemies = Enemy.GetEnemiesInGame();
            for(int i = enemies.Count -1; i >= 0 ; i--)
            {
                var enemy = enemies[i].GetComponent<Enemy>();
               
                SetEnemyPath(enemy);
                enemy.Move(totalEnergy);
            }
        }

        private static void SetEnemyPath(Enemy enemy)
        {
            Cell origin = enemy.GetCurrentPosition();
            Cell destination = enemy.GetDestination();
            Func<Cell, bool> invalidCellPredicate = enemy.PathfindingInvalidCell;
            Highlight(destination); 
            enemy.path = AStarPathfinding.GetPath(origin, destination, invalidCellPredicate); 
        }
        
        private static void Highlight(Cell cell)
        {
            Instantiate(TowerDefenseManager.highlighter, TilingGrid.CellPositionToLocal(cell), quaternion.identity);
            
        }
}
