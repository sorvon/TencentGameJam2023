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

    [Header("Debug")]
    [SerializeField] int airVehicleIndex = 0;
    int collectionCount;

    private void Awake()
    {
        for (int i = 0; i < airVehicleList.Length; i++)
        {
            airVehicleList[i].SetActive(i == airVehicleIndex);
        }
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
            if (airVehicleIndex >= collectionCountConfig.Length)
            {
                return;
            }
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
        }
        if (collision.CompareTag("Obstacle"))
        {

        }
    }
}
