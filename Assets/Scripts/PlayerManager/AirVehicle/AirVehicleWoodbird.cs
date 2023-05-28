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
    [SerializeField] bool isHard = false;
    [SerializeField] float hardHorizontalStrength = 5;

    Spine.AnimationState.TrackEntryDelegate cc;
    float flyIntervalCount = 0;
    bool flyEnd = true;
    private void Awake()
    {
        ska = GetComponent<SkeletonAnimation>();
    }

    void Update()
    {
        if (isHard)
        {
            var hAxis = Input.GetAxisRaw("Horizontal");
            if (hAxis != 0)
            {
                ska.AnimationState.SetAnimation(0, "ÆËÒí", false);
                flyEnd = false;
                cc = delegate
                {
                    flyEnd = true;
                    ska.AnimationState.Complete -= cc;
                };
                ska.AnimationState.Complete += cc;
                rb.velocity = new Vector2(hAxis * hardHorizontalStrength, flyStrength);
            }
            if (hAxis > 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else if (hAxis < 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
            }
        }
        else
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
