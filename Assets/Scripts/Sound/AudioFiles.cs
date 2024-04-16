using UnityEngine;

namespace Sound
{
    public class AudioFiles : MonoBehaviour
    {
        
        public static AudioFiles Instance{ get; private set; }
        [SerializeField] private AudioClip bonusAudioClip;
        [SerializeField] private AudioClip resourceAudioClip;
        [SerializeField] private AudioClip malusAudioSound;
        [SerializeField] private AudioClip victoryAudioLip;
        [SerializeField] private AudioClip lostAudioSound;

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