using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CombineConfig", menuName = "CombineConfig", order = 1)]
public class CombineConfig: ScriptableObject
{
    [Header("障碍物")]
    [Label("障碍物间隔")]public float obstacleInterval; 
    public List<EObject> combineTypes;
}