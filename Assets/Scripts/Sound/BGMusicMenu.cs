using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sound
{
    public class BGMusicMenu : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        public static BGMusicMenu instance;
        
        private void Awake()
        {
            if(instance != null)
                Destroy(gameObject);
            else
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
                audioSource = GetComponent<AudioSource>();
                PlayMusic();
            }
        }
        
        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                audioSource.Stop();
            }
            
        }
        public void PlayMusic()
        {
            if (audioSource.isPlaying) return;
            audioSource.Play();
        }

        public void StopMusic()
        {
            audioSource.Stop();
        }

        private void OnDestroy()
        {
            StopMusic(); 
        }
    }
}