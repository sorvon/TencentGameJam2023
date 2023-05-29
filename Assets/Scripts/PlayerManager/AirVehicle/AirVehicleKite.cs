using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public class AirVehicleKite : AirVehicleBase
{
    [Header("∑ÁÛ›≈‰÷√")]
    public float maxVelocity_Y = -5;
    public float glideInternal = 0.5f;
    [Header("Debug")]
    SkeletonAnimation ska;

    AudioManager audioManager;
    LevelManager levelManager;
    float glideLastTime = 0;
    private void Awake()
    {
        ska = GetComponent<SkeletonAnimation>();
    }

    private void Start()
    {
        audioManager = Services.ServiceLocator.Get<AudioManager>();
    }
    private void OnEnable()
    {
        levelManager = Services.ServiceLocator.Get<LevelManager>();

    }
    void Update()
    {
        HorizontalMove();
        if ((Input.GetButton("Fire1") || Input.GetButton("Fire2")))
        {
            if (ska.AnimationName != "ª¨œË")
            {
                if (Time.time - glideLastTime > glideInternal)
                {
                    glideLastTime = Time.time;
                    rb.velocity = new Vector2(rb.velocity.x, 2);
                    audioManager.PlaySound("OpenKite");
                    ska.AnimationState.SetAnimation(0, "ª¨œË", true);
                }
            }
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(maxVelocity_Y, rb.velocity.y));
        }
        else
        {
            if (ska.AnimationName != "µÙ¬‰")
            {
                ska.AnimationState.SetAnimation(0, "µÙ¬‰", false);
            }
        }
    }
}
