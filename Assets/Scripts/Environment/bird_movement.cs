using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;

public class bird_movement : MonoBehaviour
{
    [SerializeField, Label("移动距离")]
    private float moveDist;
    [SerializeField, Label("移动速度")]
    private float moveSpeed;
    private MyObject myObject;
    private SpriteRenderer sp;
    private Vector2 generatePos;
    private bool ifLeft;

    private void Start()
    {
        myObject = GetComponent<MyObject>();
        sp = GetComponent<SpriteRenderer>();
        myObject.OnActivate += OnActivate;
        myObject.OnRecycle += OnRecycle;
    }

    private void FixedUpdate()
    {
        if (ifLeft)
        {
            transform.Translate(Vector2.left * (Time.deltaTime * moveSpeed));
            if (transform.position.x < generatePos.x-moveDist)
            {
                ifLeft = false;
            }
        }
        else
        {
            transform.Translate(Vector2.right * (Time.deltaTime * moveSpeed));
            if (transform.position.x > generatePos.x+moveDist)
            {
                ifLeft = true;
            }
        }

        sp.flipX = ifLeft;
    }

    private void OnActivate()
    {
        generatePos = transform.position;
    }

    private void OnRecycle()
    {
        ifLeft = true;
        sp.flipX = false;
        generatePos = Vector2.zero;
    }

    public void Init()
    {
        ifLeft = true;
        sp.flipX = false;
        generatePos = transform.position;
    }
}
