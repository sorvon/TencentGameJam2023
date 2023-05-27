using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVehicleKite : AirVehicleBase
{
    [Header("∑ÁÛ›≈‰÷√")]
    public float maxVelocity_Y = -5;

    void Update()
    {
        HorizontalMove();
        if (Input.GetButton("Fire1"))
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(maxVelocity_Y, rb.velocity.y));
        }
    }
}
