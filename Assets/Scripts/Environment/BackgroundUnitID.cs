using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BackgroundUnitID : MonoBehaviour
{
    public int ID = 0;
    public bool ifTransition=false;

    public Transform height0;
    public Transform transition0;
    public Transform height1;
    public Transform transition1;
    public Transform height2;
    public Transform transition2;
    public Transform height3;

    private void Awake()
    {
        height0 = transform.Find("Height0");
        height1 = transform.Find("Height1");
        height2 = transform.Find("Height2");
        height3 = transform.Find("Height3");
        transition0 = transform.Find("Transition0");
        transition1 = transform.Find("Transition1");
        transition2=transform.Find("Transition2");
    }

    public void DisableAll()
    {
        height0.gameObject.SetActive(false);
        height1.gameObject.SetActive(false);
        height2.gameObject.SetActive(false);
        height3.gameObject.SetActive(false);
        transition0.gameObject.SetActive(false);
        transition1.gameObject.SetActive(false);
        transition2.gameObject.SetActive(false);
    }
}
