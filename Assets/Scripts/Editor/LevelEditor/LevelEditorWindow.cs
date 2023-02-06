using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Raiders.Editors.LevelEditor
{
    public class LevelEditorWindow : EditorWindow
    {
        private List<Node> _nodes = new List<Node>();
        private List<Connection> _connections = new List<Connection>();

        private Node selectedInNode;

        private GUIStyle nodeStyle;
        private GUIStyle selectedNodeStyle;

        private float menuBarHeight = 20f;
        private float inspectorWidth = 200f;
        private Rect menuBar;
        private ResizablePanel inspector;

        [MenuItem("Window/Level Editor")]
        private static void OpenWindow()
        {
            LevelEditorWindow window = GetWindow<LevelEditorWindow>();
            window.titleContent = new GUIContent("Level Editor");
        }

        private void OnEnable()
        {
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("Assets/Resources/Sprites/icons/house-vikings.png") as Texture2D;
            nodeStyle.border = new RectOffset(0, 0, 0, 0);
            selectedNodeStyle = new GUIStyle();
            selectedNodeStyle.normal.background = EditorGUIUtility.Load("Assets/Resources/Sprites/icons/house-rebels.png") as Texture2D;
            selectedNodeStyle.border = new RectOffset(0, 0, 0, 0);

            var resizerStyle = new GUIStyle();
            resizerStyle.normal.background = EditorGUIUtility.Load("icons/d_AvatarBlendBackground.png") as Texture2D;
            var boxStyle = new GUIStyle();
            boxStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/cn entrybackodd.png") as Texture2D;
            inspector = new ResizablePanel(new Rect(0, menuBarHeight, inspectorWidth, position.height), boxStyle, resizerStyle);
        }

        private void OnGUI()
        {
            DrawMenuBar();
            DrawInspector();
            DrawConnections();
            DrawBind();
            DrawNodes();

            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);

            if (GUI.changed) Repaint();
        }

        private void DrawInspector()
        {
            inspector.Draw();
        }

        private void DrawMenuBar()
        {
            menuBar = new Rect(0, 0, position.width, menuBarHeight);

            GUILayout.BeginArea(menuBar, EditorStyles.toolbar);
            GUILayout.BeginHorizontal();

            GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(40));
            GUILayout.Space(5);
            GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(35));

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawNodes()
        {
            if (_nodes != null)
            {
                for (int i = 0; i < _nodes.Count; i++)
                {
                    _nodes[i].Draw();
                }
            }
        }

        private void DrawConnections()
        {
            if (_nodes != null)
            {
                for (int i = 0; i < _connections.Count; i++)
                {
                    _connections[i].Draw();
                }
            }
        }

        private void DrawBind()
        {
            if(selectedInNode == null) return;

            Handles.DrawLine(
                selectedInNode.Rect.center,
                Event.current.mousePosition
            ) ;

            GUI.changed = true;
        }

        private void ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    ClearConnectionSelection();
                    if (e.button == 1)
                    {
                        ProcessContextMenu(e.mousePosition);
                    }
                    break;
            }
        }

        private void ProcessNodeEvents(Event e)
        {
            if (_nodes != null)
            {
                for (int i = _nodes.Count - 1; i >= 0; i--)
                {
                    bool guiChanged = _nodes[i].ProcessEvents(e);

                    if (guiChanged)
                    {
                        GUI.changed = true;
                    }
                }
            }
        }

        private void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
            genericMenu.ShowAsContext();
        }

        private void OnClickAddNode(Vector2 mousePosition)
        {
            Node node = new Node(mousePosition, 100, 100, nodeStyle, selectedNodeStyle);
            _nodes.Add(node);
            node.OnBindNode += OnBindNode;
            node.OnClick += OnClickNode;
            node.OnRemoveNode += OnRemoveNode;
        }

        private void OnClickRemoveConnection(Connection connection)
        {
            _connections.Remove(connection);
        }

        private void OnBindNode(Node node)
        {
            selectedInNode = node;
        }

        private void OnRemoveNode(Node node)
        {
            if (_connections != null)
            {
                List<Connection> connectionsToRemove = new List<Connection>();

                for (int i = 0; i < _connections.Count; i++)
                {
                    if (_connections[i].node1 == node || _connections[i].node2 == node)
                    {
                        connectionsToRemove.Add(_connections[i]);
                    }
                }

                for (int i = 0; i < connectionsToRemove.Count; i++)
                {
                    _connections.Remove(connectionsToRemove[i]);
                }

                connectionsToRemove = null;
            }

            _nodes.Remove(node);
        }

        private void OnClickNode(Node node)
        {
            if (selectedInNode != null && selectedInNode != node) ConnectNodes(node);
        }

        private void ConnectNodes(Node node)
        {
            foreach (var conn in _connections)
            {
                if ((conn.node1 == node && conn.node2 == selectedInNode) ||
                    (conn.node2 == node && conn.node1 == selectedInNode)) 
                    return;
            }

            Connection connection = new Connection(selectedInNode, node);
            _connections.Add(connection);
            connection.OnClickRemoveConnection += (Connection conn)=> { _connections.Remove(conn); };
            ClearConnectionSelection();
        }

        private void ClearConnectionSelection()
        {
            selectedInNode = null;
        }
    }
}