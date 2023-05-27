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
    private Dictionary<EObject, Collection> collections;
    private Dictionary<EObject, List<float>> spawnPosDict;
    private Camera camera;
    private EObject currentType; //当前收集物的种类
    private List<EObject> obstacleTypes;
    private float lastObstaclePos = 0f;
    private float obstacleInterval = 0f;

    #region 摄像机变量

    private float CurrentHeight => cameraTrans.position.y; //当前摄像机竖直坐标
    private float CameraUp => cameraTrans.position.y + camera.orthographicSize; //摄像机上边界
    private float CameraDown => cameraTrans.position.y - camera.orthographicSize;
    private float CameraLeft => cameraTrans.position.x - camera.orthographicSize * camera.aspect; //摄像机左边界
    private float CameraRight => cameraTrans.position.x + camera.orthographicSize * camera.aspect;
    private float SpawnUp => CameraUp + 2 * camera.orthographicSize; //生成上边界：摄像机上边界+2个屏幕高度
    private float SpawnDown => CameraDown - 2 * camera.orthographicSize;

    #endregion


    protected override void Start()
    {
        base.Start();
        Init();
    }

    public void Update()
    {
        GenerateCollection();
        GenerateObstacle();
    }

    private void Init()
    {
        camera = cameraTrans.GetComponent<Camera>();
        InitGeneratorConfig();
    }

    private void InitGeneratorConfig()
    {
        collections = new Dictionary<EObject, Collection>();
        CollectionGenerateInterval intervalData = Resources.Load<CollectionGenerateInterval>("CollectionIntervals");
        foreach (var interval in intervalData._intervals)
        {
            Collection collection = new Collection(interval.type, interval.interval);
            collections.Add(interval.type, collection);
        }

        obstacleInterval = intervalData.obstacleInterval;
        currentType = EObject.Cloud;
        obstacleTypes = new List<EObject>()
        {
            EObject.ThunderCloud,
            EObject.LeftStick,
            EObject.RightStick
        };
    }

    private void GenerateCollection()
    {
        Collection co = collections[currentType];
        if (!(Mathf.Abs(co.lastPos - CurrentHeight) > co.interval))
            return;

        float generateY, generateX;
        int tryCount = 0;
        if ((co.lastPos - CurrentHeight) > 0)
        {
            // Debug.Log("DownGenerate");
            do
            {
                tryCount++;
                generateX = Random.Range(CameraLeft * 0.9f, CameraRight * 0.9f);
                generateY = Random.Range(CameraDown, SpawnDown);
            } while (Physics2D.OverlapCircle(new Vector2(generateX, generateY), 2.5f, LayerMask.GetMask("EnvItem")) &&
                     tryCount < 25); //防止卡死
        }
        else
        {
            // Debug.Log("UpGenerate");
            do
            {
                tryCount++;
                generateX = Random.Range(CameraLeft * 0.9f, CameraRight * 0.9f);
                generateY = Random.Range(CameraUp, SpawnUp);
            } while (Physics2D.OverlapCircle(new Vector2(generateX, generateY), 2.5f, LayerMask.GetMask("EnvItem")) &&
                     tryCount < 25);
        }

        if (tryCount >= 25)
        {
            Debug.LogWarning("不能在指定范围内生成不与其他物体不重叠的收集物");
        }

        co.lastPos = CurrentHeight;

        objectManager.Activate(currentType, new Vector2(generateX, generateY));
    }

    private void GenerateObstacle()
    {
        if (!(Mathf.Abs(lastObstaclePos - CurrentHeight) > obstacleInterval))
            return;
        
        float generateY, generateX;
        int tryCount = 0;
        EObject obstacleType = obstacleTypes[Random.Range(0, obstacleTypes.Count)];
        
        
        if ((lastObstaclePos - CurrentHeight) > 0)
        {
            // Debug.Log("DownGenerate");
            do
            {
                tryCount++;
                SwitchObstacleXPos(obstacleType);
                generateY = Random.Range(CameraDown, SpawnDown);
            } while (Physics2D.OverlapCircle(new Vector2(generateX, generateY), 2.5f, LayerMask.GetMask("EnvItem")) &&
                     tryCount < 25); //防止卡死
        }
        else
        {
            // Debug.Log("UpGenerate");
            do
            {
                tryCount++;
                SwitchObstacleXPos(obstacleType);
                generateY = Random.Range(CameraUp, SpawnUp);
            } while (Physics2D.OverlapCircle(new Vector2(generateX, generateY), 2.5f, LayerMask.GetMask("EnvItem")) &&
                     tryCount < 25);
        }
        
        lastObstaclePos = CurrentHeight;
        if (tryCount >= 25)
        {
            Debug.LogWarning("不能在指定范围内生成不与其他物体不重叠的障碍物");
            return;
        }

        
        objectManager.Activate(obstacleType, new Vector2(generateX, generateY));

        void SwitchObstacleXPos(EObject type)
        {
            switch (obstacleType)
            {
                case EObject.LeftStick:
                    generateX = CameraLeft;
                    break;
                case EObject.RightStick:
                    generateX = CameraRight;
                    break;
                default:
                    generateX = Random.Range(CameraLeft * 0.9f, CameraRight * 0.9f);
                    break;
            }
        }
    }
}