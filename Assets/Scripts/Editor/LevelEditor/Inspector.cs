using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelEditor
{
    public class Inspector : IWindowElement
    {
        private IWindowElement _owner;
        private NodeData _data;

        public NodeData Data
        { get { return _data; }  set { _data = value; GUI.changed = true; } }
        public void AddChild(IWindowElement child)
        {
        }

        public void Draw()
        {
            if (_data != null)
            {
                EditorGUILayout.Space();
                _data.type = (BuildingType)EditorGUILayout.EnumPopup("Type", _data.type);
                _data.side = (Side)EditorGUILayout.EnumPopup("Side", _data.side);
            }
        }

        public List<IWindowElement> GetChildren()
        {
            return null;
        }

        public Rect GetRect()
        {
            return _owner == null ? _owner.GetRect() : new Rect();
        }

        public bool IsContains(Vector2 position)
        {
            return _owner == null? _owner.IsContains(position) : false;
        }

        public void ProcessEvents(Event e)
        {
        }

        public void SetOwner(IWindowElement owner)
        {
            _owner = owner;
            _owner?.AddChild(this);
        }
    }
}