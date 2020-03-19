using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GlobalLifeManager))]
public class GlobalLifeManagerEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        
        
        GlobalLifeManager life = (GlobalLifeManager)target;
        if (GUILayout.Button("Update Life")) {
            life.InitLife();
        }
    }
}
