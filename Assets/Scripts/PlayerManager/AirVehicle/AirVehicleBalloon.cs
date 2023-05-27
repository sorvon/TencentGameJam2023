using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVehicleBalloon : AirVehicleBase
{
    [Header("����������")]
    public float balloonStrength = 15;
    public float maxFuel = 5;

    float currentFuel;
    void Awake()
    {
        currentFuel = maxFuel;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(currentFuel);
        HorizontalMove();
        if (Input.GetButton("Fire1") && currentFuel > 0 )
        {
            currentFuel -= Time.deltaTime;
            rb.AddForce(new Vector2(0, balloonStrength));
        }
        else
        {
            currentFuel = Mathf.Clamp(currentFuel + Time.deltaTime, -1000, maxFuel);
        }
    }
}