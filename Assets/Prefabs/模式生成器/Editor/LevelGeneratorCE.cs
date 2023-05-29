using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelDesign
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LevelGenerator))]
    public class LevelGeneratorCE : Editor
    {
        public override void OnInspectorGUI()
        {
            LevelGenerator handler = target as LevelGenerator;
            EditorGUILayout.LabelField("快速工具", EditorStyles.boldLabel);
            if (GUILayout.Button("生成关卡"))
            {
                handler.GenerateLevel();
            }
            GUILayout.Space(4);
            base.DrawDefaultInspector();
        }
    }
}