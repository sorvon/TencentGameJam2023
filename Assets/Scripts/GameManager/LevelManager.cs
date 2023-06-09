using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class LevelManager : Service
{
    [SerializeField] private List<int> collectionCountConfig;
    [SerializeField] private TextMeshProUGUI heightNumberText;
    [SerializeField] private TextMeshProUGUI collectNumberText;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameObject handbookObject;
    [SerializeField] private Image progressMask;
    [SerializeField] private Image progressImage;
    [SerializeField] private GameObject progressIcon;
    [SerializeField] private Sprite[] targetSprites;
    private int collectionCount = 0;
    [Other] private SceneController _sceneController;

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
                if (Level < targetSprites.Length)
                {
                    progressImage.sprite = targetSprites[Level];
                }
                
                
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
        CheckEnd();
    }

    private void UpdateHeightText()
    {
        // if (!heightNumberText)
        //     return;
        //Debug.Log( $"{Height:N}");
        heightNumberText.text = $"{Height:N}";
    }

    private void CheckEnd()
    {
        if (Height > 1200)
        {
            _sceneController.LoadNextScene();
        }
    }

    private void UpdateCollectionText()
    {
        if (!collectNumberText)
            return;
        if (Level < collectionCountConfig.Count)
        {
            collectNumberText.text = $"{collectionCount}/{collectionCountConfig[Level]}";
            progressMask.fillAmount = (float)collectionCount / collectionCountConfig[Level];
            var icon =  progressIcon.transform.GetChild(0);
            var p1 = progressIcon.transform.GetChild(1);
            var p2 = progressIcon.transform.GetChild(2);
            icon.position = Vector3.Lerp(p1.position, p2.position, progressMask.fillAmount);
        }
        else
        {
            if (! collectNumberText.gameObject.activeSelf)
            {
                collectNumberText.gameObject.SetActive(true);
            }
            collectNumberText.text = $"{collectionCount}";
            progressMask.fillAmount = 1;
            var icon = progressIcon.transform.GetChild(0);
            var p1 = progressIcon.transform.GetChild(1);
            var p2 = progressIcon.transform.GetChild(2);
            icon.position = p2.position;
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