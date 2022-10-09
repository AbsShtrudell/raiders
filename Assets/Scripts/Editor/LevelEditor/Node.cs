using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace LevelEditor
{
    public class Node : Panel
    {
        private bool _isDragged;
        private bool _isSelected;

        private GUIStyle style;
        private GUIStyle defaultNodeStyle;
        private GUIStyle selectedNodeStyle;

        public NodeData data;

        public event Action<Node> OnRemoveNode;
        public event Action<Node> OnBindNode;
        public event Action<Node> OnClick;
        public event Action<Node> OnSelected;
        public event Action<Node> OnDeselected;

        public Node(Rect rect, GUIStyle nodeStyle, GUIStyle selectedStyle) :base(rect, nodeStyle)
        {
            style = nodeStyle;
            defaultNodeStyle = nodeStyle;
            selectedNodeStyle = selectedStyle;
            data = new NodeData(rect.position);
        }

        public void Drag(Vector2 delta)
        {
            _rect.position += delta;
            data.position = Rect.position;
        }

        public override void Draw()
        {
            GUI.Box(Rect, "", style);
        }

        public override void ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (IsContains(e.mousePosition))
                        {
                            OnClick?.Invoke(this);
                            _isDragged = true;
                            Select();
                        }
                        else
                        {
                            Deselect();
                        }
                    }
                    if (e.button == 1 && IsContains(e.mousePosition))
                    {
                        ProcessContextMenu();
                        e.Use();
                    }
                    break;

                case EventType.MouseUp:
                    _isDragged = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && _isDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                    }
                    break;
            }
        }

        private void Select()
        {
            GUI.changed = true;
            _isSelected = true;
            style = selectedNodeStyle;
            OnSelected?.Invoke(this);
        }

        private void Deselect()
        {
            _isSelected = false;
            GUI.changed = true;
            style = defaultNodeStyle;
            OnDeselected?.Invoke(this);
        }

        private void ProcessContextMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Remove"), false, OnClickRemoveNode);
            genericMenu.AddItem(new GUIContent("Bind"), false, OnClickBindNode);
            genericMenu.ShowAsContext();
        }

        private void OnClickRemoveNode()
        {
            OnRemoveNode?.Invoke(this);
        }

        private void OnClickBindNode()
        {
            OnBindNode?.Invoke(this);
        }
    }
}