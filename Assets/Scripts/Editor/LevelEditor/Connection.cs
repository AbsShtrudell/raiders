using UnityEditor;
using UnityEngine;
using System;

namespace Raiders.LevelEditor
{
    public class Connection 
    {
        public Node node1
        { get; private set; }
        public Node node2
        { get; private set; }

        public event Action<Connection> OnClickRemoveConnection;

        public Connection(Node node1, Node node2)
        {
            this.node1 = node1;
            this.node2 = node2;
        }

        public void Draw()
        {
            Handles.DrawLine(
                node1.Rect.center,
                node2.Rect.center
            );

            if (Handles.Button((node1.Rect.center + node2.Rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
            {
                OnClickRemoveConnection?.Invoke(this);
            }
        }
    }
}