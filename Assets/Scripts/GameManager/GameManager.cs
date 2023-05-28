using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;

public class GameManager : Service
{
    [Other] private SceneController sceneController;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void StartGame()
    {
        sceneController.LoadNextScene();
    }
    public void RestartGame()
    {
        if (SceneControllerUtility.SceneIndex == 3)
        {
            sceneController.LoadScene(3);
            Debug.LogWarning("RestartGame");
        }
    }
}
