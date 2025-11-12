using Raylib_cs;
using System;
using System.Drawing;
using System.Numerics;

public class Sphere : Mesh
{
    public int StepX;
    public int StepY;
    public int Radius;
    public Sphere(string name, int verticeCount, int faceCount, Vector3 pos, int stepCountX, int stepCountY, int radius) : base(name, verticeCount, faceCount, pos)
    {
        this.StepX = stepCountX;
        this.StepY = stepCountY;
        this.Radius = radius;
    }

    public void CreateSphere()
    {
        for (int i = 0; i <= 180; i += StepY) //latitude 
            for (int j = 0; j <= 360; j += StepX)
            {
                double phi = Math.PI * i / 180;
                double theta = Math.PI * j / 180;
                int x = (int)(Radius * Math.Sin(phi) * Math.Cos(theta));
                int y = (int)(Radius * Math.Cos(phi));
                int z = (int)(Radius * Math.Sin(phi) * Math.Sin(theta));
                AddVertex(new(x, y, z));
            }

        AddFaces();
    }
    
    private void AddFaces()
    {
        //retrieve the vectors from the array of vertices to make up the faces of the triangle
        int xPoint = StepX / 360;
        int yPoint = (StepY / 180) + 1;
        for(int i = 0; i < yPoint; i++)
            for(int j = 0; j < xPoint; j++)
            {
                int point1 = (j % xPoint) + (xPoint * i);
                int point2 = ((j + 1) % xPoint) + (xPoint * i);
                int point3 = (j % xPoint) + (xPoint * (i + 1));
                int point4 = ((j + 1) % xPoint) + (xPoint * (i + 1));

                AddFace(point1, point2, point3);
                AddFace(point3, point4, point2);
                Console.WriteLine($"{point1} {point2} {point3} {point4}");
            }
    }
}