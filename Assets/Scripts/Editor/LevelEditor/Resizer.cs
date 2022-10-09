using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelEditor
{
    public class Resizer : Panel
    {
        protected float _width = 3;

        public Resizer(Rect rect, GUIStyle style, Panel owner) : base(new Rect(), style)
        {
            _owner = owner;
        }

        public override void Draw()
        {
            Rect = new Rect(_owner.GetRect().position.x + _owner.GetRect().width, _owner.GetRect().position.y, _width, _owner.GetRect().height);
            GUILayout.BeginArea(Rect, _style);
            GUILayout.EndArea();

            EditorGUIUtility.AddCursorRect(Rect, MouseCursor.ResizeHorizontal);
        }

        public override void ProcessEvents(Event e)
        {
        }
    }
}