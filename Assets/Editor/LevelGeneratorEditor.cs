using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor((typeof(LevelGenerator)), true)]
public class LevelGeneratorEditor : Editor
{
    private LevelGenerator generator;

    private void Awake()
    {
        generator = (LevelGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button(("Generate Map")))
        {
            generator.GenerateMap();
        }
    }
}
