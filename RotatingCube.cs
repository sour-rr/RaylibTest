using Raylib_cs;
using System;
using System.Numerics;

class RotatingCube
{
    static void Main()
    {
        const int width = 1000;
        const int height = 800;
        Vector2 centre = new(width / 2, height / 2);

        Raylib.InitWindow(width, height, "Raylib Window YAY");
        Raylib.SetTargetFPS(60);

        //define 2D points 
        int sideLength = 200;
        int half = sideLength / 2;
        float d = 10f; //distance from camera (origin)

        Mesh cube = new Mesh("Cube", 8);

        Mesh pyramid = new Mesh("Pyramid", 5);
        pyramid.points[0] = new(1, -1, 1);
        pyramid.points[1] = new(1, -1, -1);
        pyramid.points[2] = new(-1, -1, 1);
        pyramid.points[3] = new(-1, -1, -1);
        pyramid.points[4] = new(0, 1, 0); //top-center 

        Vector3[] basePoints =
        {
            new(-1, -1, -1), new(1, -1, -1), new(1, 1, -1), new(-1, 1, -1),
            new(-1, -1,  1), new(1, -1,  1), new(1, 1,  1), new(-1, 1,  1)
        };

        //translate points by d away from the camera
        // for (int i = 0; i < 8; i++)
        // {
        //     basePoints[i].Z += d;
        // }

        while (!Raylib.WindowShouldClose())
        {
            //time
            double time = Raylib.GetTime();
            double radians = time; //one radian of rotation per second
            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            //rotation on the y-axis
            //transformed points using rotations 
            Vector3[] transformedPoints = new Vector3[8];
            for (int i = 0; i < 8; i++)
            {
                float x = basePoints[i].X;
                float y = basePoints[i].Y;
                float z = basePoints[i].Z;

                // float x_1 = (float)(cos * x + sin * z);
                // float z_1 = (float)(-sin * x + cos * z);
                float x_1 = (float)(x * cos + y * sin * sin + z * sin * cos);
                float y_1 = (float)(y * cos + z * -sin);
                float z_1 = (float)(x * -sin + cos * sin * y + cos * cos * z);
                transformedPoints[i] = new(x_1, y_1, z_1);
            }

            //project onto the 2D screen based off of z-depth
            Vector2[] projectedPoint = new Vector2[8];
            for (int i = 0; i < 8; i++)
            {
                float x = transformedPoints[i].X;
                float y = transformedPoints[i].Y;
                float z = transformedPoints[i].Z;

                float x_1 = (d / (z + d)) * x;
                float y_1 = (d / (z + d)) * y;
                // Console.WriteLine("X: " + centre.X + (x_1 * sideLength));
                // Console.WriteLine("Y: " + centre.Y + (y_1 * sideLength));
                projectedPoint[i] = new(centre.X + (x_1 * sideLength), centre.Y + (y_1 * sideLength));
            }

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            //draw the points
            for (int i = 0; i < 8; i++)
            {
                float x = projectedPoint[i].X;
                float y = projectedPoint[i].Y;
                Raylib.DrawCircle((int)x, (int)y, 3, Color.Blue);
            }

            //draw the lines connecting them together
            for (int i = 0; i < 8; i++)
            {
                var a = projectedPoint[i];
                var b = projectedPoint[i];
                //var b = projectedPoint[(i + 1) % 8];
                for (int j = 0; j < 8; j++)
                {
                    if (i == j) continue;
                    b = projectedPoint[j];
                    Vector2 startPoint = new(a.X, a.Y);
                    Vector2 endPoint = new(b.X, b.Y);
                    Raylib.DrawLineEx(startPoint, endPoint, 3, Color.Blue);
                }
            }

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}   

public class Mesh
{
    //constructor 
    public string Name { get; private set; }
    public Vector3[] points { get; set; }
    public Mesh(string name, int verticeCount)
    {
        this.Name = name;
        points = new Vector3[verticeCount];
    }
}