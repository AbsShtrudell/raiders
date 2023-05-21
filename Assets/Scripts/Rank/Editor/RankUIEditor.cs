using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Raiders.Editors
{
    [CustomEditor(typeof(RankUI))]
    public class RankUIEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var ui = (RankUI)target;

            if (GUILayout.Button("Refresh"))
                ui.Refresh();
        }
    }
}