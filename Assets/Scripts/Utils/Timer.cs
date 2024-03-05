using UnityEngine;

namespace Unity.Multiplayer.Samples.Utilities.ClientAuthority.Utils
{
    public class Timer
    {
        public float period { set; get; }

        private float _startTime;
        
        public Timer(float period)
        {
            this.period = period;
        }

        public void Start()
        {
            _startTime = Time.time;
        }

        public bool HasTimePassed()
        {
            return Time.time - _startTime >= period; 
        }
    }
}