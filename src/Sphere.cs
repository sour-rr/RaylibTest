using Raylib_cs;
using System;
using System.Drawing;
using System.Numerics;

public class Sphere : Mesh
{
    public int StepX;
    public int StepY;
    public int Radius;
    public Sphere(string name, Vector3 pos, int stepCountX, int stepCountY, int radius) : base(name, pos)
    {
        this.StepX = stepCountX;
        this.StepY = stepCountY;
        this.Radius = radius;
        this.CreateSphere();
    }

    public void CreateSphere()
    {
        int verticeCount = (360 / StepX) * ((180 / StepY) + 1);
        int faceCount = (verticeCount - (360/StepX)) * 2;
        Console.WriteLine($"Vertice Count: {verticeCount}, Face Count: {faceCount}");
        Initialise(verticeCount, faceCount);
        
        for (int i = 0; i <= 180; i += StepY) //latitude 
            for (int j = 0; j < 360; j += StepX)
            {
                double phi = Math.PI * i / 180;
                double theta = Math.PI * j / 180;
                int x = (int)(Radius * Math.Sin(phi) * Math.Cos(theta));
                int y = (int)(Radius * Math.Cos(phi));
                int z = (int)(Radius * Math.Sin(phi) * Math.Sin(theta));
                AddVertex(new(x + this.WorldPosition.X, y + this.WorldPosition.Y, z + this.WorldPosition.Z));
                //if (i == 0 || i == 180) break;
            }

        AddFaces();
    }
    
    private void AddFaces()
    {
        //retrieve the vectors from the array of vertices to make up the faces of the triangle
        int xPoint = 360 / StepX;
        int yPoint = (180 / StepY);
        int count = 0;
        for (int i = 0; i < yPoint; i++)
            for (int j = 0; j < xPoint; j++)
            {
                count++;
                int point1 = (j % xPoint) + (xPoint * i);
                int point2 = ((j + 1) % xPoint) + (xPoint * i);
                int point3 = (j % xPoint) + (xPoint * (i + 1));
                int point4 = ((j + 1) % xPoint) + (xPoint * (i + 1));

                AddFace(point1, point2, point3);
                //AddFace(point3, point4, point1);

                // AddFace(point1, point3, point2);
                AddFace(point2, point3, point4);
                Console.WriteLine($"i:{i} j:{j} {point1} {point2} {point3} {point4}");
            }
        Console.WriteLine($"Face Count: {count}");
    }
}