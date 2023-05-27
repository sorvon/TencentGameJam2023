using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AirVehicleBase : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] float horizontalVelocity = 1;
    // Start is called before the first frame update
    void Awake()
    {
        if (rb == null)
        {
            rb = transform.parent.GetComponent<Rigidbody2D>();
        }
    }

    protected void HorizontalMove()
    {
        var hAxis = Input.GetAxisRaw("Horizontal");
        
        rb.velocity = new Vector2(hAxis * horizontalVelocity, rb.velocity.y);
        //transform.DOKill();
        //transform.DO
    }
}
