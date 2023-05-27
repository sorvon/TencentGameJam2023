using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AirVehicleFireworks : AirVehicleBase
{

    [Header("—Ãª≈‰÷√")]
    public float fireStrength = 20;
    public float directionSensitivity = 60;
    [SerializeField] CinemachineVirtualCamera vcam;

    [Header("Debug")]
    [SerializeField] float fireDirection;
    CinemachineFramingTransposer vcamTransposer;
    // Start is called before the first frame update
    void Awake()
    {
        fireDirection = 0;
        vcamTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        fireDirection = Mathf.Clamp(fireDirection - hAxis * directionSensitivity * Time.deltaTime, -45, 45);
        transform.rotation = Quaternion.Euler(0, 0, fireDirection);

        if (Input.GetButton("Fire1")
            || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.A))
        {
            var radian = Mathf.Deg2Rad * (fireDirection + 90);
            rb.AddForce(new Vector2(fireStrength * Mathf.Cos(radian), 9.81f + fireStrength * Mathf.Sin(radian)));
            vcamTransposer.m_TrackedObjectOffset = Vector3.Lerp(vcamTransposer.m_TrackedObjectOffset,
                new Vector3(0, 3, 0), Time.deltaTime);
        }
        else
        {
            //rb.velocity = new Vector2(0, rb.velocity.y);
            vcamTransposer.m_TrackedObjectOffset = Vector3.Lerp(vcamTransposer.m_TrackedObjectOffset,
                new Vector3(0, -3, 0), Time.deltaTime);
        }
    }
}
