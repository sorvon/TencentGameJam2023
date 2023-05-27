using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    [CreateAssetMenu(fileName = "ObjectPoolData", menuName = "ObjectPool", order = 1)]
    internal class ObjectManagerData : ScriptableObject
    {
        [SerializeField]
        internal EObjectPrefabPair[] datas;
    }
}