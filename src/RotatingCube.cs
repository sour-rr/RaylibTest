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
        int sideLength = 75;
        float d = 5f; //distance from camera (origin)

        Vector3[] basePoints =
        {
            new(-1, -1, -1), new(1, -1, -1), new(1, 1, -1), new(-1, 1, -1),
            new(-1, -1,  1), new(1, -1,  1), new(1, 1,  1), new(-1, 1,  1)
        };

        int verticeCount = 8;

        bool rotateXAxis = false;
        bool rotateYAxis = true;
        bool rotateZAxis = false;
        while (!Raylib.WindowShouldClose())
        {
            //time
            double time = Raylib.GetTime();
            double radians = time; //one radian of rotation per second

            //transformed points using rotations 
            Vector3[] transformedPoints = new Vector3[verticeCount];
            for (int i = 0; i < verticeCount; i++)
            {
                var newPoint = new Vector3();

                if (rotateXAxis)
                {
                    //rotation about the x-axis 
                    Raylib.DrawText("Rotating about the X-Axis", 0, 0, 40, Color.Black);
                    newPoint = Matrix.Mulitply(MatrixRotation.X(time), basePoints[i]);
                }

                else if (rotateYAxis)
                {
                    // rotation about the y-axis
                    Raylib.DrawText("Rotating about the Y-Axis", 0, 0, 40, Color.Black);
                    newPoint = Matrix.Mulitply(MatrixRotation.Y(time), basePoints[i]);
                }

                else if (rotateZAxis)
                {
                    //rotaiton about the z-axis 
                    Raylib.DrawText("Rotating about the Z-Axis", 0, 0, 40, Color.Black);
                    newPoint = Matrix.Mulitply(MatrixRotation.Z(time), basePoints[i]);
                }

                newPoint = MatrixTranslation.X(newPoint, 0);
                newPoint = MatrixTranslation.Y(newPoint, 0);
                newPoint = MatrixTranslation.Z(newPoint, 3);
                // rotating about the x-axis then the y-axis
                // Raylib.DrawText("Rotating about the X-Axis, then the Y-axis", 0, 0, 40, Color.Black);
                // var XY = Matrix.Multiply(MatrixRotation.X(time), MatrixRotation.Y(time)); //arguments m1, m2, multiplication  m2 then m1 
                // var newPoint = Matrix.Mulitply(XY, basePoints[i]);
                transformedPoints[i] = newPoint;
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
                    //Raylib.DrawLineEx(startPoint, endPoint, thickness, color);
                    DrawLine.DrawLineC(startPoint, endPoint, Color.Black);
                }
            }

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}   