using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : Service
{
    [SerializeField] private List<int> collectionCountConfig;
    [SerializeField] private TextMeshProUGUI heightNumberText;
    [SerializeField] private TextMeshProUGUI collectNumberText;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameObject handbookObject; 
    private int collectionCount = 0;

    /// <summary>
    /// 当前收集物数量
    /// </summary>
    public int CollectionCount
    {
        get { return collectionCount; }
        set
        {
            if (value == collectionCount||value<0)
                return;
           
            collectionCount = value;
            if (Level < collectionCountConfig.Count && collectionCount >= collectionCountConfig[Level])
            {
                if (handbookObject != null)
                {
                    handbookObject.SetActive(true);
                    handbookObject.GetComponent<HandbookManager>().SetHandbookSprite(Level);
                    handbookObject.GetComponent<Animator>().SetTrigger("Open");
                }
                Level++;
                collectionCount = 0;
            }
            UpdateCollectionText();

        }
    }

    private int level = 0;

    /// <summary>
    /// 当前等级
    /// </summary>
    public int Level
    {
        get { return level; }
        private set
        {
            if (value == level || value > collectionCountConfig.Count )
                return;
            level = value;
            OnLevelUpInt?.Invoke(level);
            OnLevelUp?.Invoke();
            UpdateCollectionText();

        }
    }

    public float Height =>playerManager. transform.position.y;

    private int heightLevel;
    public int HeightLevel
    {
        get
        {
            return heightLevel;
        }
        set
        {
            if(value<=heightLevel)
                return;
            heightLevel = value;
            OnHeightLevelInt?.Invoke(value);
            Debug.Log($"当前海拔等级来到{value}");
        }
    }

    /// <summary>
    /// 无参数的升级事件
    /// </summary>
    public UnityAction OnLevelUp;

    /// <summary>
    /// 有参数的升级事件
    /// </summary>
    public UnityAction<int> OnLevelUpInt;

    public UnityAction<int> OnHeightLevelInt;

    protected override void Start()
    {
        base.Start();
        CollectionCount = 0;
        Level = 0;
        UpdateCollectionText();
    }

    private void Update()
    {
        UpdateHeightText();
    }

    private void UpdateHeightText()
    {
        // if (!heightNumberText)
        //     return;
        //Debug.Log( $"{Height:N}");
        heightNumberText.text = $"{Height:N}";
    }

    private void UpdateCollectionText()
    {
        if (!collectNumberText)
            return;
        if (Level < collectionCountConfig.Count)
        {
            collectNumberText.text = $"{collectionCount}/{collectionCountConfig[Level]}";
        }
        else
        {
            collectNumberText.text = $"{collectionCount}";
        }
        
    }

    /// <summary>
    /// 测试时使用
    /// </summary>
    public void LevelUp()
    {
        Level++;
    }
}