using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVehicleWoodbird : AirVehicleBase
{
    [Header("Ä¾ÄñÅäÖÃ")]
    public float flyStrength = 5;
    public float flyInterval = 1;

    float flyIntervalCount = 0;

    void Update()
    {
        HorizontalMove();
        flyIntervalCount += Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && flyIntervalCount >= flyInterval)
        {
            flyIntervalCount = 0;
            rb.velocity = new Vector2(rb.velocity.x, flyStrength);
        }
    }
}
