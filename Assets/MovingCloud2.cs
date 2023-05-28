using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCloud2 : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;

    [SerializeField,Label("是否开始时向左")] private bool ifLeft;//是否往左移动

    private void FixedUpdate()
    {
        if (ifLeft)
        {
            transform.Translate(Vector2.left * (Time.deltaTime * moveSpeed));
            if (transform.position.x < leftPoint.position.x)
            {
                ifLeft = false;
            }
        }
        else
        {
            transform.Translate(Vector2.right * (Time.deltaTime * moveSpeed));
            if (transform.position.x > rightPoint.position.x)
            {
                ifLeft = true;
            }
        }
    }
}
