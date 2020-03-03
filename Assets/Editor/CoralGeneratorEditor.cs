using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CoralGenerator))]
public class CoralGeneratorEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        
        
        CoralGenerator gen = (CoralGenerator)target;
        if (GUILayout.Button("Build coral")) {
            gen.GenerateCoral(1.0f);
        }
        if (GUILayout.Button("Remove coral")) {
            gen.RemoveChildren();
        }
    }
}
