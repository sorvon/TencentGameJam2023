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
    [Other] private LevelManager levelManager;
    private Dictionary<EObject, Collection> collections;
    private Dictionary<int, EObject> level2collection;

    new private Camera camera;

    private EObject currentType => level2collection[currentLevel]; //当前收集物的种类
    private List<EObject> combineTypes;
    private float lastCombinePos = 0f;
    private float combineInterval = 0f;

    #region 摄像机变量

    private float CurrentHeight => cameraTrans.position.y; //当前摄像机竖直坐标
    private float CameraUp => cameraTrans.position.y + camera.orthographicSize; //摄像机上边界
    private float CameraDown => cameraTrans.position.y - camera.orthographicSize;
    private float CameraLeft => cameraTrans.position.x - camera.orthographicSize * camera.aspect; //摄像机左边界
    private float CameraRight => cameraTrans.position.x + camera.orthographicSize * camera.aspect;
    private float SpawnUpDown => CameraUp + 0.5f * camera.orthographicSize; //生成上边界：摄像机上边界+2个屏幕高度
    private float SpawnUpUp => CameraUp + 1f * camera.orthographicSize;
    private float SpawnDownDown => CameraDown - 2f * camera.orthographicSize;
    private float SpawnDownUp => CameraDown - 1.5f * camera.orthographicSize;

    #endregion

    public int currentLevel => levelManager.Level;

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

        levelManager.OnLevelUp += ReadLevelConfig;
    }

    private void InitGeneratorConfig()
    {
        collections = new Dictionary<EObject, Collection>();
        CollectionGenerateInterval config = Resources.Load<CollectionGenerateInterval>("CollectionIntervals");
        foreach (var interval in config._intervals)
        {
            Collection collection = new Collection(interval.type, interval.interval);
            if (! collections.ContainsKey(interval.type))
            {
                collections.Add(interval.type, collection);
            }
            
        }

        level2collection = new Dictionary<int, EObject>()
        {
            { 0, EObject.Kite },
            { 1, EObject.Feather },
            { 2, EObject.Firecracker },
            { 3, EObject.Kindling },
            { 4, EObject.Star },
            { 5, EObject.Star }
        };
        // currentType = level2collection[0];
        ReadLevelConfig();
    }

    private void ReadLevelConfig()
    {
        //TODO: 等配置
        if(currentLevel>2)
            return;
        CombineConfig config = Resources.Load<CombineConfig>($"CombineConfig{currentLevel}");
        combineInterval = config.obstacleInterval;
        combineTypes = new List<EObject>();
        foreach (var comb in config.combineTypes)
        {
            combineTypes.Add(comb);
        }
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
                generateY = Random.Range(SpawnDownDown, SpawnDownUp);
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
                generateY = Random.Range(SpawnUpDown, SpawnUpUp);
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
        Transform combTrans = objectManager.Activate(combination, new Vector2(generateX, generateY)).Transform;
        if ((Mathf.Abs(co.lastPos - CurrentHeight) > co.interval))
        {
            Transform coTrans = combTrans.Find("Collection");
            if (coTrans)
                objectManager.Activate(currentType, coTrans.position);
        }
    }

    // public void LevelUp(int level)
    // {
    //     // currentType = level2collection[currentLevel];
    //     ReadLevelConfig();
    // }
}