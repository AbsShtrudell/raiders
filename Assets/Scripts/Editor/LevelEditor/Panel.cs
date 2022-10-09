using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelEditor
{
    public abstract class Panel : IWindowElement
    {
        protected Rect _rect;
        public Rect Rect
        { get { return _rect; } protected set { _rect = value; } }

        protected GUIStyle _style;
        protected IWindowElement _owner;
        protected List<IWindowElement> _children = new List<IWindowElement>();

        public Panel(Rect rect, GUIStyle style)
        {
            this.Rect = rect;
            this._style = style;
        }

        public abstract void Draw();

        public bool IsContains(Vector2 position)
        {
            return Rect.Contains(position);
        }

        public void SetOwner(IWindowElement owner)
        {
            _owner?.GetChildren().Remove(this);

            _owner = owner;
            _owner?.AddChild(this);
            
        }

        public Rect GetRect()
        {
            return Rect;
        }

        public List<IWindowElement> GetChildren()
        {
            return _children;
        }

        public abstract void ProcessEvents(Event e);

        public void AddChild(IWindowElement child)
        {
            if (child == null || _children.Contains(child)) return;

            _children.Add(child);

            child.SetOwner(this);
        }
    }
}