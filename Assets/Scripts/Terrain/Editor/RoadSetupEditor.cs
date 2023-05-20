using UnityEditor;
using UnityEngine;

namespace Raiders.Editors
{
    [CustomEditor(typeof(RoadSetup))]
    public class RoadSetupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var roadSetup = (RoadSetup)target;

            if (GUILayout.Button("Setup"))
            {
                roadSetup.Setup();
            }
        }
    }
}