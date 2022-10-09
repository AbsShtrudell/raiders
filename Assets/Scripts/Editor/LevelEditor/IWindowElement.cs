using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWindowElement
{
    public void SetOwner(IWindowElement owner);
    public void Draw();
    public bool IsContains(Vector2 position);
    public Rect GetRect();
    public List<IWindowElement> GetChildren();
    public void ProcessEvents(Event e);
    public void AddChild(IWindowElement child);
}
