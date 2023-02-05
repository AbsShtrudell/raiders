using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Raiders.LevelEditor
{
    public abstract class Panel
    {
        public Rect rect
        { get; protected set; }

        protected GUIStyle _style;
        protected float _resizerWidtht = 5f;

        public Panel(Rect rect, GUIStyle style)
        {
            this.rect = rect;
            this._style = style;
        }

        public abstract void Draw();

        public bool IsContains(Vector2 position)
        {
            return rect.Contains(position);
        }
    }
}