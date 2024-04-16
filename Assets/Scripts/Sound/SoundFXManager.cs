using Unity.Netcode;
using UnityEngine;

namespace Sound
{
    public class SoundFXManager : MonoBehaviour
    {
        public static SoundFXManager instance;
        [SerializeField] private AudioSource soundFXObject;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void PlaySoundFXCLip(AudioClip audioClip, Transform spawnTransform, float volume)
        {
            AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.Play();
            float clipLength = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLength);
        }
        
        public void PlaySoundFXCLip(AudioClip audioClip, Vector3 position, float volume)
        {
            AudioSource audioSource = Instantiate(soundFXObject, position, Quaternion.identity);
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            Debug.Log(" lenghht audio clip "  + audioClip.length);
            audioSource.Play();
            float clipLength = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLength);
        }
        

    }
}