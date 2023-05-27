using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCloud : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    
    private Camera camera;
    private Transform cameraTrans;

    private float CameraLeft => cameraTrans.position.x - camera.orthographicSize * camera.aspect;
    private float CameraRight => cameraTrans.position.x + camera.orthographicSize * camera.aspect;
    private float CameraSize => camera.orthographicSize*camera.aspect;
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameraTrans = camera.GetComponent<Transform>();
    }
    
    void FixedUpdate()
    {
        transform.Translate(Vector2.right * (Time.deltaTime * moveSpeed));
        if (transform.position.x > CameraRight + CameraSize)
        {
            transform.position = new Vector3(CameraLeft - CameraSize,transform.position.y);
        }
    }
}
