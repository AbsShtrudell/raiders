using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BuildingsGraphEditor))]
public class BuildingsGraphEditorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var editor = (BuildingsGraphEditor)target;

        if (GUILayout.Button("Add Node"))
        {
            editor.AddBuilding();
        }
        if (GUILayout.Button("Random bind"))
        {
            editor.BindRandomBuildings();
        }
    }
}