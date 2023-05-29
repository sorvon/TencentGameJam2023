using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVehicleMeteor : AirVehicleBase
{
    [Header("星星配置")]
    public float meteorStrength = 15;
    public float defauleVelocity_Y = 10;
    public float boostRate = 0.4f;
    [SerializeField] CinemachineVirtualCamera vcam;

    CinemachineFramingTransposer vcamTransposer;
    AudioManager audioManager;
    LevelManager levelManager;
    private void Awake()
    {
        vcamTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
       
    }

    private void Start()
    {
        audioManager = Services.ServiceLocator.Get<AudioManager>();
        levelManager = Services.ServiceLocator.Get<LevelManager>();
    }
    private void OnEnable()
    {
        if (!audioManager)
        {
            audioManager = Services.ServiceLocator.Get<AudioManager>();
        }
        audioManager.PlaySound("Meteor");
    }
    void FixedUpdate()
    {
        HorizontalMove();
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(defauleVelocity_Y * (1 + boostRate * levelManager.CollectionCount), rb.velocity.y));
        if (Input.GetButton("Fire1"))
        {
            rb.AddForce(new Vector2(0, meteorStrength));
        }
        vcamTransposer.m_TrackedObjectOffset = Vector3.Lerp(vcamTransposer.m_TrackedObjectOffset,
                new Vector3(0, 3, 0), Time.deltaTime);
    }

    private void OnDisable()
    {
        if(audioManager)
            audioManager.StopSound("Meteor");
    }
}
