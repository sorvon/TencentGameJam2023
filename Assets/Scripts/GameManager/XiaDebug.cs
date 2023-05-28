using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;

public class XiaDebug : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3;
    new private Camera camera;
    private Transform cameraTrans;
    private GameManager manager;
    private EnvGenerator generator; 
    private void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameraTrans = camera.transform;
        manager = ServiceLocator.Get<GameManager>();
        generator = ServiceLocator.Get<EnvGenerator>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            cameraTrans.Translate(Vector2.up * (moveSpeed * Time.deltaTime));
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            cameraTrans.Translate(Vector2.down * (moveSpeed * Time.deltaTime));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            manager.RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            generator.LevelUp();
        }
    }
}
