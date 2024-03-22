using System.Collections;
using UnityEngine;
using Unity.Collections;
using System;
using Unity.VisualScripting;
using Unity.Mathematics;

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
        
        public void FireBetween(Vector3 initialPosition, Vector3 targetPosition, float radAngle)
        {
            Vector3 thirdPosition = GetThirdPoint(initialPosition, targetPosition,radAngle);
        }
  
        private static Vector3 GetThirdPoint(Vector3 initpos, Vector3 targetPos, float startingAngle)
        {
            // On cherche le point du milieu entre init et target
            Vector3 midPoint = (targetPos + initpos) / 2;
        
            float distance = Vector3.Distance(initpos, midPoint);
            float oppositeLength = (float) Math.Tan(startingAngle) * distance; 
            Vector3 oppositeVector = Vector3.up * oppositeLength;
            Vector3 thirdPointPosition = midPoint + oppositeVector;
             
            return thirdPointPosition;
        }
        
    }
}