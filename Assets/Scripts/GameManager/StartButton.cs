using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    private GameManager manager;
    void Start()
    {
        manager = ServiceLocator.Get<GameManager>();
    }

    public void StartGame() => manager.StartGame();

}
