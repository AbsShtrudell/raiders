using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Raiders.Editors
{
    [CustomEditor(typeof(Road))]
    public class RoadInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var road = (Road)target;

            if (GUILayout.Button("Rebuild"))
            {
                road.Rebuild();
            }
        }
    }
}