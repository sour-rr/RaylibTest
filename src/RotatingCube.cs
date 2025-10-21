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

        int verticeCount = 8;
        Mesh cube = new Mesh("Cube", verticeCount, 12);
        cube.AddVertex(new(1, 1, 1)); //0
        cube.AddVertex(new(1, 1, -1)); //1
        cube.AddVertex(new(-1, 1, -1)); //2
        cube.AddVertex(new(-1, 1, 1)); //3
        cube.AddVertex(new(1, -1, 1)); //4
        cube.AddVertex(new(1, -1, -1)); //5
        cube.AddVertex(new(-1, -1, -1)); //6
        cube.AddVertex(new(-1, -1, 1)); //7

        cube.AddFace(0, 1, 2); //top faces
        cube.AddFace(2, 3, 0);

        cube.AddFace(4, 5, 6); //bottom faces
        cube.AddFace(6, 7, 4);

        cube.AddFace(0, 1, 5); //side face 1 
        cube.AddFace(5, 4, 0);

        cube.AddFace(3, 7, 4); //side face 2
        cube.AddFace(4, 0, 3);

        cube.AddFace(1, 5, 6); //side face 3
        cube.AddFace(6, 2, 1);

        cube.AddFace(2, 6, 7); //side face 4
        cube.AddFace(7, 3, 2); 

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
                var vertex = cube.Vertices[i];
                var newPoint = new Vector3();

                if (rotateXAxis)
                {
                    //rotation about the x-axis 
                    Raylib.DrawText("Rotating about the X-Axis", 0, 0, 40, Color.Black);
                    newPoint = Matrix.Mulitply(MatrixRotation.X(radians), vertex);
                }

                else if (rotateYAxis)
                {
                    // rotation about the y-axis
                    Raylib.DrawText("Rotating about the Y-Axis", 0, 0, 40, Color.Black);
                    newPoint = Matrix.Mulitply(MatrixRotation.Y(radians), vertex);
                }

                else if (rotateZAxis)
                {
                    //rotaiton about the z-axis 
                    Raylib.DrawText("Rotating about the Z-Axis", 0, 0, 40, Color.Black);
                    newPoint = Matrix.Mulitply(MatrixRotation.Z(radians), vertex);
                }

                newPoint = MatrixTranslation.X(newPoint, 0);
                newPoint = MatrixTranslation.Y(newPoint, 0);
                newPoint = MatrixTranslation.Z(newPoint, 0);

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
            for(int i = 0, n = cube.Faces.Length; i < n; i++)
            {
                var vertexA = projectedPoint[cube.Faces[i].A];
                var vertexB = projectedPoint[cube.Faces[i].B];
                var vertexC = projectedPoint[cube.Faces[i].C];

                DrawLine.DrawLineC(vertexA, vertexB, Color.Black);
                DrawLine.DrawLineC(vertexB, vertexC, Color.Black);
                DrawLine.DrawLineC(vertexC, vertexA, Color.Black);
            }

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}   