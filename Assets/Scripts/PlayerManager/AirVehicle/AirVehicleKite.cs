using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public class AirVehicleKite : AirVehicleBase
{
    [Header("��������")]
    public float maxVelocity_Y = -5;
    [Header("Debug")]
    SkeletonAnimation ska;

    AudioManager audioManager;
    private void Awake()
    {
        audioManager = Services.ServiceLocator.Get<AudioManager>();
        ska = GetComponent<SkeletonAnimation>();
    }
    void Update()
    {
        HorizontalMove();
        if (Input.GetButton("Fire1") || Input.GetButton("Fire2"))
        {
            if (ska.AnimationName != "����")
            {
                audioManager.PlaySound("OpenKite");
                ska.AnimationState.SetAnimation(0, "����", true);
            }
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(maxVelocity_Y, rb.velocity.y));
        }
        else
        {
            if (ska.AnimationName != "����")
            {
                ska.AnimationState.SetAnimation(0, "����", false);
            }
        }
    }
}
