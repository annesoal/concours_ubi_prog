using System;
using System.Collections.Generic;
using Enemies;
using Grid;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Managers
{
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
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                var enemy = enemies[i].GetComponent<Enemy>();
                SetEnemyPath(enemy);
                enemy.Move(totalEnergy);
            }
        }

        private static void SetEnemyPath(Enemy enemy)
        {
         
            if (enemy.hasPath) return;
            
            Cell origin = enemy.GetCurrentPosition();
            Debug.Log("vvv 1");
            Debug.Log("ICI origin posotion" + origin.position);
            Cell destination = enemy.GetDestination();
            Debug.Log("vvv 2");
            Debug.Log("ICI origin destinitaion" + destination.position);
            Func<Cell, bool> invalidCellPredicate = enemy.PathfindingInvalidCell;
            enemy.path = AStarPathfinding.GetPath(origin, destination, invalidCellPredicate);
            Debug.Log("vvv 3");
            Debug.Log("ICI path firtd " + enemy.path[0].position);
            enemy.hasPath = true;
            enemy.path.RemoveAt(0); // cuz l'olgo donne la cell d'origine comme premier element
            Debug.Log("vvv 4" + enemy.path.Count);
            Debug.Log("ICI path apres remove pos 1 " + enemy.path[1].position);
            Debug.Log("ICI path apres remove " + enemy.path[0].position);
        }
     
        private static void Highlight(Cell cell)
        {
            Instantiate(TowerDefenseManager.highlighter, TilingGrid.CellPositionToLocal(cell), quaternion.identity);
         
        }

        public static void ResetEnemies()
        {
            List<GameObject> enemies = Enemy.GetEnemiesInGame();
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Enemy>().hasPath = false;
            }
           }
    }
}
