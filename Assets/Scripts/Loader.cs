using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenuScene,
        LobbyScene,
        GameScene,
        LoadingScene,
        CharacterSelectScene,
        // TEST SCENES
        Blocks,
        RaphCopieBlocks,
    }

    private static Scene _targetScene;

    public static void Load(Scene targetScene)
    {
        _targetScene = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoadNetwork(Scene targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(_targetScene.ToString());
    }

    public static void ReturnToMainMenuClean()
    {
        GameLobbyManager.Instance.LeaveLobby();
            
        NetworkManager.Singleton.Shutdown();
            
        Load(Scene.MainMenuScene);
    }
}
