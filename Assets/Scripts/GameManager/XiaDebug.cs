using System;
using System.Collections;
using System.Collections.Generic;
using LevelDesign;
using Services;
using UnityEngine;

public class XiaDebug : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private LevelGenerator _generator;
    new private Camera camera;
    private Transform cameraTrans;
    private GameManager manager;
    private LevelManager levelManager;
    private EnvGenerator generator;
    private AudioManager audiomanager;
    private ObjectManager objectManager;
    private SceneController sceneController;

    private Dictionary<EObject, GameObject> e2obj;
    public List<EObject> anchorTypes;
    public List<GameObject> anchorObjs;
    public List<EObject> floatTypes;
    public List<GameObject> floatObjs;
    private void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameraTrans = camera.transform;
        // manager = ServiceLocator.Get<GameManager>();
        levelManager = ServiceLocator.Get<LevelManager>();
        generator = ServiceLocator.Get<EnvGenerator>();
        audiomanager = ServiceLocator.Get<AudioManager>();
        objectManager = ServiceLocator.Get<ObjectManager>();
        sceneController = ServiceLocator.Get<SceneController>();
        InitGenerator();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            cameraTrans.Translate(Vector2.up * (moveSpeed * Time.deltaTime));
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            cameraTrans.Translate(Vector2.down * (moveSpeed * Time.deltaTime));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            // manager.RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            levelManager.LevelUp();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            objectManager.Activate(EObject.Bird, cameraTrans.position+new Vector3(0,0,10));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            sceneController.LoadNextScene();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            _generator.RuntimeGenerate(cameraTrans.position,anchorTypes,floatTypes,true,EObject.Collection);
        }
    }

    private void InitGenerator()
    {
        e2obj = new Dictionary<EObject, GameObject>();
        for (int i = 0; i < floatTypes.Count; i++)
        {
            e2obj.Add(floatTypes[i],floatObjs[i]);
        }
        for (int i = 0; i < anchorTypes.Count; i++)
        {
            e2obj.Add(anchorTypes[i],anchorObjs[i]);
        }
        // _generator.InitE2ObjDict(e2obj);
    }
}
