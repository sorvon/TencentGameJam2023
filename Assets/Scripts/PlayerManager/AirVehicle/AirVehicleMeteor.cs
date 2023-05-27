using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVehicleMeteor : AirVehicleBase
{
    [Header("星星配置")]
    public float meteorStrength = 15;
    public float defauleVelocity_Y = 10;

    void FixedUpdate()
    {
        HorizontalMove();
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(defauleVelocity_Y, rb.velocity.y));
        if (Input.GetButton("Fire1"))
        {
            rb.AddForce(new Vector2(0, meteorStrength));
        }
    }
}
