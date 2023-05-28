using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVehicleMeteor : AirVehicleBase
{
    [Header("星星配置")]
    public float meteorStrength = 15;
    public float defauleVelocity_Y = 10;
    [SerializeField] CinemachineVirtualCamera vcam;

    CinemachineFramingTransposer vcamTransposer;
    AudioManager audioManager;
    private void Awake()
    {
        vcamTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
       
    }

    private void Start()
    {
        audioManager = Services.ServiceLocator.Get<AudioManager>();
    }
    private void OnEnable()
    {
        audioManager.PlaySound("Meteor");
    }
    void FixedUpdate()
    {
        HorizontalMove();
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(defauleVelocity_Y, rb.velocity.y));
        if (Input.GetButton("Fire1"))
        {
            rb.AddForce(new Vector2(0, meteorStrength));
        }
        vcamTransposer.m_TrackedObjectOffset = Vector3.Lerp(vcamTransposer.m_TrackedObjectOffset,
                new Vector3(0, 3, 0), Time.deltaTime);
    }

    private void OnDisable()
    {
        audioManager.StopSound("Meteor");
    }
}
