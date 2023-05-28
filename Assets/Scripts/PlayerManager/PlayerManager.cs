using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Spine.Unity;
public class PlayerManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] int[] collectionCountConfig;//
    [SerializeField] GameObject[] airVehicleList;
    [SerializeField] TextMeshProUGUI heightNumberText;//
    [SerializeField] TextMeshProUGUI collectNumberText;//
    [SerializeField] float invincibleTime = 3;

    [Header("Debug")]
    [SerializeField] GameObject currentAirVehicle;
    [SerializeField] int airVehicleIndex = 0;
    [SerializeField] Rigidbody2D rb;
    int collectionCount = 0;//
    bool isInvincible = false;
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
        if (collectNumberText != null)
        {
            collectNumberText.text = string.Format("{0}/{1}", collectionCount, collectionCountConfig[airVehicleIndex]);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (heightNumberText != null)
        {
            heightNumberText.text = string.Format("{0:N}", transform.position.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Collection"))
        {
            collision.GetComponent<Services.IMyObject>().Recycle();
            if (airVehicleIndex < collectionCountConfig.Length)
            {
                collectionCount++;
                if (collectionCount == collectionCountConfig[airVehicleIndex])
                {
                    collectionCount = 0;
                    airVehicleIndex++;
                    for (int i = 0; i < airVehicleList.Length; i++)
                    {
                        if (i == airVehicleIndex)
                        {
                            currentAirVehicle = airVehicleList[i];
                        }
                        airVehicleList[i].SetActive(i == airVehicleIndex);
                    }
                }
                if (airVehicleIndex < collectionCountConfig.Length)
                {
                    if (collectNumberText != null)
                    {
                        collectNumberText.text = string.Format("{0}/{1}", collectionCount, collectionCountConfig[airVehicleIndex]);
                    }
                }
                else
                {
                    if (collectNumberText != null)
                    {
                        collectNumberText.text = string.Format("{0}", collectionCount);
                    }
                }
            }
            
        }
        if (collision.CompareTag("Obstacle") && !isInvincible)
        {
            rb.velocity = Vector2.zero;
            collectionCount = Mathf.Max(collectionCount - 1, 0);
            if (collectNumberText != null)
            {
                collectNumberText.text = string.Format("{0}/{1}", collectionCount, collectionCountConfig[airVehicleIndex]);
            }
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
}
