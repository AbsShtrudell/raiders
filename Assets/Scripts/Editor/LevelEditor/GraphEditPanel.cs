using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelEditor
{
    public class GraphEditPanel : Panel
    { 
        private List<Node> _nodes = new List<Node>();
        private List<Connection> _connections = new List<Connection>();
        private Node _selectedBindNode;
        private Node _selectedNode;

        private GUIStyle nodeStyle;
        private GUIStyle selectedNodeStyle;

        public List<Node> Nodes => _nodes;
        public List<Connection> Connections => _connections;
        public Node SelectedNode => _selectedNode;

        public GraphEditPanel(Rect rect, GUIStyle style) : base(rect, style)
        {
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("Assets/Resources/Sprites/icons/house-vikings.png") as Texture2D;
            nodeStyle.border = new RectOffset(0, 0, 0, 0);
            selectedNodeStyle = new GUIStyle();
            selectedNodeStyle.normal.background = EditorGUIUtility.Load("Assets/Resources/Sprites/icons/house-rebels.png") as Texture2D;
            selectedNodeStyle.border = new RectOffset(0, 0, 0, 0);
        }

        public override void Draw()
        {
            Rect = new Rect(Rect.x, Rect.y, _owner.GetRect().width, _owner.GetRect().height);

            DrawBind();
            DrawConnections();
            DrawNodes();
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
            if (_selectedBindNode == null) return;

            Handles.DrawLine(
                _selectedBindNode.Rect.center,
                Event.current.mousePosition
            );

            GUI.changed = true;
        }

        private void ProcessNodeEvents(Event e)
        {
            if (_nodes != null)
            {
                for (int i = _nodes.Count - 1; i >= 0; i--)
                {
                    _nodes[i].ProcessEvents(e);
                }
            }
            GUI.changed = true;
        }

        private void OnClickAddNode(Vector2 mousePosition)
        {
            Node node = new Node(new Rect(mousePosition.x - 50, mousePosition.y - 50, 100, 100), nodeStyle, selectedNodeStyle);
            _nodes.Add(node);
            node.OnBindNode += OnBindNode;
            node.OnClick += OnClickNode;
            node.OnRemoveNode += OnRemoveNode;
            node.OnSelected += SelectNode;
            node.OnDeselected += DeselectNode;
        }

        private void OnBindNode(Node node)
        {
            _selectedBindNode = node;
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
            if (_selectedBindNode != null && _selectedBindNode != node) ConnectNodes(node);
        }

        private void SelectNode(Node node)
        {
            _selectedNode = node;
        }

        private void DeselectNode(Node node)
        {
            if (_selectedNode != node) return;

            _selectedNode = null;
        }

        private void ConnectNodes(Node node)
        {
            foreach (var conn in _connections)
            {
                if ((conn.node1 == node && conn.node2 == _selectedBindNode) ||
                    (conn.node2 == node && conn.node1 == _selectedBindNode))
                    return;
            }

            Connection connection = new Connection(_selectedBindNode, node);
            _connections.Add(connection);
            connection.OnClickRemoveConnection += (Connection conn) => { _connections.Remove(conn); };
            ClearConnectionSelection();
        }

        private void ClearConnectionSelection()
        {
            _selectedBindNode = null;
        }

        private void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
            genericMenu.ShowAsContext();
        }

        public override void ProcessEvents(Event e)
        {
            ProcessNodeEvents(e);

            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 1 && IsContains(e.mousePosition))
                    {
                        ProcessContextMenu(e.mousePosition);
                        e.Use();
                    }
                    break;
            }
        }
    }
}