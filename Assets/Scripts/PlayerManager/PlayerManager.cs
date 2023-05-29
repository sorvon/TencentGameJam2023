using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using TMPro;
using Spine.Unity;
using Object = System.Object;

public class PlayerManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] GameObject[] airVehicleList;
    [SerializeField] float invincibleTime = 3;
    [SerializeField] float obstacleStrength = 3;
    [SerializeField] BoxCollider2D cTrigger;

    [Header("Debug")]
    [SerializeField] GameObject currentAirVehicle;
    [SerializeField] int airVehicleIndex = 0;
    [SerializeField] Rigidbody2D rb;

    AudioManager audioManager;
    private ObjectManager _objectManager;
    bool isInvincible = false;
    private LevelManager levelManager;
    private int CurrentLevel => levelManager.Level;
    bool onWind = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        for (int i = 0; i < airVehicleList.Length; i++)
        {
            if (i == airVehicleIndex)
            {
                currentAirVehicle = airVehicleList[i];
            }
            airVehicleList[i].SetActive(i == airVehicleIndex);
        }
    }

    private void Start()
    {
        audioManager = ServiceLocator.Get<AudioManager>();
        levelManager = ServiceLocator.Get<LevelManager>();
        _objectManager = ServiceLocator.Get<ObjectManager>();
        levelManager.OnLevelUpInt += OnLevelUp;
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -250 && !onWind)
        {
            onWind = true;
            audioManager.PlaySound("Wind");
            cTrigger.size = new Vector2(60, 1500);
        }
        else if (transform.position.y > -100)
        {
            if (onWind == true)
            {
                onWind = false;
                cTrigger.size = new Vector2(60, 15);
            }
        }
        if (onWind)
        {
            var v =  transform.position;
            v.y = Mathf.Lerp(v.y, -80, Time.deltaTime * 0.5f);
            transform.position = v;
            rb.velocity = Vector2.zero;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (onWind)
        {
            return;
        }
        if (collision.CompareTag("Collection"))
        {
            collision.GetComponent<Services.IMyObject>().Recycle();
            levelManager.CollectionCount++;
            audioManager.PlaySound("Pick");
        }
        if (collision.CompareTag("Obstacle") && !isInvincible)
        {
            var direction2D = -rb.velocity.normalized;
            Debug.Log(direction2D * obstacleStrength);
            rb.velocity = direction2D * obstacleStrength;
            levelManager.CollectionCount--;
            if (currentAirVehicle.TryGetComponent(out SkeletonAnimation ska))
            {
                StartCoroutine(SpineSkeletonFlash(ska));
            }
        }
    }

    IEnumerator SpineSkeletonFlash(SkeletonAnimation skeletonAnimation)
    {
        float invincibleTimeCount = 0;
        bool toTransparent = true;
        isInvincible = true;
        while(invincibleTimeCount < invincibleTime)
        {
            invincibleTimeCount += 0.3f;
            skeletonAnimation.skeleton.A = toTransparent ? 0.7f : 1;
            toTransparent = !toTransparent;
            yield return new WaitForSeconds(0.5f);
        }
        skeletonAnimation.skeleton.A = 1;
        isInvincible = false;
    }

    private void OnLevelUp(int level)
    {
        Debug.Log($"升级，当前等级{level}");
        for (int i = 0; i < airVehicleList.Length; i++)
        {
            if (i == CurrentLevel)
            {
                currentAirVehicle = airVehicleList[i];
            }
            airVehicleList[i].SetActive(i == CurrentLevel);
        }
    }
}
