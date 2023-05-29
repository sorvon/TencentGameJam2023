using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StartFilmController : MonoBehaviour
{
    [SerializeField] private GameObject startText;

    private GameManager gameManager;
    private SceneController sceneManager;
    private AudioManager audioManager;
    [SerializeField] private VideoPlayer video;

    private void Start()
    {
        gameManager = ServiceLocator.Get<GameManager>();
        sceneManager = ServiceLocator.Get<SceneController>();
        audioManager = ServiceLocator.Get<AudioManager>();

        StartCoroutine(nameof(GameStart));
    }

    private IEnumerator GameStart()
    {
        audioManager.bgmController.PlayStartFilmBGM();
        video.Pause();
        yield return new WaitForSecondsRealtime(1.5f);
        video.Play();
        while (video.time / video.length * 100 < 95)
        {
            Debug.Log($"Length:{video.length} Current:{video.time} Percent:{video.time / video.length * 100}%");
            yield return new WaitForFixedUpdate();
        }
        video.Pause();
        StartTextAppear();
    }
    
    private void StartTextAppear()
    {
        startText.SetActive(true);
    }
}