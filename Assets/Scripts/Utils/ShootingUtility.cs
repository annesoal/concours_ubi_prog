using System.Collections;
using UnityEngine;
using Unity.Collections;

namespace Utils
{
    public class ShootingUtility :  MonoBehaviour
    {
        public float TimeToFly { set; private get; }
        public GameObject ObjectToFire{ set; private get; }

        public float Gravity
        {
            set => _gravity.y = value;
            get => _gravity.y; 
        }
        private Vector3 _gravity = new Vector3();
        
        public void FireBetween(Vector3 initialPosition, Vector3 targetPosition, float angle)
        {
            StartCoroutine( Fire(initialPosition, targetPosition, angle));
        }

        private IEnumerator Fire(Vector3 initialPosition, Vector3 targetPosition, float startingForce)
        {
            float timer = TimeToFly;
            Vector3 currentPosition = initialPosition;
            while (!HasReachedTarget(currentPosition, targetPosition))
            {
                float ratio = timer / Time.deltaTime; 
                Vector3 nextPosition = FindNextPosition(currentPosition, targetPosition, startingForce, ratio);
                yield return null;
            }
        }

        private static bool HasReachedTarget(Vector3 position, Vector3 targetPosition)
        {
            return position.Equals(targetPosition);
        }

        private Vector3 FindNextPosition(Vector3 initialPosition, Vector3 targetPosition, float angle, float ratio)
        {
            return new Vector3();
        }
    }
}