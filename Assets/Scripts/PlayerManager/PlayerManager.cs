using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] int[] collectionCountConfig;
    [SerializeField] GameObject[] airVehicleList;
    [SerializeField] TextMeshProUGUI heightNumberText;
    [SerializeField] TextMeshProUGUI collectNumberText;

    [Header("Debug")]
    [SerializeField] int airVehicleIndex = 0;
    [SerializeField] Rigidbody2D rb;
    int collectionCount = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        for (int i = 0; i < airVehicleList.Length; i++)
        {
            airVehicleList[i].SetActive(i == airVehicleIndex);
        }
        collectNumberText.text = string.Format("{0}/{1}", collectionCount, collectionCountConfig[airVehicleIndex]);
    }
    // Update is called once per frame
    void Update()
    {
        heightNumberText.text = string.Format("{0:N}", transform.position.y);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
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
                        airVehicleList[i].SetActive(i == airVehicleIndex);
                    }
                }
                collectNumberText.text = string.Format("{0}/{1}", collectionCount, collectionCountConfig[airVehicleIndex]);
            }
            else
            {
                collectNumberText.text = string.Format("{0}", collectionCount);
            }
        }
        if (collision.CompareTag("Obstacle"))
        {
            rb.velocity = Vector2.zero;
            collectionCount = Mathf.Max(collectionCount - 1, 0);
            collectNumberText.text = string.Format("{0}/{1}", collectionCount, collectionCountConfig[airVehicleIndex]);
        }
    }
}
