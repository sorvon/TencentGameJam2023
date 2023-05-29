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
        audioManager.StopSound("StartFilmBGM");
        sceneController.LoadNextScene();
        audioManager.PlaySound("Level0BGM", AudioPlayMode.Interrupt);
    }
    public void RestartGame()
    {
        audioManager.bgmController.StartGameBGM();
        if (SceneControllerUtility.SceneIndex == 2)
        {
            sceneController.LoadScene(2);
            Debug.LogWarning("RestartGame");
        }
    }
}
