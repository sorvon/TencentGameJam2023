using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;

public class GameManager : Service
{
    [Other] private SceneController sceneController;
    [Other] private AudioManager audioManager;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void StartGame()
    {
        sceneController.LoadNextScene();
        audioManager.bgmController.StartGameBGM();
    }
    public void RestartGame()
    {
        if (SceneControllerUtility.SceneIndex == 2)
        {
            sceneController.LoadScene(2);
            Debug.LogWarning("RestartGame");
        }
        audioManager.bgmController.StartGameBGM();
    }
}
