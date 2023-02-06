using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Raiders.Editors
{
    [CustomEditor(typeof(Squad))]
    public class SquadEditor : Editor
    {
       public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var squad = target as Squad;
            if (GUILayout.Button("Spawn Soldiers"))
            {
                squad.SpawnEmptySoldiers();
            }
        }
    }
}

