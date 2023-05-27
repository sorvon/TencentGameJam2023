using UnityEngine;

namespace Services
{
    public class ObjectManager : Service
    {
        private ObjectManagerCore core;

        protected override void Awake()
        {
            base.Awake();
            core = new ObjectManagerCore(transform);
        }

        /// <summary>
        /// 激活一个游戏物体，若对象池中的对象用完，创建一个对象并添加到对象池中，再激活
        /// </summary>
        /// <param name="eObject">要激活的游戏物体对应的枚举</param>
        /// <param name="position">位置</param>
        /// <param name="eulerAngles">欧拉角</param>
        /// <param name="parent">将激活的游戏物体设为某个游戏物体的子物体，默认情况下是对象池的子物体</param>
        /// <returns>被激活的游戏物体</returns>
        public IMyObject Activate(EObject eObject, Vector3 position, Vector3 eulerAngles, Transform parent = null)
            => core.Activate(eObject, position, eulerAngles, parent);

        /// <summary>
        /// (用于2D游戏)激活一个游戏物体，若对象池中的对象用完，创建一个对象并添加到对象池中，再激活
        /// </summary>
        /// <param name="eObject">要激活的游戏物体对应的枚举</param>
        /// <param name="position">位置</param>
        /// <param name="eulerAngleZ">z方向欧拉角</param>
        /// <param name="parent">将激活的游戏物体设为某个游戏物体的子物体，默认情况下是对象池的子物体</param>
        /// <returns>被激活的游戏物体</returns>
        public IMyObject Activate(EObject eObject, Vector3 position, float eulerAngleZ = 0f, Transform parent = null)
            => core.Activate(eObject, position, new Vector3(0f, 0f, eulerAngleZ), parent);
        public IMyObject Activate(EObject eObject, Vector3 position, Transform parent)
            => core.Activate(eObject, position, Vector3.zero, parent);

        /// <summary>
        /// 预生成物体
        /// </summary>
        /// <param name="eObject">要生成的游戏物体对应的枚举</param>
        /// <param name="count">数量</param>
        public void PreCreate(EObject eObject, int count)
            => core.PreCreate(eObject, count);

        public GameObject GetPrefab(EObject eObject) => core.objectDict[eObject];
        public GameObject[] GetPrefabs(params EObject[] eObjects)
        {
            GameObject[] gameObjects = new GameObject[eObjects.Length];
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i] = GetPrefab(eObjects[i]);
            }
            return gameObjects;
        }

        public GameObject Clone(EObject eObject) => 
            ObjectPoolUtility.Clone(GetPrefab(eObject)).gameObject;
    }
}

