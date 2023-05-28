using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Spine.Unity;
using DG.Tweening;
public class AirVehicleBalloon : AirVehicleBase
{
    [Header("热气球配置")]
    public float balloonStrength = 15;
    public float maxFuel = 5;
    [SerializeField] CinemachineVirtualCamera vcam;
    [SerializeField] GameObject fireObject;

    [Header("Debug")]
    public bool isHard = false;
    public float hardHorizontalStrength = 5;
    public float hardHorizontalInterval = 1;
    SkeletonAnimation ska;

    CinemachineFramingTransposer vcamTransposer;
    float currentFuel;
    float hardHorizontalIntervalCount = 0;
    AudioManager audioManager;
    void Awake()
    {
        currentFuel = maxFuel;
        vcamTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        ska = GetComponent<SkeletonAnimation>();
        audioManager = Services.ServiceLocator.Get<AudioManager>();
    }

    private void Update()
    {
        if (isHard)
        {
            var hAxis = Input.GetAxisRaw("Horizontal");
            hAxis = -hAxis;
            hardHorizontalIntervalCount += Time.deltaTime;
            if (hAxis != 0 && hardHorizontalIntervalCount >= hardHorizontalInterval)
            {
                hardHorizontalIntervalCount = 0;
                rb.velocity = new Vector2(hAxis * hardHorizontalStrength, rb.velocity.y);
                transform.rotation = Quaternion.Euler(new Vector3(0, hAxis > 0 ? -180 : 0, 0));
                transform.DOShakeRotation(hardHorizontalInterval, 10);
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isHard)
        {
            HorizontalMove();
        }
        
        if (Input.GetButton("Fire1"))
        {
            if (ska.AnimationName != "飞行")
            {
                audioManager.PlaySound("Fire", AudioPlayMode.Wait);
                ska.AnimationState.SetAnimation(0, "飞行", true);
            }
            currentFuel -= Time.deltaTime;
            var radian = Mathf.Deg2Rad * (transform.rotation.eulerAngles.z+90);

            rb.AddForce(new Vector2(balloonStrength * Mathf.Cos(radian), 
                balloonStrength * Mathf.Sin(radian) + 9.81f * rb.gravityScale));
            vcamTransposer.m_TrackedObjectOffset = Vector3.Lerp(vcamTransposer.m_TrackedObjectOffset,
                new Vector3(0, 3, 0), Time.deltaTime);
            fireObject.SetActive(true);
        }
        else
        {
            if (ska.AnimationName != "下落")
            {
                audioManager.StopSound("Fire");
                ska.AnimationState.SetAnimation(0, "下落", true);
            }
            currentFuel = Mathf.Clamp(currentFuel + Time.deltaTime, -1000, maxFuel);
            vcamTransposer.m_TrackedObjectOffset = Vector3.Lerp(vcamTransposer.m_TrackedObjectOffset,
                new Vector3(0, -3, 0), Time.deltaTime);
            fireObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        audioManager.StopSound("Fire");
    }
}
