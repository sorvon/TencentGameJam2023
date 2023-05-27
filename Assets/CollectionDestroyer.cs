using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using Unity.VisualScripting;
using UnityEngine;

public class CollectionDestroyer : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log($"TriggerEnter,tag:{other.gameObject.tag}");
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        // Debug.Log($"TriggerExit,tag:{other.gameObject.tag}");
        if (other.CompareTag("Collection")|| other.CompareTag("Obstacle"))
            other.GetComponent<IMyObject>().Recycle();
    }
}