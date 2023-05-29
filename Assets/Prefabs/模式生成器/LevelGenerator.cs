using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelDesign
{
    public class LevelGenerator : MonoBehaviour
    {
        [Header("收集物锚点对象")] public GameObject collection;
        public Vector2 collectionSize = new Vector2(1f, 1f);

        [Header("限域锚点")] public Transform bottomLeft;
        public Transform topRight;

        public float MinX
        {
            get
            {
                if (useCliff)
                {
                    return bottomLeft.position.x + (topRight.position.x - bottomLeft.position.x) / 10f;
                }
                else
                {
                    return bottomLeft.position.x;
                }
            }
        }

        public float MaxX
        {
            get
            {
                if (useCliff)
                {
                    return topRight.position.x - (topRight.position.x - bottomLeft.position.x) / 10f;
                }
                else
                {
                    return topRight.position.x;
                }
            }
        }

        public float CenterX
        {
            get => (MinX + MaxX) / 2f;
        }

        public float MinY
        {
            get => bottomLeft.position.y;
        }

        public float MaxY
        {
            get => topRight.position.y;
        }

        public float CenterY
        {
            get => (MinY + MaxY) / 2f;
        }

        public Vector2 MinXY
        {
            get => new Vector2(MinX, MinY);
        }

        public Vector2 MaxXY
        {
            get => new Vector2(MaxX, MaxY);
        }

        public Vector2 CenterXY
        {
            get => new Vector2(CenterX, CenterY);
        }

        public Vector2 MinMaxX
        {
            get => new Vector2(MinX, MaxX);
        }

        public Vector2 MinMaxY
        {
            get => new Vector2(MinY, MaxY);
        }

        public Vector2 SizeXY
        {
            get => new Vector2(MaxX - MinX, MaxY - MinY);
        }

        public Bounds Bound
        {
            get => new Bounds(CenterXY, SizeXY);
        }
        

        [Header("[编辑模式]目标对象")] [SerializeField]
        private GameObject editorTarget;

        [Header("悬崖壁")] public bool useCliff;

        [Header("悬崖障碍物")] public List<GameObject> anchorObjects;
        [Header("浮动障碍物")] public List<GameObject> floatObjects;

        [Header("间隙规格")] [Range(0f, 17.8f)] public float gapX = 3f;
        [Range(0f, 10f)] public float gapY = 5f;

        [Header("生成数量")] [Range(0, 10)] public int generateCount = 3;

        [Header("最大重试次数")] [Range(0, 1000)] public int retryMax = 10;

        [Header("默认标签和层")] public string defaultTag = "Combine";
        public string defaultLayer = "Combine";

        private List<Bounds> generatedBounds = new();
        public Dictionary<EObject, GameObject> e2obj;
        private ObjectManager _objectManager;

        public void ClearTarget(GameObject target)
        {
            generatedBounds.Clear();
            if (target == null)
            {
                return;
            }

            List<GameObject> toDestroy = new();
            for (int c = 0; c < target.transform.childCount; c++)
            {
                Transform child = target.transform.GetChild(c);
                if (child.name == bottomLeft.name || child.name == topRight.name)
                {
                    continue;
                }

                toDestroy.Add(child.gameObject);
            }

            foreach (var item in toDestroy)
            {
                DestroyImmediate(item);
            }
        }

        // 检查是否处与障碍区域不相交
        private bool InRegion(Bounds bounds)
        {
            return Bound.Contains(bounds.min) && Bound.Contains(bounds.max);
        }

        private bool InRegion(Bounds bounds, Transform generatePos)
        {
            Bounds cameraBound = new Bounds(CenterXY + (Vector2)generatePos.position, SizeXY);
            return cameraBound.Contains(bounds.min) && cameraBound.Contains(bounds.max);
        }

        // 检查是否处与障碍区域不相交
        private bool IntersectWithObstacle(Bounds bounds)
        {
            foreach (var bd in generatedBounds)
            {
                if (bounds.Intersects(bd))
                {
                    return true;
                }
            }
            Debug.Log("与障碍相交");
            return false;
        }

        private void GenerateCollection(Transform parent)
        {
            Vector2 xy = Vector2.zero;
            Bounds bounds = new(xy, collectionSize);
            bool check = false;

            for (int i = 0; i < retryMax; i++)
            {
                xy = new Vector2(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY));
                bounds = new Bounds(xy, collectionSize);

                // 检查是否超出边界
                // 检查是否处与障碍区域相交
                if (InRegion(bounds) && !IntersectWithObstacle(bounds))
                {
                    check = true;
                    break;
                }
            }

            var newCollection = Instantiate(collection);
            newCollection.name = newCollection.name.Replace("(Clone)", "");
            xy = check ? xy : CenterXY;
            newCollection.transform.position = xy;
            newCollection.transform.SetParent(parent);
            bounds = new(xy, collectionSize);
            bounds.Expand(new Vector2(gapX, gapY) * 2f);
            generatedBounds.Add(bounds);
        }

        private void GenerateObstacle(Transform parent)
        {
            int thresh = anchorObjects.Count;
            int tot = anchorObjects.Count + floatObjects.Count;
            for (int i = 0; i < generateCount; i++)
            {
                int sel = Random.Range(0, tot);
                if (sel < thresh)
                {
                    GenerateAnchorObstacle(sel, parent);
                }
                else
                {
                    GenerateFloatObstacle(sel - thresh, parent);
                }
            }
        }

        #region MyRegion

        /// <summary>
        /// 运行时使用
        /// </summary>
        /// <returns></returns>
        public void RuntimeGenerateObstacles(List<GenerateUnit> units, List<EObject> anchorTypes,List<EObject> floatTypes, Transform generatePos)
        {
            generatedBounds.Clear();
            int thresh = anchorTypes.Count;
            int tot = anchorTypes.Count + floatTypes.Count;
            for (int i = 0; i < generateCount; i++)
            {
                int sel = Random.Range(0, tot);
                if (sel < thresh)
                {
                    RuntimeGenerateAnchorObstacle(sel, units, anchorTypes, generatePos);
                }
                else
                {
                    RuntimeGenerateFloatObstacle(sel - thresh, units, floatTypes, generatePos);
                }
            }
        }

        private void RuntimeGenerateAnchorObstacle(int sel, List<GenerateUnit> units, List<EObject> anchortypes,
            Transform generatePos)
        {
            for (int i = 0; i < retryMax; i++)
            {
                bool anchorRight = Random.Range(0, 2) == 1; // false as left, true as right

                Debug.Log($"dictSize:{e2obj.Count} sel:{sel} anchorTypes{anchortypes[sel]}");
                Vector2 bdSize = e2obj[anchortypes[sel]].GetComponent<SpriteRenderer>().bounds.size;
                Vector2 center = new Vector2(anchorRight ? MaxX - 0.5f * bdSize.x : MinX + 0.5f * bdSize.x,
                    Random.Range(MinY, MaxY));
                center += (Vector2)generatePos.position;
                Bounds bounds = new Bounds(center, bdSize);

                // 检查是否超出边界
                // 检查是否处与障碍区域相交
                if (!InRegion(bounds,generatePos) || IntersectWithObstacle(bounds))
                {
                    continue;
                }

                //var newAnchorObstacle = Instantiate(anchorObjects[sel]);
                //newAnchorObstacle.name = newAnchorObstacle.name.Replace("(Clone)", "");
                var newPos = new Vector2(0, 0);
                Quaternion rotation = Quaternion.Euler(0, 0, 0);
                ;
                if (anchorRight)
                {
                    // newAnchorObstacle.transform.rotation = Quaternion.Euler(0, 180, 0);
                    rotation = Quaternion.Euler(0, 180, 0);
                }

                // newAnchorObstacle.transform.position = center;
                newPos = center ;
                units.Add(new GenerateUnit(newPos, anchortypes[sel], rotation));

                bounds.Expand(new Vector2(gapX, gapY) * 2f);
                generatedBounds.Add(bounds);
                break;
            }
        }

        private void RuntimeGenerateFloatObstacle(int sel, List<GenerateUnit> units, List<EObject> floatTypes,
            Transform generatePos)
        {
            for (int i = 0; i < retryMax; i++)
            {
                Vector2 bdSize = e2obj[floatTypes[sel]].GetComponent<SpriteRenderer>().bounds.size;
                Vector2 center = new Vector2(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY));
                center += (Vector2)generatePos.position;
                Bounds bounds = new Bounds(center, bdSize);

                // 检查是否超出边界
                // 检查是否处与障碍区域相交
                if (!InRegion(bounds,generatePos) || IntersectWithObstacle(bounds))
                {
                    continue;
                }

                var newPos = Vector2.zero;
                // var newFloatObstacle = Instantiate(floatObjects[sel]);
                // newFloatObstacle.name = newFloatObstacle.name.Replace("(Clone)", "");
                // newFloatObstacle.transform.position = center;
                newPos = center;
                units.Add(new GenerateUnit(newPos,floatTypes[sel],Quaternion.Euler(0,0,0)));
                    // newFloatObstacle.transform.SetParent(parent);

                bounds.Expand(new Vector2(gapX, gapY) * 2f);
                generatedBounds.Add(bounds);
                break;
            }
        }

        public void InitE2ObjDict(Dictionary<EObject, GameObject> dict)
        {
            e2obj = dict;
        }
        #endregion


        private void GenerateAnchorObstacle(int sel, Transform parent)
        {
            for (int i = 0; i < retryMax; i++)
            {
                bool anchorRight = Random.Range(0, 2) == 1; // false as left, true as right

                Vector2 bdSize = anchorObjects[sel].GetComponent<SpriteRenderer>().bounds.size;
                Vector2 center = new Vector2(anchorRight ? MaxX - 0.5f * bdSize.x : MinX + 0.5f * bdSize.x,
                    Random.Range(MinY, MaxY));
                Bounds bounds = new Bounds(center, bdSize);

                // 检查是否超出边界
                // 检查是否处与障碍区域相交
                if (!InRegion(bounds) || IntersectWithObstacle(bounds))
                {
                    continue;
                }

                var newAnchorObstacle = Instantiate(anchorObjects[sel]);
                newAnchorObstacle.name = newAnchorObstacle.name.Replace("(Clone)", "");
                if (anchorRight)
                {
                    newAnchorObstacle.transform.rotation = Quaternion.Euler(0, 180, 0);
                }

                newAnchorObstacle.transform.position = center;
                newAnchorObstacle.transform.SetParent(parent);

                bounds.Expand(new Vector2(gapX, gapY) * 2f);
                generatedBounds.Add(bounds);
                break;
            }
        }

        private void GenerateFloatObstacle(int sel, Transform parent)
        {
            for (int i = 0; i < retryMax; i++)
            {
                Vector2 bdSize = floatObjects[sel].GetComponent<SpriteRenderer>().bounds.size;
                Vector2 center = new Vector2(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY));
                Bounds bounds = new Bounds(center, bdSize);

                // 检查是否超出边界
                // 检查是否处与障碍区域相交
                if (!InRegion(bounds) || IntersectWithObstacle(bounds))
                {
                    continue;
                }

                var newFloatObstacle = Instantiate(floatObjects[sel]);
                newFloatObstacle.name = newFloatObstacle.name.Replace("(Clone)", "");
                newFloatObstacle.transform.position = center;
                newFloatObstacle.transform.SetParent(parent);

                bounds.Expand(new Vector2(gapX, gapY) * 2f);
                generatedBounds.Add(bounds);
                break;
            }
        }

        public GameObject Assemable(GameObject target)
        {
            if (target == null)
            {
                target = new GameObject("NewLevel");
            }

            target.tag = defaultTag;
            target.layer = LayerMask.NameToLayer(defaultLayer);
            if (target.GetComponent<MyObject>() == null)
            {
                target.AddComponent<MyObject>();
            }

            if (target.GetComponent<BoxCollider2D>() == null)
            {
                target.AddComponent<BoxCollider2D>();
            }

            target.GetComponent<BoxCollider2D>().isTrigger = true;
            return target;
        }

        private void MergeBoundsTo(BoxCollider2D coll)
        {
            float minX = float.MaxValue, maxX = float.MinValue;
            float minY = float.MaxValue, maxY = float.MinValue;
            foreach (var bd in generatedBounds)
            {
                minX = Mathf.Min(minX, bd.min.x);
                maxX = Mathf.Max(maxX, bd.max.x);
                minY = Mathf.Min(minY, bd.min.y);
                maxY = Mathf.Max(maxY, bd.max.y);
            }

            Vector2 center = new Vector2(0.5f * (minX + maxX), 0.5f * (minY + maxY)) +
                             (Vector2)(transform.position - coll.transform.position);
            Vector2 size = new Vector2(maxX - minX - 2f * gapX, maxY - minY - 2f * gapY);

            coll.offset = center;
            coll.size = size;
        }

        // 编辑模式生成用
        [ContextMenu("生成关卡")]
        public GameObject GenerateLevel()
        {
            GameObject obj = null;
            if (editorTarget is not null)
            {
                ClearTarget(editorTarget);
                obj = editorTarget;
            }

            obj = Assemable(obj);

            GenerateObstacle(obj.transform);
            GenerateCollection(obj.transform);
            MergeBoundsTo(obj.GetComponent<BoxCollider2D>());

            return obj;
        }

        // 运行时生成用
        public GameObject GenerateLevel(GameObject target = null)
        {
            if (target is not null)
            {
                ClearTarget(target);
            }

            target = Assemable(target);

            GenerateObstacle(target.transform);
            GenerateCollection(target.transform);
            MergeBoundsTo(target.GetComponent<BoxCollider2D>());

            return target;
        }

        public void RuntimeGenerate(Transform cameraTrans,List<EObject> anchorTypes,List<EObject> floatTypes)
        {
            if (!_objectManager)
                _objectManager = ServiceLocator.Get<ObjectManager>();
            List<GenerateUnit> units = new List<GenerateUnit>();
            RuntimeGenerateObstacles(units,anchorTypes,floatTypes,cameraTrans);
            foreach (var unit in units)
            {
                Transform unitTrans = _objectManager.Activate(unit.type, unit.unitPos).Transform;
                unitTrans.rotation = unit.rotation;  
            }
        }
    }
}