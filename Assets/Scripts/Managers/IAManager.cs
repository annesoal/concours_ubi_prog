using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class IAManager : MonoBehaviour
{
    public static IAManager Instance{ get; private set; }


    private void Awake()
    {
        Instance = this;
    }
    
        public void MoveEnemies()
        {
            Enemy enemy;
            List<GameObject> enemies = Enemy.GetEnemiesInGame();
            foreach (var enemyObj in enemies)
            {
                enemy = enemyObj.GetComponent<Enemy>();
                StartRoutineMoveEnemy(enemy);

            }
        }

        private void StartRoutineMoveEnemy(Enemy enemy)
        {
            enemy.Move();
        }


    

}
