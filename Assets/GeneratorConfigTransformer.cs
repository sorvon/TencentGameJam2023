using System.Collections;
using System.Collections.Generic;
using LevelDesign;
using Services;
using UnityEngine;

public class GeneratorConfigTransformer : Service
{
    [SerializeField] private LevelGenerator level1config;
    [SerializeField] private LevelGenerator level2config;
    [SerializeField] private LevelGenerator level3config;
    [SerializeField] private LevelGenerator level4config;
    [SerializeField] private List<E2ObjDict> e2Objs;
    private LevelGenerator mainGenerator;
    private Dictionary<EObject, GameObject> e2ObjDict;
    private Dictionary<GameObject, EObject> obj2EDict;
    private Dictionary<int, EObject> level2collection;

    private List<GeneratorConfig> transConfigs;

    private List<EObject> curAnchor;
    private List<EObject> curFloat;

    [Other] private LevelManager levelManager;

    protected override void Start()
    {
        base.Start();
        mainGenerator = GetComponent<LevelGenerator>();
        InitDict();
        transConfigs = new List<GeneratorConfig>();
        ReadConfig(level1config);
        ReadConfig(level2config);
        ReadConfig(level3config);
        ReadConfig(level4config);
        ApplyConfig(0);
        levelManager.OnHeightLevelInt += ApplyConfig;
    }

    private void ReadConfig(LevelGenerator config)
    {
        List<EObject> fl = new List<EObject>();
        List<EObject> an = new List<EObject>();
        foreach (var anchor in config.anchorObjects)
        {
            an.Add(obj2EDict[anchor]);
        }

        foreach (var floatObj in config.floatObjects)
        {
            fl.Add(obj2EDict[floatObj]);
        }

        GeneratorConfig transConfig =
            new GeneratorConfig(fl, an, config.gapX, config.gapY, config.generateCount, config.retryMax,config.useCliff);
        transConfigs.Add(transConfig);
    }

    private void InitDict()
    {
        level2collection= new Dictionary<int, EObject>()
        {
            { 0, EObject.Kite },
            { 1, EObject.Feather },
            { 2, EObject.Firecracker },
            { 3, EObject.Kindling },
            { 4, EObject.Star },
            { 5, EObject.Star }
        };
        e2ObjDict = new Dictionary<EObject, GameObject>();
        obj2EDict = new Dictionary<GameObject, EObject>();
        foreach (var pair in e2Objs)
        {
            e2ObjDict.Add(pair.type, pair.prefab);
            obj2EDict.Add(pair.prefab, pair.type);
        }

        mainGenerator.InitE2ObjDict(e2ObjDict);
    }

    public void ApplyConfig(int level)
    {
        mainGenerator.gapX = transConfigs[level].gapX;
        mainGenerator.gapY = transConfigs[level].gapY;
        mainGenerator.generateCount = transConfigs[level].generateCount;
        mainGenerator.retryMax = transConfigs[level].retryMax;
        mainGenerator.useCliff = transConfigs[level].useCliff;
        curAnchor = transConfigs[level].anchorTypes;
        curFloat = transConfigs[level].floatTypes;
        
    }

    public void DoGenerate(Vector2 pos, bool ifGenerateCollection) =>
        mainGenerator.RuntimeGenerate(pos, curAnchor, curFloat, ifGenerateCollection,level2collection[levelManager.Level]);
}

public struct GeneratorConfig
{
    public GeneratorConfig(List<EObject> floatTypes, List<EObject> anchorTypes, float gapX, float gapY,
        int generateCount, int retryMax,bool useCliff)
    {
        this.floatTypes = floatTypes;
        this.anchorTypes = anchorTypes;
        this.gapX = gapX;
        this.gapY = gapY;
        this.generateCount = generateCount;
        this.retryMax = retryMax;
        this.useCliff = useCliff;
    }

    public List<EObject> floatTypes;
    public List<EObject> anchorTypes;
    public float gapX;
    public float gapY;
    public int generateCount;
    public int retryMax;
    public bool useCliff;
}

[System.Serializable]
public struct E2ObjDict
{
    public EObject type;
    public GameObject prefab;
}