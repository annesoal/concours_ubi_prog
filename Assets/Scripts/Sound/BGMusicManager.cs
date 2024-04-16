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
            Debug.Log("La scène " + scene.name + " a été chargée.");
            PlayMusic();
        }

        private void PlayMusic()
        {
            if (audioSource.clip == audioClipMenu)
            {
                Debug.Log("music : musique menu est en train de jouer");
                if (isMenuScenes())
                    return;
                Debug.Log("music : musique menu devrait arreter");
                audioSource.Stop();
            }

            if (audioSource.clip == audioClipGame)
            {
                Debug.Log("music : musique game est en train de jouer");
                if (!isMenuScenes())
                    return;
                Debug.Log("music : musique game devrait arreter");
                audioSource.Stop();
            }

            Debug.Log("music : on check pour changer de music");
            StartMusic();
        }

        private void StartMusic()
        {
            if (isMenuScenes())
            {
                Debug.Log("music menu devrait commencer bientot");

                audioSource.clip = audioClipMenu;
           
                Debug.Log("music clip  menu mis");
                if (audioSource.enabled)
                {
                    Debug.Log("music audio source etait enabled");
                }
                audioSource.Play();
                
                
            }
            else
            {
                Debug.Log("music game devrait commencer");
                
                audioSource.clip = audioClipGame;
                
                if (audioSource.enabled)
                {
                    Debug.Log("music audio source etait enabled");
                }
                audioSource.Play();
            }
        }


        private void PlayMenuMusic()
        {
            Debug.Log("music menu devrait commencer bientot");

            audioSource.clip = audioClipMenu;
           
            Debug.Log("music clip  menu mis");
            if (audioSource.enabled)
            {
                Debug.Log("music audio source etait enabled");
            }
            audioSource.Play();
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