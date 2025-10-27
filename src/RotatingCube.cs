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
        float d = 2f; //distance from camera (origin)

        Mesh[] cubes = new Mesh[5];
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

        Camera cam = new Camera(new(2, 2, 2));
        while (!Raylib.WindowShouldClose())
        {
            //time
            int fpsCount = Raylib.GetFPS();
            Raylib.DrawText($"FPS: {fpsCount}", 800, 0, 40, Color.Black);

            double time = Raylib.GetTime();
            double dt = Raylib.GetFrameTime();
            cam.Update((float)dt);
            //float moveSpeed = 10;
            //float rotateSpeed = 5;
            double radians = time; //one radian of rotation per second

            //transformed points using rotations in model space (so objects own local origin) 
            //Model Space -> World Space
            Vector3[] transformedPoints = new Vector3[verticeCount];
            for (int i = 0; i < verticeCount; i++)
                transformedPoints[i] = cube.WorldProjection(cube.Vertices[i], AxisRotation.y, (float)radians, new(0,0,0));

            // model space into view space (how does that position and transformation look to the camera) 
            // World Space -> View Space
            Vector3[] viewPoints = new Vector3[verticeCount];
            for (int i = 0; i < verticeCount; i++)
                viewPoints[i] = cam.ViewProjection(transformedPoints[i]);

            //project onto the 2D screen based off of z-depth
            //View Space -> Screen Space
            Vector2[] projectedPoint = new Vector2[verticeCount];
            for (int i = 0; i < verticeCount; i++)
                projectedPoint[i] = cube.ProjectionMatrix(viewPoints[i], d, centre, sideLength);

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);
            Color color = Color.Red;

            //draw the points
            for (int i = 0; i < verticeCount; i++)
            {
                float x = cube.Vertices[i].X;
                float y = cube.Vertices[i].Y;
                Raylib.DrawPixel((int)x, (int)y, color);
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