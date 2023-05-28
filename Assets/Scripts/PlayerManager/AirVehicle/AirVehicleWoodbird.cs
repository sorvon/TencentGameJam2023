using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public class AirVehicleWoodbird : AirVehicleBase
{
    [Header("Ä¾ÄñÅäÖÃ")]
    public float flyStrength = 5;
    public float flyInterval = 1;
    public float maxVelocity_Y = -2;

    [Header("Debug")]
    SkeletonAnimation ska;

    Spine.AnimationState.TrackEntryDelegate cc;
    float flyIntervalCount = 0;
    bool flyEnd = true;
    private void Awake()
    {
        ska = GetComponent<SkeletonAnimation>();
    }

    void Update()
    {
        HorizontalMove();
        flyIntervalCount += Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && flyIntervalCount >= flyInterval)
        {
            ska.AnimationState.SetAnimation(0, "ÆËÒí", false);
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
                    rb.velocity = new Vector2(rb.velocity.x, flyStrength / 5);
                    ska.AnimationState.SetAnimation(0, "»¬Ïè", true);
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
