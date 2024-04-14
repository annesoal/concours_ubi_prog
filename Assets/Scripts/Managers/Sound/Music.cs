using System;
using UnityEngine;

namespace Sound
{
    public class Music : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
     
        
        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            audioSource = GetComponent<AudioSource>();
            PlayMusic();
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