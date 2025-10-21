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

        Vector3[] basePoints =
        {
            new(1, 1, 1), new(1, 1, -1), new(-1, 1, -1), new(-1, 1, 1), //top
            new(1, -1,  1), new(1, -1,  -1), new(-1, -1,  -1), new(-1, -1, 1) //bottom
        };

        int[,] edges =
        {
            {0,1}, {1,2}, {2,3}, {3,0}, //top edges
            {4,5}, {5,6}, {6,7}, {7,4}, //bottom edges
            {0,4}, {1,5}, {2,6}, {3,7} //side eges
        };

        int verticeCount = 8;
        int edgeCount = 12;

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
                newPoint = MatrixTranslation.Z(newPoint, 0);
                // rotating about the x-axis then the y-axis
                // Raylib.DrawText("Rotating about the X-Axis, then the Y-axis", 0, 0, 40, Color.Black);
                // var XY = Matrix.Multiply(MatrixRotation.X(time), MatrixRotation.Y(time)); //arguments m1, m2, multiplication  m2 then m1 
                // var newPoint = Matrix.Mulitply(XY, basePoints[i]);
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
                //rotate around y-axis
                viewPoints[i] = Matrix.Mulitply(MatrixRotation.Y(-cam.Yaw), viewPoints[i]);
                //rotate around the x-axis
                viewPoints[i] = Matrix.Mulitply(MatrixRotation.X(-cam.Pitch), viewPoints[i]);

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
            int thickness = 3;

            //draw the points
            for (int i = 0; i < verticeCount; i++)
            {
                float x = projectedPoint[i].X;
                float y = projectedPoint[i].Y;
                Raylib.DrawCircle((int)x, (int)y, thickness, color);
            }

            //draw the lines connecting them together
            for (int i = 0; i < edgeCount; i++)
            {
                Vector2 startPoint = projectedPoint[edges[i, 0]]; // edges[row, element]
                Vector2 endPoint = projectedPoint[edges[i, 1]];
                //check for invalid point 
                if (float.IsNaN(startPoint.X) || float.IsNaN(endPoint.X)) continue;
                DrawLine.DrawLineC(startPoint, endPoint, Color.Black);
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