using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace LevelEditor
{
    public class Node
    {
        private Rect _rect;
        private bool _isDragged;
#pragma warning disable 0414
        private bool _isSelected;
#pragma warning restore 0414
        private GUIStyle style;
        private GUIStyle defaultNodeStyle;
        private GUIStyle selectedNodeStyle;

        public Rect Rect
        { get { return _rect; } }

        public event Action<Node> OnRemoveNode;
        public event Action<Node> OnBindNode;
        public event Action<Node> OnClick;

        public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle)
        {
            _rect = new Rect(position.x, position.y, width, height);
            style = nodeStyle;
            defaultNodeStyle = nodeStyle;
            selectedNodeStyle = selectedStyle;
        }

        public void Drag(Vector2 delta)
        {
            _rect.position += delta;
        }

        public void Draw()
        {
            GUI.Box(_rect, "", style);
        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (_rect.Contains(e.mousePosition))
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
                    if (e.button == 1 && _rect.Contains(e.mousePosition))
                    {
                        Select();
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
                        return true;
                    }
                    break;
            }

            return false;
        }

        private void Select()
        {
            GUI.changed = true;
            _isSelected = true;
            style = selectedNodeStyle;
        }

        private void Deselect()
        {
            _isSelected = false;
            GUI.changed = true;
            style = defaultNodeStyle;
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