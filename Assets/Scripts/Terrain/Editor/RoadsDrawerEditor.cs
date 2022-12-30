using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoadsDrawer))]
public class RoadsDrawerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RoadsDrawer roadsDrawer = (RoadsDrawer)target;

        GUILayout.Space(15);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Draw"))
        {
            roadsDrawer.Draw();
        }

        if (GUILayout.Button("Clear"))
        {
            roadsDrawer.Clear();
        }

        GUILayout.EndHorizontal();
    }
}
