using Unity.Netcode;
using UnityEngine;

namespace Sound
{
    public class SoundFXManager : NetworkBehaviour
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
            float clipLenght = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLenght);
        }

    }
}