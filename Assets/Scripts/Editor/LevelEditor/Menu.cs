using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace LevelEditor
{
    public class Menu : Panel
    {
        public Menu(Rect rect) : base(rect, null)
        {
        }

        public event Action OnSave;
        public event Action OnLoad;

        public override void Draw()
        {
            Rect = new Rect(0, 0, _owner.GetRect().width, 20);

            GUILayout.BeginArea(Rect, EditorStyles.toolbar);
            GUILayout.BeginHorizontal();

            if(GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(40)))
            {
                OnSave?.Invoke();
            }
            GUILayout.Space(5);
            if(GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(35)))
            {
                OnLoad?.Invoke();
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        public override void ProcessEvents(Event e)
        {
        }
    }
}
