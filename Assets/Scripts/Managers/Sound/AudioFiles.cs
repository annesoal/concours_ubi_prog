using UnityEngine;

namespace Sound
{
    public class AudioFiles : MonoBehaviour
    {
        
        public static AudioFiles Instance{ get; private set; }
        [SerializeField] protected AudioClip bonusAudioClip;
        [SerializeField] protected AudioClip resourceAudioClip;
        [SerializeField] protected AudioClip malusAudioSound;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public AudioClip getBonusClip()
        {
            return bonusAudioClip;
        }
        public AudioClip getResourceClip()
        {
            return resourceAudioClip;
        }
        public AudioClip getMalusClip()
        {
            return malusAudioSound;
        }
    }
}