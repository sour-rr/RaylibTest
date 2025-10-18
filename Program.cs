using Raylib_cs;
using System;
using System.Numerics;

class Program
{
    static void DrawSquare()
    {
        const int width = 1000;
        const int height = 800;
        Vector2 centre = new(width / 2, height / 2);

        Raylib.InitWindow(width, height, "Raylib Window YAY");
        Raylib.SetTargetFPS(60);

        //define 2D points 
        int sideLength = 200;
        int half = sideLength / 2;
        Vector2[] basePoints =
        {
            new(half, half),
            new(half, -half),
            new(-half, -half),
            new(-half, half)
        };

        while (!Raylib.WindowShouldClose())
        {
            double time = Raylib.GetTime();
            double radians = time; //one radian of rotation per second
            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            //transformed points using rotations 
            Vector2[] transformedPoints = new Vector2[4];
            for (int i = 0; i < 4; i++)
            {
                float x = basePoints[i].X;
                float y = basePoints[i].Y;
                float rx = (float)(cos * x - sin * y);
                float ry = (float)(sin * x + cos * y);
                transformedPoints[i] = new(centre.X + rx, centre.Y + ry);
            }

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            //draw the points
            for (int i = 0; i < 4; i++)
            {
                float x = transformedPoints[i].X;
                float y = transformedPoints[i].Y;
                Raylib.DrawCircle((int)x, (int)y, 10, Color.Red);
            }

            // //draw the lines connecting them together
            for(int i = 0; i < 4; i++)
            {
                var a = transformedPoints[i];
                var b = transformedPoints[(i + 1) % 4];
                Vector2 startPoint = new(a.X, a.Y);
                Vector2 endPoint = new(b.X, b.Y);
                Raylib.DrawLineEx(startPoint, endPoint, 10, Color.Red);
            }
            
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}

