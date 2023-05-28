using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;

public class BackgroundScroller : Service
{
    [Header("摄像机")]
    [SerializeField] new private Camera camera;
    [Header("背景图")]
    [SerializeField] private List<Transform> backgroundTrans;

    [Header("场景切换海拔")] private List<float> transHeight=new List<float>();

    private Transform cameraTrans;
    
    private Transform top;
    private Transform mid;
    private Transform bottom;
    private int topid = 0;
    private int midid = 1;
    private int bottomid = 2;

    private float CameraSize => camera.orthographicSize*2;
    private float CameraUp => cameraTrans.position.y + camera.orthographicSize; //摄像机上边界
    private float CameraDown => cameraTrans.position.y - camera.orthographicSize;
    private float CameraX => cameraTrans.position.x;

    protected override void Start()
    {
        base.Start();
        cameraTrans = camera.GetComponent<Transform>();
        Init();
    }

    private void Update()
    {
        if (CameraUp > top.position.y)
        {
            bottom.position = new Vector3(CameraX, top.position.y + CameraSize);
            int temp = topid;
            topid = bottomid;
            bottomid = midid;
            midid = temp;
            ApplyTrans();
            
        }
        else if (CameraDown < bottom.position.y)
        {
            top.position = new Vector3(CameraX, bottom.position.y - CameraSize);
            int temp = bottomid;
            bottomid = topid;
            topid = midid;
            midid = temp;
            ApplyTrans();
        }
    }

    private void ApplyTrans()
    {
        top = backgroundTrans[topid];
        mid = backgroundTrans[midid];
        bottom = backgroundTrans[bottomid];
    }

    private void Init()
    {
        top = backgroundTrans[0];
        mid = backgroundTrans[1];
        bottom = backgroundTrans[2];
    }
}


//top
//mid
//botttom