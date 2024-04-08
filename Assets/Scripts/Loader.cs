using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenuScene = 0,
        LobbyScene,
        GameScene,
        LoadingScene,
        CharacterSelectScene,
        // TEST SCENES
        Blocks = 5,
        LevelSelection,
    }

    public static Scene TargetScene { private set; get; }

    public static void Load(Scene targetScene)
    {
        TargetScene = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoadNetwork(Scene targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(TargetScene.ToString());
    }

    public static void ReturnToMainMenuClean()
    {
        GameLobbyManager.Instance.LeaveLobby();
            
        NetworkManager.Singleton.Shutdown();
            
        Load(Scene.MainMenuScene);
    }

}
