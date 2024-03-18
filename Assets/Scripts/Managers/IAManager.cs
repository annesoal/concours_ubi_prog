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
    
        public static void MoveEnemies(int energy)
        {
            Enemy enemy;
            List<GameObject> enemies = Enemy.GetEnemiesInGame();
            for(int i = enemies.Count -1; i >= 0 ; i--)
            {
                enemy = enemies[i].GetComponent<Enemy>();
                enemy.Move(energy);
            }
            
            
        }

      


    

}
