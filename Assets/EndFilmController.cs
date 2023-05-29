using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.Video;

public class EndFilmController : MonoBehaviour
{
    private AudioManager audioManager;
    private SceneController sceneController;
    [SerializeField] private VideoPlayer video;


    private void Start()
    {
        audioManager = ServiceLocator.Get<AudioManager>();
        sceneController = ServiceLocator.Get<SceneController>();
        StartCoroutine(nameof(GameEnd));
    }

    IEnumerator GameEnd()
    {
        video.Pause();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForSecondsRealtime(1.5f);
        audioManager.bgmController.PlayEndFilmBGM();
        video.Play();

        while (video.time / video.length * 100 < 70)
        {
            Debug.Log($"Length:{video.length} Current:{video.time} Percent:{video.time / video.length * 100}%");
            yield return new WaitForFixedUpdate();
        }

        video.Pause(); //画面停留在但愿人长久
        yield return new WaitForSecondsRealtime(3f);
        video.Play();
        while(video.time / video.length * 100 < 99)
        {
            yield return new WaitForFixedUpdate();
        }
        while (true)
        {
            if (Input.anyKey)
                Application.Quit();
            yield return new WaitForEndOfFrame();
        }
    }

    private void ThanksForPlaying() //谢幕表
    {
    }
}