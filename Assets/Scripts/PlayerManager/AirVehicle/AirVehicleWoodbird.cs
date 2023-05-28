using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Cinemachine;

public class AirVehicleWoodbird : AirVehicleBase
{
    [Header("Ä¾ÄñÅäÖÃ")]
    public float flyStrength = 5;
    public float flyInterval = 1;
    public float glideInternal = 1;
    public float maxVelocity_Y = -2;
    [SerializeField] CinemachineVirtualCamera vcam;

    [Header("Debug")]    
    public bool isHard = false;
    [SerializeField] float hardHorizontalStrength = 5;
    [SerializeField] float hardHorizontalInterval = 1;
    [SerializeField] Color color;
    SkeletonAnimation ska;
    CinemachineFramingTransposer vcamTransposer;
    
    Spine.AnimationState.TrackEntryDelegate cc;
    float flyIntervalCount = 0;
    float glideLastTime = 0;
    bool flyEnd = true;
    AudioManager audioManager;
    LevelManager levelManager;
    private void Awake()
    {
        ska = GetComponent<SkeletonAnimation>();
        vcamTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Start()
    {
        audioManager = Services.ServiceLocator.Get<AudioManager>();
    }

    private void OnEnable()
    {
        levelManager = Services.ServiceLocator.Get<LevelManager>();
        vcamTransposer.m_TrackedObjectOffset = new Vector3(0, 0, 0);
    }

    void Update()
    {
        ska.skeleton.R = color.r;
        ska.skeleton.G = color.g;
        ska.skeleton.B = color.b;
        flyIntervalCount += Time.deltaTime;
        if (isHard)
        {
            var hAxis = Input.GetAxisRaw("Horizontal");

            if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) && flyIntervalCount >= hardHorizontalInterval)
            {
                flyIntervalCount = 0;
                ska.AnimationState.SetAnimation(0, "ÆËÒí", false);
                audioManager.PlaySound("Fly");
                flyEnd = false;
                cc = delegate
                {
                    flyEnd = true;
                    ska.AnimationState.Complete -= cc;
                };
                ska.AnimationState.Complete += cc;
                rb.velocity = new Vector2(hAxis * hardHorizontalStrength, flyStrength);
                transform.rotation = Quaternion.Euler(new Vector3(0, hAxis > 0 ? 0 : -180, 0));
            }
        }
        else
        {
            HorizontalMove();
        }
        if (Input.GetButtonDown("Fire1") && flyIntervalCount >= flyInterval)
        {
            ska.AnimationState.SetAnimation(0, "ÆËÒí", false);
            audioManager.PlaySound("Fly");
            flyEnd = false;
            cc = delegate
            {
                flyEnd = true;
                ska.AnimationState.Complete -= cc;
            };
            ska.AnimationState.Complete += cc;
            flyIntervalCount = 0;
            rb.velocity = new Vector2(rb.velocity.x, flyStrength);
        }
    }

    private void FixedUpdate()
    {
        if (flyEnd)
        {
            if (Input.GetButton("Fire2"))
            {
                if (ska.AnimationName != "»¬Ïè")
                {
                    if (Time.time - glideLastTime > glideInternal)
                    {
                        glideLastTime = Time.time;
                        audioManager.PlaySound("Fly");
                        rb.velocity = new Vector2(rb.velocity.x, 2);
                        ska.AnimationState.SetAnimation(0, "»¬Ïè", true);
                    }
                    
                }
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(maxVelocity_Y, rb.velocity.y));
            }
            else
            {
                if (ska.AnimationName != "ÏÂ×¹")
                {
                    ska.AnimationState.SetAnimation(0, "ÏÂ×¹", false);
                }
            }
        }
    }
}
