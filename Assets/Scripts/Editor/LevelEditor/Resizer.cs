using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Raiders.LevelEditor
{
    public class Resizer : Panel
    {
        protected Panel _owner;
        protected float _width = 3;


        public Resizer(Rect rect, GUIStyle style, Panel owner) : base(new Rect(), style)
        {
            _owner = owner;
        }

        public override void Draw()
        {
            rect = new Rect(_owner.rect.position.x + _owner.rect.width, _owner.rect.position.y, _width, _owner.rect.height);
            GUILayout.BeginArea(rect, _style);
            GUILayout.EndArea();

            EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeHorizontal);
        }
    }
}