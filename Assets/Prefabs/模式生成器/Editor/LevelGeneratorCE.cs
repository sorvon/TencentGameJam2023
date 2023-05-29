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
            EditorGUILayout.LabelField("���ٹ���", EditorStyles.boldLabel);
            if (GUILayout.Button("���ɹؿ�"))
            {
                handler.GenerateLevel();
            }
            GUILayout.Space(4);
            base.DrawDefaultInspector();
        }
    }
}