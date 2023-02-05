using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Raiders
{
    [CustomEditor(typeof(Soldier))]
    public class SoldierEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var soldier = target as Soldier;
            if (GUILayout.Button("Change items"))
            {
                soldier.ChangeItems();
            }
        }
    }
}