using Raylib_cs;
using System;
using System.Numerics;

public class Sphere : Mesh
{
    public int StepX;
    public int StepY;
    public int Radius;
    public Sphere(string name, int verticeCount, int faceCount, int stepCountX, int stepCountY, int radius) : base(name, verticeCount, faceCount)
    {
        this.StepX = stepCountX;
        this.StepY = stepCountY;
        this.Radius = radius;
    }

    public void CreateSphere()
    {
        for(int i = 0; i <= 180; i += StepY) //latitude 
            for (int j = 0; j <= 360; j += StepX)
            {
                double phi = Math.PI * i / 180;
                double theta = Math.PI * j / 180;
                int x = (int)(Radius * Math.Sin(phi) * Math.Cos(theta));
                int y = (int)(Radius * Math.Cos(phi));
                int z = (int)(Radius * Math.Sin(phi) * Math.Sin(theta));
                AddVertex(new(x, y, z));
            }
    }
}