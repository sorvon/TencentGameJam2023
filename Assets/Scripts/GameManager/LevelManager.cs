using System.Collections;
using System.Collections.Generic;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : Service
{
    [SerializeField] List<int> collectionCountConfig;
    [SerializeField] TextMeshProUGUI heightNumberText;
    [SerializeField] TextMeshProUGUI collectNumberText;

    private int collectionCount = 0;
    /// <summary>
    /// 当前收集物数量
    /// </summary>
    public int CollectionCount
    {
        get
        {
            return collectionCount;
        }
        set
        {
            collectionCount = value;
            if (collectionCount >= collectionCountConfig[Level])
            {
                Level++;
                
            }
        }
    }

    private int level = 0;
    /// <summary>
    /// 当前等级
    /// </summary>
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            if (value == level)
                return;
            level = value;
            OnLevelUpInt?.Invoke(level);
            OnLevelUp?.Invoke();
        }
    }
    
    public UnityAction OnLevelUp;
    public UnityAction<int> OnLevelUpInt;
    protected override void Start()
    {
        base.Start();
    }
    
}
