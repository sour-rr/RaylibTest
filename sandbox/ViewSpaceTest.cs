using Raylib_cs;
using System.Numerics;
using System;

public static class ViewSpaceTest
{
    public static void Square()
    {
        const int width = 1000;
        const int height = 800;
        Vector2 centre = new(width / 2, height / 2);

        Raylib.InitWindow(width, height, "2D View Space");
        Raylib.SetTargetFPS(60);

        //define 2D points 
        int sideLength = 75;
        float d = 5f; //distance from camera (origin)
        Camera2D cam = new Camera2D(new(2, 2)); //position is at (2,2)

        Vector2[] basePoints =
        {
            new (1,1),
            new (1,-1),
            new (-1,-1),
            new (-1,1)
        };

        int noOfVertices = 4;

        int[,] edges =
        {
            {0,1},
            {1,2},
            {2,3},
            {3,0}
        };

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);
            double deltaTime = Raylib.GetFrameTime();
            //WASD to move around - translation - modifiying camera position
            float moveSpeed = 2;
            if (Raylib.IsKeyDown(KeyboardKey.W)) cam.Move(0, -moveSpeed * (float)deltaTime);
            if (Raylib.IsKeyDown(KeyboardKey.S)) cam.Move(0, moveSpeed * (float)deltaTime);
            if (Raylib.IsKeyDown(KeyboardKey.D)) cam.Move(moveSpeed * (float)deltaTime, 0);
            if (Raylib.IsKeyDown(KeyboardKey.A)) cam.Move(-moveSpeed * (float)deltaTime, 0);

            //adjust for the viewpoint of the camera World Space -> View Space
            Vector2[] viewPoints = new Vector2[4];
            for (int i = 0; i < noOfVertices; i++)
            {
                float x = basePoints[i].X;
                float y = basePoints[i].Y;
                viewPoints[i] = new(x -= cam.Position.X, y -= cam.Position.Y);
            }

            //draw base points on the screen then scale them up accordingly  View Space -> Screen Space
            Vector2[] projectedPoints = new Vector2[noOfVertices];
            for (int i = 0; i < noOfVertices; i++)
            {
                float x = viewPoints[i].X;
                float y = viewPoints[i].Y;
                projectedPoints[i] = new(centre.X + (x * sideLength), centre.Y + (y * sideLength));
                Raylib.DrawCircle((int)projectedPoints[i].X, (int)projectedPoints[i].Y, 3, Color.Red);
            }

            //draw the edges
            for (int i = 0; i < noOfVertices; i++)
            {
                Vector2 startPoint = projectedPoints[edges[i, 0]];
                Vector2 endPoint = projectedPoints[edges[i, 1]];
                Raylib.DrawLineV(startPoint, endPoint, Color.Red);
            }

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}

public class Camera2D
{
    private Vector2 position;
    public Vector2 Position => position;
    public Camera2D(Vector2 pos)
    {
        this.position = pos;
    }

    public void Move(float x, float y)
    {
        position.X += x;
        position.Y += y;
    }
}