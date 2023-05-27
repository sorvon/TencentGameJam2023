using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;

public class EnvGenerator : Service
{
    [Other] private ObjectManager objectManager;
    private Dictionary<EObject, bool> collectionEnables;

    protected override void Start()
    {
        base.Start();
        Init();
        objectManager = ServiceLocator.Get<ObjectManager>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            objectManager.Activate(EObject.Cloud, new Vector3(0, 0, 0));
        }
    }

    private void Init()
    {
        collectionEnables = new Dictionary<EObject, bool>()
        {
            { EObject.Kite, true },
            { EObject.Feather, false },
            { EObject.Kindling, false },
            { EObject.Star, false }
        };
    }
}
