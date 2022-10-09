using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.Collections.Generic;

namespace LevelEditor
{
    public class LevelEditorWindow : EditorWindow, IWindowElement
    {
        private Menu _menu;
        private ResizablePanel _inspectorPanel;
        private Inspector _inspector;
        private GraphEditPanel _graphEditPanel;


        [MenuItem("Window/Level Editor")]
        private static void OpenWindow()
        {
            LevelEditorWindow window = GetWindow<LevelEditorWindow>();
            window.titleContent = new GUIContent("Level Editor");
        }

        private void OnEnable()
        {
            var resizerStyle = new GUIStyle();
            resizerStyle.normal.background = EditorGUIUtility.Load("icons/d_AvatarBlendBackground.png") as Texture2D;

            var boxStyle = new GUIStyle();
            boxStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/cn entrybackodd.png") as Texture2D;

            _menu = new Menu(new Rect(0, 0, position.width, 20));
            _menu.SetOwner(this);
            _inspectorPanel = new ResizablePanel(new Rect(0, _menu.GetRect().height, 300, position.height), boxStyle, resizerStyle);
            _inspectorPanel.SetOwner(this);
            _graphEditPanel = new GraphEditPanel(new Rect(_inspectorPanel.GetRect().width, _menu.GetRect().height, position.width, position.height), boxStyle);
            _graphEditPanel.SetOwner(this);
            _inspector = new Inspector();
            _inspector.SetOwner(_inspectorPanel);
            _menu.OnSave += () => { Save.SaveData(_graphEditPanel.Nodes, _graphEditPanel.Connections, "Assets/Resources/Levels/Test.lvl"); };
        }

        private void OnGUI()
        {
            Draw();

            ProcessEvents(Event.current);

            if (GUI.changed) Repaint();
        }

        public void ProcessEvents(Event e)
        {
            _graphEditPanel.ProcessEvents(e);
            _inspectorPanel.ProcessEvents(e);
            _menu.ProcessEvents(e);
        }

        public void SetOwner(IWindowElement owner)
        {
        }

        public void Draw()
        {
            if (_graphEditPanel.SelectedNode == null) _inspector.Data = null;
            else _inspector.Data = _graphEditPanel.SelectedNode.data;

            _graphEditPanel.Draw();
            _inspectorPanel.Draw();
            _menu.Draw();
        }

        public bool IsContains(Vector2 position)
        {
            return this.position.Contains(position);
        }

        public Rect GetRect()
        {
            return position;
        }

        public List<IWindowElement> GetChildren()
        {
            return new List<IWindowElement>{ _menu, _inspectorPanel, _graphEditPanel };
        }

        public void AddChild(IWindowElement child)
        {
        }
    }
}