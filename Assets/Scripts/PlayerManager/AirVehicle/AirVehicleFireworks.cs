using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Spine.Unity;
public class AirVehicleFireworks : AirVehicleBase
{

    [Header("—Ãª≈‰÷√")]
    public float fireStrength = 20;
    public float directionSensitivity = 60;
    [SerializeField] ParticleSystem particle;
    [SerializeField] CinemachineVirtualCamera vcam;

    [Header("Debug")]
    [SerializeField] float fireDirection;
    CinemachineFramingTransposer vcamTransposer;
    SkeletonAnimation ska;
    AudioManager audioManager;
    // Start is called before the first frame update
    void Awake()
    {
        fireDirection = 0;
        vcamTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        ska = GetComponent<SkeletonAnimation>();
        
    }

    private void Start()
    {
        audioManager = Services.ServiceLocator.Get<AudioManager>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        fireDirection = Mathf.Clamp(fireDirection - hAxis * directionSensitivity * Time.deltaTime, -30, 30);
        transform.rotation = Quaternion.Euler(0, 0, fireDirection);

        if (Input.GetButton("Fire1")
            || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.A))
        {
            if (ska.AnimationName != "∑…")
            {
                audioManager.PlaySound("Fireworks", AudioPlayMode.Wait);
                particle.Play();
                ska.AnimationState.SetAnimation(0, "∑…", true);
            }
            var radian = Mathf.Deg2Rad * (fireDirection + 90);
            rb.AddForce(new Vector2(fireStrength * Mathf.Cos(radian), 9.81f * rb.gravityScale + fireStrength * Mathf.Sin(radian)));
            vcamTransposer.m_TrackedObjectOffset = Vector3.Lerp(vcamTransposer.m_TrackedObjectOffset,
                new Vector3(0, 3, 0), Time.deltaTime);
        }
        else
        {
            //rb.velocity = new Vector2(0, rb.velocity.y);
            if (ska.AnimationName != "drop")
            {
                audioManager.PauseSound("Fireworks");
                particle.Stop(true);
                ska.AnimationState.SetAnimation(0, "drop", false);
            }
            vcamTransposer.m_TrackedObjectOffset = Vector3.Lerp(vcamTransposer.m_TrackedObjectOffset,
                new Vector3(0, -3, 0), Time.deltaTime);
        }
    }

    private void OnDisable()
    {
        audioManager.StopSound("Fireworks");
    }
}
