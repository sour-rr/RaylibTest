using System;
using Raylib_cs;

public class Colour : Component
{
    public Color Color {get; private set;}  
    public bool UpdateColour(Color color)
    {
        this.Color = color;
        return true;
    }
}