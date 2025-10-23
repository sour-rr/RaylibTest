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

        Camera cam = new Camera(new(2, 2, 2));
        while (!Raylib.WindowShouldClose())
        {
            //time
            int fpsCount = Raylib.GetFPS();
            Raylib.DrawText($"FPS: {fpsCount}", 800, 0, 40, Color.Black);

            double time = Raylib.GetTime();
            double dt = Raylib.GetFrameTime();
            float moveSpeed = 10;
            float rotateSpeed = 5;
            double radians = time; //one radian of rotation per second

            //Input - Movement 
            if (Raylib.IsKeyDown(KeyboardKey.W)) cam.Move(new(0, 0, (float)(moveSpeed * dt))); //move forwards (increase z)
            if (Raylib.IsKeyDown(KeyboardKey.S)) cam.Move(new(0, 0, (float)(-moveSpeed * dt))); //move backwards (decrease z)
            if (Raylib.IsKeyDown(KeyboardKey.D)) cam.Move(new((float)(moveSpeed * dt), 0, 0)); //move right
            if (Raylib.IsKeyDown(KeyboardKey.A)) cam.Move(new((float)(-moveSpeed * dt), 0, 0)); //move left
            if (Raylib.IsKeyDown(KeyboardKey.Space)) cam.Move(new(0, (float)(-moveSpeed * dt), 0)); //move cam up, so move world down
            if (Raylib.IsKeyDown(KeyboardKey.LeftShift)) cam.Move(new(0, (float)(moveSpeed * dt), 0)); //move cam down, so move world up

            //Input - Rotation 
            if (Raylib.IsKeyDown(KeyboardKey.Right)) cam.RotateX((float)(rotateSpeed * dt));
            if (Raylib.IsKeyDown(KeyboardKey.Left)) cam.RotateX((float)(-rotateSpeed * dt));
            if (Raylib.IsKeyDown(KeyboardKey.Up)) cam.RotateY((float)(rotateSpeed * dt));
            if (Raylib.IsKeyDown(KeyboardKey.Down)) cam.RotateY((float)(-rotateSpeed * dt));

            //transformed points using rotations in model space (so objects own local origin) 
            //Model Space -> World Space
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

            //model space into view space (how does that position and transformation look to the camera) 
            //World Space -> View Space
            Vector3[] viewPoints = new Vector3[verticeCount];
            for (int i = 0; i < verticeCount; i++)
            {
                float x = transformedPoints[i].X;
                float y = transformedPoints[i].Y;
                float z = transformedPoints[i].Z;
                //translation of world space points making the camera its origin now and moving relative to the camera
                viewPoints[i] = new(x -= cam.Position.X, y -= cam.Position.Y, z -= cam.Position.Z);

                //rotate relative to the camera
                //rotate around the x-axis
                viewPoints[i] = Matrix.Mulitply(MatrixRotation.X(-cam.Yaw), viewPoints[i]);
                //rotate around y-axis
                viewPoints[i] = Matrix.Mulitply(MatrixRotation.Y(-cam.Pitch), viewPoints[i]);
            }

            //project onto the 2D screen based off of z-depth
            //View Space -> Screen Space
            Vector2[] projectedPoint = new Vector2[verticeCount];
            for (int i = 0; i < verticeCount; i++)
            {
                float x = viewPoints[i].X;
                float y = viewPoints[i].Y;
                float z = viewPoints[i].Z;

                if (z <= -d) {
                    projectedPoint[i] = new(float.NaN, float.NaN);
                    continue;
                }
                float x_1 = (d / (z + d)) * x;
                float y_1 = (d / (z + d)) * y;
                projectedPoint[i] = new(centre.X + (x_1 * sideLength), centre.Y + (y_1 * sideLength));
            }

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);
            Color color = Color.Red;

            //draw the points
            for (int i = 0; i < verticeCount; i++)
            {
                float x = projectedPoint[i].X;
                float y = projectedPoint[i].Y;
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

public class Camera
{
    private Vector3 position;
    private float pitch = 0; //x-axis of rotation
    private float yaw = 0; //y-axis of rotation
    public Vector3 Position => position;
    public float Pitch => pitch;
    public float Yaw => yaw;

    public Camera(Vector3 pos)
    {
        this.position = pos;
    }

    public void Move(Vector3 translate) => this.position += translate;
    public void RotateX(float units) => this.pitch += units;
    public void RotateY(float units) => this.yaw += units;
}