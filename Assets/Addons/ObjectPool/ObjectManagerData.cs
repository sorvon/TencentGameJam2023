using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    [CreateAssetMenu(fileName = "ObjectManagerData", menuName = "ObjectManagerData", order = 1)]
    internal class ObjectManagerData : ScriptableObject
    {
        [SerializeField]
        internal EObjectPrefabPair[] datas;
    }
}