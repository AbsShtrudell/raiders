using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Raiders.Editors
{
    [CustomEditor(typeof(BuildingsGraphEditor))]
    public class BuildingsGraphEditorInspector : Editor
    {
        private const float matrixElementWidth = 20f;

        private BuildingsGraphEditor editor;
        private BitArray bindMatrix;

        private int nodeCount => editor.graph.Nodes.Count;
        private int selectedIndex;

        private void OnEnable()
        {
            editor = (BuildingsGraphEditor)target;
            CreateBindMatrix();
            InitializeBindMatrix();

            Undo.undoRedoPerformed += ayaya;
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= ayaya;
        }

        private void ayaya()
        {
            CreateBindMatrix();
            InitializeBindMatrix();

        }

        private void CreateBindMatrix()
        {
            bindMatrix = new BitArray(nodeCount * nodeCount, false);
        }

        private void InitializeBindMatrix()
        {
            for (int i = 0; i < nodeCount; i++)
            {
                for (int j = 0; j < nodeCount; j++)
                {
                    if (i == j) continue;

                    bindMatrix[j + i * nodeCount] = editor.graph.Nodes[i].Adjacents.Contains(editor.graph.Nodes[j].Index);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+", GUILayout.Width(matrixElementWidth)))
            {
                editor.AddBuilding();
                CreateBindMatrix();
                InitializeBindMatrix();
            }
            for (int i = 0; i < nodeCount; i++)
            {
                DrawBuildingButton(i);
            }
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < nodeCount; i++)
            {
                EditorGUILayout.BeginHorizontal();
                DrawBuildingButton(i);
                EditorGUILayout.LabelField("", GUILayout.Width(0.5f));
                for (int j = 0; j < nodeCount; j++)
                {
                    if (i == j)
                        EditorGUILayout.LabelField("", GUILayout.Width(matrixElementWidth));
                    else
                    {
                        bindMatrix[j + i * nodeCount] = EditorGUILayout.Toggle(bindMatrix[j + i * nodeCount], GUILayout.Width(matrixElementWidth));

                        bindMatrix[i + j * nodeCount] = bindMatrix[j + i * nodeCount];
                    }

                }
                EditorGUILayout.EndHorizontal();
            }


            if (GUILayout.Button("Bind"))
            {
                Undo.RegisterCompleteObjectUndo(editor, "ed");
                foreach (var node in editor.graph.Nodes)
                {
                    Undo.RegisterCompleteObjectUndo(node.Value, "ed");
                }
                for (int i = 0; i < nodeCount; i++)
                {
                    for (int j = i + 1; j < nodeCount; j++)
                    {
                        editor.ChangeBindStatus(bindMatrix[j + i * nodeCount], i, j);
                    }
                }
                Undo.SetCurrentGroupName("Change bindings");
            }
        }

        private void DrawBuildingButton(int i)
        {
            if (GUILayout.Button(i.ToString(), GUILayout.Width(matrixElementWidth)))
            {
                selectedIndex = i;

                GenericMenu menu = new GenericMenu();

                menu.AddItem(new GUIContent("Ping"), false, PingBuilding);
                menu.AddItem(new GUIContent("Select"), false, SelectBuilding);
                menu.ShowAsContext();
            }
        }

        private void PingBuilding()
        {
            Building b = editor.graph.Nodes[selectedIndex].Value;

            EditorGUIUtility.PingObject(b);
            SceneView.lastActiveSceneView.LookAt(b.transform.position);
        }

        private void SelectBuilding()
        {
            Selection.activeGameObject = editor.graph.Nodes[selectedIndex].Value.gameObject;
            SceneView.FrameLastActiveSceneView();
        }

    }
}