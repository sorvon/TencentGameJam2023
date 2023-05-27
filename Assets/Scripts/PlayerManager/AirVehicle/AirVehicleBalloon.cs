using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AirVehicleBalloon : AirVehicleBase
{
    [Header("����������")]
    public float balloonStrength = 15;
    public float maxFuel = 5;
    [SerializeField] CinemachineVirtualCamera vcam;

    CinemachineFramingTransposer vcamTransposer;
    float currentFuel;
    void Awake()
    {
        currentFuel = maxFuel;
        vcamTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
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
            vcamTransposer.m_TrackedObjectOffset = Vector3.Lerp(vcamTransposer.m_TrackedObjectOffset,
                new Vector3(0, 3, 0), Time.deltaTime);
        }
        else
        {
            currentFuel = Mathf.Clamp(currentFuel + Time.deltaTime, -1000, maxFuel);
            vcamTransposer.m_TrackedObjectOffset = Vector3.Lerp(vcamTransposer.m_TrackedObjectOffset,
                new Vector3(0, -3, 0), Time.deltaTime);
        }
    }
}
