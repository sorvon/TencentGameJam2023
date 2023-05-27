using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnvGenerator : Service
{
    [SerializeField] private Transform cameraTrans;
    
    

    [Other] private ObjectManager objectManager;
    private Dictionary<EObject,Collection> collections;
    private Dictionary<EObject, List<float>> spawnPosDict;
    private Camera camera;
    private EObject currentType;
    

    private float CurrentHeight => cameraTrans.position.y;//当前摄像机竖直坐标
    private float CameraUp => cameraTrans.position.y + camera.orthographicSize;//摄像机上边界
    private float CameraDown => cameraTrans.position.y - camera.orthographicSize;
    private float CameraLeft => cameraTrans.position.x - camera.orthographicSize * camera.aspect;//摄像机左边界
    private float CameraRight => cameraTrans.position.x + camera.orthographicSize * camera.aspect;
    private float SpawnUp => CameraUp + 2*camera.orthographicSize;//生成上边界：摄像机上边界+2个屏幕高度
    private float SpawnDown => CameraDown - 2*camera.orthographicSize;


    protected override void Start()
    {
        base.Start();
        Init();
    }

    public void Update()
    {
        // Debug.Log(CameraUp);
        // Debug.Log(CameraDown);
        // Debug.Log(CameraLeft);
        // Debug.Log(CameraRight);
        // Debug.Log(SpawnUp);
        // Debug.Log(SpawnDown);
        GenerateCollection();
    }

    private void Init()
    {
        camera = cameraTrans.GetComponent<Camera>();
        InitCollections();
    }

    private void GenerateCollection()
    {
        
        Collection co = collections[currentType];
        if (!(Mathf.Abs(co.lastPos - CurrentHeight) > co.interval))
            return;
        
        float generateY,generateX;
        
        if ((co.lastPos - CurrentHeight) > 0)
        {
            Debug.Log("DownGenerate");
            generateY = Random.Range(CameraDown,SpawnDown);
        }
        else
        {
            Debug.Log("UpGenerate");
            generateY = Random.Range(CameraUp, SpawnUp);
        }

        co.lastPos = CurrentHeight;
        generateX = Random.Range(CameraLeft * 0.9f, CameraRight * 0.9f);
        objectManager.Activate(currentType, new Vector2(generateX, generateY));
    }

    private void InitCollections()
    {
        collections = new Dictionary<EObject, Collection>();
        CollectionGenerateInterval intervalData = Resources.Load<CollectionGenerateInterval>("CollectionIntervals");
        foreach (var interval in intervalData._intervals)
        {
            Collection collection = new Collection(interval.type, interval.interval);
            collections.Add(interval.type,collection);
        }

        currentType = EObject.Cloud;
    }
}