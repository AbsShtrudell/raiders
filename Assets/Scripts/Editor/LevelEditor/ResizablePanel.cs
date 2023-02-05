using UnityEngine;

namespace Raiders.LevelEditor
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
            GUILayout.BeginArea(rect, _style);
            GUILayout.BeginVertical();

            GUILayout.EndVertical();
            GUILayout.EndArea();

            resizer.Draw();
        }
    }
}