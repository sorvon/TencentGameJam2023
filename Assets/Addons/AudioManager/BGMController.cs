using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;

public class BGMController : MonoBehaviour
{
    private EventSystem eventSystem;
    private AudioManager audioManager;
    private LevelManager levelManager;

    private void Start()
    {
        eventSystem = ServiceLocator.Get<EventSystem>();
        audioManager = ServiceLocator.Get<AudioManager>();
        eventSystem.AddListener<int>(EEvent.AfterLoadScene,TryGetServices);
        
        audioManager.SetGroupVolume(ESoundGroup.BGM,0.6f);
    }

    private void TryGetServices(int index)
    {
        if(index!=2)
            return;
        levelManager = ServiceLocator.Get<LevelManager>();
        levelManager.OnHeightLevelInt += PlayBGMOnLevelUp;
    }

    private void PlayBGMOnLevelUp(int level)
    {
        audioManager.StopGroup(ESoundGroup.BGM,true);
        if (level < 1 || level > 3)
        {
            Debug.LogWarning($"播放音频时海拔等级{level}超出序列");
            return;
        }
        audioManager.PlaySound($"Level{level}BGM");

    }

    public void PlayStartFilmBGM()
    {
        audioManager.StopGroup(ESoundGroup.BGM,true);
        audioManager.PlaySound("StartFilmBGM");
    }

    public void PlayEndFilmBGM()
    {
        audioManager.StopGroup(ESoundGroup.BGM,true);
        audioManager.PlaySound("EndFilmBGM");
    }

    public void StartGameBGM()
    {
        audioManager.StopGroup(ESoundGroup.BGM,true);
        audioManager.PlaySound("Level0BGM");
    }
    
}
