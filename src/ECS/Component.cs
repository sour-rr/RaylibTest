using System;
using Raylib_cs;

public class Component
{
    public Object Object {get; private set;}
    public void Attach(Object obj)
    {
        this.Object = obj;
        OnAttach();
    }

    public virtual void OnAttach() {}
    public virtual void Update() {}
}