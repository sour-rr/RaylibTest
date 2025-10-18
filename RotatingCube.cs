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

        Raylib.InitWindow(width, height, "Rotating Cube Window");
        Raylib.SetTargetFPS(60);

        //define 2D points 
        int sideLength = 200;
        float d = 10f; //distance from camera (origin)

        Vector3[] basePoints =
        {
            new(-1, -1, -1), new(1, -1, -1), new(1, 1, -1), new(-1, 1, -1),
            new(-1, -1,  1), new(1, -1,  1), new(1, 1,  1), new(-1, 1,  1)
        };

        int verticeCount = 8;

        while (!Raylib.WindowShouldClose())
        {
            //time
            double time = Raylib.GetTime();
            double radians = time; //one radian of rotation per second
            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            //rotation on the y-axis
            //transformed points using rotations 
            Vector3[] transformedPoints = new Vector3[verticeCount];
            for (int i = 0; i < verticeCount; i++)
            {
                float x = basePoints[i].X;
                float y = basePoints[i].Y;
                float z = basePoints[i].Z;

                //rotating about the y-axis
                // float x_1 = (float)(cos * x + sin * z);
                // float z_1 = (float)(-sin * x + cos * z);

                //rotating about the x-axis then the y-axis
                float x_1 = (float)(x * cos + y * sin * sin + z * sin * cos);
                float y_1 = (float)(y * cos + z * -sin);
                float z_1 = (float)(x * -sin + cos * sin * y + cos * cos * z);
                transformedPoints[i] = new(x_1, y_1, z_1);
            }

            //project onto the 2D screen based off of z-depth
            Vector2[] projectedPoint = new Vector2[verticeCount];
            for (int i = 0; i < verticeCount; i++)
            {
                float x = transformedPoints[i].X;
                float y = transformedPoints[i].Y;
                float z = transformedPoints[i].Z;

                float x_1 = (d / (z + d)) * x;
                float y_1 = (d / (z + d)) * y;
                projectedPoint[i] = new(centre.X + (x_1 * sideLength), centre.Y + (y_1 * sideLength));
            }

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);
            Color color = Color.Red;
            int thickness = 3;

            //draw the points
            for (int i = 0; i < verticeCount; i++)
            {
                float x = projectedPoint[i].X;
                float y = projectedPoint[i].Y;
                Raylib.DrawCircle((int)x, (int)y, thickness, color);
            }

            //draw the lines connecting them together
            for (int i = 0; i < verticeCount; i++)
            {
                var a = projectedPoint[i];
                for (int j = 0; j < verticeCount; j++)
                {
                    if (i == j) continue;
                    var b = projectedPoint[j];
                    Vector2 startPoint = new(a.X, a.Y);
                    Vector2 endPoint = new(b.X, b.Y);
                    Raylib.DrawLineEx(startPoint, endPoint, thickness, color);
                }
            }

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}   