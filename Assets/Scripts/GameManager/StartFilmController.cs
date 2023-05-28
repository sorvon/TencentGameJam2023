using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StartFilmController : MonoBehaviour
{
    private GameManager gameManager;
    private SceneController sceneManager;
    [SerializeField]private VideoPlayer video;
    private void Start()
    {
        gameManager = ServiceLocator.Get<GameManager>();
        sceneManager = ServiceLocator.Get<SceneController>();
        video.Pause();
        Invoke(nameof(StartVideo),1.5f);
    }

    private void Update()
    {
        Debug.Log($"Length:{video.length} Current:{video.time} Percent:{video.time/video.length*100}%");
        if (video.time / video.length * 100 > 80)
        {
            sceneManager.LoadNextScene();
        }
    }

    private void StartVideo()
    {
        video.Play();
    }
}
