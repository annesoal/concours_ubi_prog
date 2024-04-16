using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sound
{
    public class BGMusicGame : MonoBehaviour
    {

        public static BGMusicGame instance;
        [SerializeField] private AudioSource audioSource;
     
        
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
            if (SceneManager.GetActiveScene().name == "LobbyScene")
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