using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using Unity.VisualScripting;
using UnityEngine;

public class BackgroundScroller : Service
{
    [SerializeField, Label("三层级Transform")]
    private List<Transform> backgroundTrans;

    [Header("场景切换海拔"), SerializeField] private List<float> transHeight = new List<float>();
    
    private Transform cameraTrans;
    [Other] private LevelManager levelManager;
    new private Camera camera;

    [SerializeField]private BackgroundUnit topUnit;
    [SerializeField]private BackgroundUnit midUnit;
    [SerializeField] private BackgroundUnit bottomUnit;
    private int topid = 0;
    private int midid = 1;
    private int bottomid = 2;

    private float CameraSize => camera.orthographicSize * 2;
    private float CameraUp => cameraTrans.position.y + camera.orthographicSize; //摄像机上边界
    private float CameraDown => cameraTrans.position.y - camera.orthographicSize;
    private float CameraX => cameraTrans.position.x;
    private float Height => levelManager.Height;

    protected override void Start()
    {
        base.Start();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameraTrans = camera.GetComponent<Transform>();
        Init();
    }

    private void Update()
    {
        // Debug.Log($"CameUp:{CameraUp} topY:{topUnit.transform.position.y}");
        // Debug.Log($"CameDown:{CameraDown} bottomY:{bottomUnit.transform.position.y}");
        if (CameraUp > topUnit.mtransform.position.y)
        {
            bottomUnit.mtransform.position = new Vector3(CameraX, topUnit.mtransform.position.y + CameraSize);
            int temp = topid;
            topid = bottomid;
            bottomid = midid;
            midid = temp;
            ApplyTrans();
            CheckHeight(topUnit);
        }
        else if (CameraDown < bottomUnit.mtransform.position.y)
        {
            topUnit.mtransform.position = new Vector3(CameraX, bottomUnit.mtransform.position.y - CameraSize);
            int temp = bottomid;
            bottomid = topid;
            topid = midid;
            midid = temp;
            ApplyTrans();
            CheckHeight(bottomUnit);
        }
    }

    private void ApplyTrans()
    {
        topUnit.mtransform = backgroundTrans[topid].transform;
        midUnit.mtransform = backgroundTrans[midid].transform;
        bottomUnit.mtransform = backgroundTrans[bottomid].transform;
    }

    private void Init()
    {
        topUnit=new BackgroundUnit(backgroundTrans[0].transform,levelManager);
        midUnit=new BackgroundUnit(backgroundTrans[1].transform,levelManager);
        bottomUnit=new BackgroundUnit(backgroundTrans[2].transform,levelManager);
    }

    private void CheckHeight(BackgroundUnit unit)
    {
        for (int i = 0; i < transHeight.Count; i++)
        {
            var height = transHeight[i];
            if (Mathf.Abs(unit.mtransform.position.y - height) <= CameraSize / 3f)
            {
                // Debug.Log($"{unit.mtransform.name}切换为过渡图{i}  {unit.mtransform.position.y }" );
                unit.ChangeState(true, i);
                return;
            }
        }

        for (int i = transHeight.Count - 1; i >= 0; i--)
        {
            var height = transHeight[i];
            if(unit.mtransform.position.y>=height)
            {
                // Debug.Log($"{unit.mtransform.name}切换到正常图{i+1} {unit.mtransform.position.y }");
                unit.ChangeState(false, i+1);
                return;
            }
        }
        unit.ChangeState(false, 0);
    }
}
[Serializable]
public class BackgroundUnit
{
    public Transform mtransform;
    public BackgroundUnitID unitManager => mtransform.GetComponent<BackgroundUnitID>();
    private LevelManager levelManager;

    public bool ifTransition
    {
        get
        {
            return mtransform.GetComponent<BackgroundUnitID>().ifTransition;
        }
        
        set { mtransform.GetComponent<BackgroundUnitID>().ifTransition = value; }
    }
    public int id{
        get
        {
            return mtransform.GetComponent<BackgroundUnitID>().ID;
        }
        
        set { mtransform.GetComponent<BackgroundUnitID>().ID = value; }
    }

    public BackgroundUnit(Transform mtransform,LevelManager levelManager)
    {
        this.mtransform = mtransform;
        this.levelManager = levelManager;
    }

    public void ChangeState(bool ifTransition, int id)
    {
        if (this.ifTransition && ifTransition)
        {
            Debug.LogError("同一张图两次处于过渡状态");
        }

        this.ifTransition = ifTransition;
        if (ifTransition)
        {
            if (id == 0)
            {
                unitManager.DisableAll();
                unitManager.transition0.gameObject.SetActive(true);
            }
            else if (id == 1)
            {
                unitManager.DisableAll();
                unitManager.transition1.gameObject.SetActive(true);
            }
            else if (id == 2)
            {
                unitManager.DisableAll();
                unitManager.transition2.gameObject.SetActive(true);
            }

            return;
        }
        this.id = id;
        levelManager.HeightLevel = id;
        unitManager.DisableAll();
        switch (id)
        {
            case 0:
                unitManager.height0.gameObject.SetActive(true);
                break;
            case 1:
                unitManager.height1.gameObject.SetActive(true);
                break;
            case 2:
                unitManager.height2.gameObject.SetActive(true);
                break;
            case 3:
                unitManager.height3.gameObject.SetActive(true);
                break;
            default:
                Debug.LogWarning($"不存在正常场景图{id}");
                break;
        }
    }
}


//top
//mid
//botttom