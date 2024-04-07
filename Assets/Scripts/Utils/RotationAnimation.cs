using System.Collections;
using UnityEngine;

namespace Utils
{
    public class RotationAnimation
    {
        private bool _hasFinished = false; 
        public IEnumerator TurnObject90(GameObject gameObject, float timeToMove, bool left)
        {
            _hasFinished = false;
            float currentTime = 0.0f;
            int direction = left ? 1 : -1;
            Quaternion origin = gameObject.transform.rotation;
            Quaternion rotation = Quaternion.Euler(
                gameObject.transform.eulerAngles + new Vector3(0, direction * 90, 0));
            while (timeToMove > currentTime)
            {
                gameObject.transform.rotation = Quaternion.Slerp(
                    origin, rotation,currentTime/timeToMove);
                currentTime += Time.deltaTime;
                yield return null;
            }

            _hasFinished = true;
        }

        public bool HasMoved()
        {
            return _hasFinished;
        }
        
    }
}