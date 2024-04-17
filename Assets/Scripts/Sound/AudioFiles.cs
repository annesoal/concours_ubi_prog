using UnityEngine;
using UnityEngine.Serialization;

namespace Sound
{
    public class AudioFiles : MonoBehaviour
    {
        
        public static AudioFiles Instance{ get; private set; }
        [SerializeField] private AudioClip bonusAudioClip;
        [SerializeField] private AudioClip resourceAudioClip;
        [SerializeField] private AudioClip malusAudioSound;
        [SerializeField] private AudioClip victoryAudioLip;
        [FormerlySerializedAs("lostAudioSound")] [SerializeField] private AudioClip gameOverAudioSound;
        [SerializeField] private AudioClip menuMusic;
        [SerializeField] private AudioClip gameMusic;
        [SerializeField] private AudioClip gameOverAudioClip;
        [SerializeField] private AudioClip victoryAudioClip;
        [SerializeField] private AudioClip explosionAudioClip;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public AudioClip getExplosionClip()
        {
            return explosionAudioClip;
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

    
        public AudioClip getGameOverAudio()
        {
            return gameOverAudioClip;
        }
        
        public AudioClip getVictoryAudio()
        {
            return victoryAudioClip;
        }

      
    }
}