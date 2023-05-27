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
    private List<EObject> combineTypes;
    private float lastCombinePos = 0f;
    private float combineInterval = 0f;

    #region 摄像机变量

    private float CurrentHeight => cameraTrans.position.y; //当前摄像机竖直坐标
    private float CameraUp => cameraTrans.position.y + camera.orthographicSize; //摄像机上边界
    private float CameraDown => cameraTrans.position.y - camera.orthographicSize;
    private float CameraLeft => cameraTrans.position.x - camera.orthographicSize * camera.aspect; //摄像机左边界
    private float CameraRight => cameraTrans.position.x + camera.orthographicSize * camera.aspect;
    private float SpawnUp => CameraUp + 2 * camera.orthographicSize; //生成上边界：摄像机上边界+2个屏幕高度
    private float SpawnDown => CameraDown - 2 * camera.orthographicSize;

    #endregion

    public int currentLevel = 0;

    protected override void Start()
    {
        base.Start();
        Init();
    }

    public void Update()
    {
        //GenerateCollection();
        Generate();
    }

    private void Init()
    {
        camera = cameraTrans.GetComponent<Camera>();
        InitGeneratorConfig();
    }

    private void InitGeneratorConfig()
    {
        collections = new Dictionary<EObject, Collection>();
        CollectionGenerateInterval config = Resources.Load<CollectionGenerateInterval>("CollectionIntervals");
        foreach (var interval in config._intervals)
        {
            Collection collection = new Collection(interval.type, interval.interval);
            collections.Add(interval.type, collection);
        }
        currentType = EObject.Cloud;
        ReadLevelConfig();
    }

    private void ReadLevelConfig()
    {
        CombineConfig config = Resources.Load<CombineConfig>($"CombineConfig{currentLevel}");
        combineInterval = config.obstacleInterval;
        combineTypes = new List<EObject>();
        foreach (var comb in config.combineTypes)
        {
            combineTypes.Add(comb);
        }
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

    private void Generate()
    {
        if (!(Mathf.Abs(lastCombinePos - CurrentHeight) > combineInterval))
            return;
        
        float generateY, generateX;
        int tryCount = 0;
        EObject combination = combineTypes[Random.Range(0, combineTypes.Count)];
        
        
        if ((lastCombinePos - CurrentHeight) > 0)
        {
            // Debug.Log("DownGenerate");
            do
            {
                tryCount++;
                generateX = cameraTrans.position.x;
                generateY = Random.Range(CameraDown, SpawnDown);
            } while (Physics2D.OverlapCircle(new Vector2(generateX, generateY), 4.5f, LayerMask.GetMask("EnvItem")) &&
                     tryCount < 25); //防止卡死
        }
        else
        {
            // Debug.Log("UpGenerate");
            do
            {
                tryCount++;
                generateX = cameraTrans.position.x;
                generateY = Random.Range(CameraUp, SpawnUp);
            } while (Physics2D.OverlapCircle(new Vector2(generateX, generateY), 4.5f, LayerMask.GetMask("EnvItem")) &&
                     tryCount < 25);
        }
        
        lastCombinePos = CurrentHeight;
        if (tryCount >= 25)
        {
            Debug.LogWarning("不能在指定范围内生成不与其他物体不重叠的组合");
            return;
        }
        
        //若距离上一次收集物生成未达到间隔则将组合中的收集物Disable
        Collection co = collections[currentType];
        Transform combTrans =objectManager.Activate(combination, new Vector2(generateX, generateY)).Transform;
        if (!(Mathf.Abs(co.lastPos - CurrentHeight) > co.interval))
        {
            combTrans.Find("Collection").gameObject.SetActive(false);
            
        }
        else
        {
            co.lastPos = CurrentHeight;
        }
    }
}