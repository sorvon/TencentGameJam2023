using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public class AirVehicleKite : AirVehicleBase
{
    [Header("∑ÁÛ›≈‰÷√")]
    public float maxVelocity_Y = -5;
    [Header("Debug")]
    SkeletonAnimation ska;

    private void Awake()
    {
        ska = GetComponent<SkeletonAnimation>();
    }
    void Update()
    {
        HorizontalMove();
        if (Input.GetButton("Fire1") || Input.GetButton("Fire2"))
        {
            if (ska.AnimationName != "ª¨œË")
            {
                ska.AnimationState.SetAnimation(0, "ª¨œË", true);
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
