using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Raiders.Editors
{
    [CustomEditor(typeof(RankManager))]
    public class RankEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var manager = (RankManager)target;

            if (GUILayout.Button("Reset rank"))
                manager.ResetRank();
                
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Increment rank"))
                manager.IncrementRank();
            
            GUILayout.Space(10);

            if (GUILayout.Button("Demcrement rank"))
                manager.DecrementRank();
            
            GUILayout.EndHorizontal();
        }
    }
}