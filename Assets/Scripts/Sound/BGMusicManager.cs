using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sound
{
    public class BgMusicManager : MonoBehaviour
    {
        public static BgMusicManager instance;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioClipMenu;
        [SerializeField] private AudioClip audioClipGame;

        private void Awake()
        {
            if (instance != null)
                Destroy(gameObject);
            else
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            PlayMusic();
        }

        private void PlayMusic()
        {
            if (audioSource.clip == audioClipMenu)
            {
                if (isMenuScenes())
                    return;
                audioSource.Stop();
            }

            if (audioSource.clip == audioClipGame)
            {
                if (!isMenuScenes())
                    return;
                audioSource.Stop();
            }
            
            StartMusic();
        }

        private void StartMusic()
        {
            if (isMenuScenes())
            {
                audioSource.clip = audioClipMenu;
                audioSource.Play();
            }
            else
            {
                audioSource.clip = audioClipGame;
                audioSource.Play();
            }
        }

        
        private bool isMenuScenes()
        {
            Debug.Log("music :  est menu scnene bool " + (SceneManager.GetActiveScene().name != "Level1" &&
                                                             SceneManager.GetActiveScene().name != "Level3" &&
                                                             SceneManager.GetActiveScene().name != "NewsBlocks"));
            return SceneManager.GetActiveScene().name != "Level1" &&
                   SceneManager.GetActiveScene().name != "Level3" &&
                   SceneManager.GetActiveScene().name != "NewsBlocks";
        }
        
        
        
    }
}