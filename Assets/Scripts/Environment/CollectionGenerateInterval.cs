using System;
using System.Collections.Generic;
using Services;
using UnityEngine;
[CreateAssetMenu(fileName = "CollectionIntervals", menuName = "CollectionIntervals", order = 1)]
public class CollectionGenerateInterval: ScriptableObject
{
    public List<CollectionInterval> _intervals;
}

[Serializable]
public struct CollectionInterval
{
    [Label("收集物类型")]
    public EObject type;
    [Label("生成间隔")]
    public float interval;
}