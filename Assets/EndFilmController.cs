using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.Video;

public class EndFilmController : MonoBehaviour
{
    private AudioManager audioManager;
    [SerializeField] private VideoPlayer video;

    private void Start()
    {
        audioManager = ServiceLocator.Get<AudioManager>();
        StartCoroutine(nameof(GameEnd));
    }

    IEnumerator GameEnd()
    {
        video.Pause();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        audioManager.bgmController.PlayEndFilmBGM();
        yield return new WaitForSecondsRealtime(1.5f);
        video.Play();
        
        while (video.time / video.length * 100 < 70)
        {
            Debug.Log($"Length:{video.length} Current:{video.time} Percent:{video.time / video.length * 100}%");
            yield return new WaitForFixedUpdate();
        }
        video.Pause();//画面停留在但愿人长久
        yield return new WaitForSecondsRealtime(3f);
        video.Play();
        
    }

    private void ThanksForPlaying()//谢幕表
    {
        
    }
}
