using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using TMPro;
using Spine.Unity;

public class HandbookManager : MonoBehaviour 
{
    [SerializeField] Sprite[] handbookSprites;
    
    LevelManager levelManager;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        levelManager = Services.ServiceLocator.Get<LevelManager>();
    }

    public void SetHandbookSprite(int index)
    {
        if (index < handbookSprites.Length)
        {
            spriteRenderer.sprite = handbookSprites[index];
        }
        else
        {
            Debug.LogError($"SetHandbookSprite index:{index} is out of handbookSprites Length");
        }
        
    }
}
