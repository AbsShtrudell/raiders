using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    public class ResizablePanel : Panel
    {
        protected Resizer resizer;

        public ResizablePanel(Rect rect, GUIStyle style, GUIStyle resStyle) : base(rect, style)
        {
            resizer = new Resizer(new Rect(), resStyle, this);
        }

        public override void Draw()
        {
            Rect = new Rect(Rect.x, Rect.y, Rect.width, _owner.GetRect().height);

            GUILayout.BeginArea(Rect, _style);
            GUILayout.BeginVertical();

            foreach(var child in _children)
            {
                child.Draw();
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();

            resizer.Draw();
        }

        public override void ProcessEvents(Event e)
        {
            foreach(var child in _children)
                child.ProcessEvents(e);
            resizer.ProcessEvents(e);
        }
    }
}