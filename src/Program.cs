using Raylib_cs;
using System;
using System.Numerics;

class Program
{
    const int width = 1000;
    const int height = 800;
    static Vector2 centre = new(width / 2, height / 2);
    public static Camera cam = new Camera(new(2, 2, 2));
    //define 2D points 
    static int sideLength = 75; //75
    static float d = 2f; //distance from camera (origin)

    static void Main()
    {
        Raylib.InitWindow(width, height, "Rotating Cube Window");
        Raylib.SetTargetFPS(60);
        List<Mesh> meshes = new List<Mesh>();

        Sphere sphere = new Sphere("Sphere", new(0,0,0), 10, 10, 30);
        sphere.CreateSphere();

        meshes.Add(sphere);
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            //time
            int fpsCount = Raylib.GetFPS();
            Raylib.DrawText($"FPS: {fpsCount}", 800, 0, 40, Color.Black);

            double time = Raylib.GetTime();
            double dt = Raylib.GetFrameTime();
            cam.Update((float)dt);
            
            if(Raylib.IsKeyPressed(KeyboardKey.Z))
                sphere.RotateZAxis();
            else if(Raylib.IsKeyPressed(KeyboardKey.Y))
                sphere.RotateYAxis();
            else if(Raylib.IsKeyPressed(KeyboardKey.X))
                sphere.RotateXAxis();

            double radians = time; //one radian of rotation per second

            foreach (var mesh in meshes)
            {
                Render(mesh, radians, mesh.WorldPosition);
            }

            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }
    
    private static void Render(Mesh mesh, double radians, Vector3 pos)
    {
        //transformed points using rotations in model space (so objects own local origin) 
        //Model Space -> World Space
        int verticeCount = mesh.Vertices.Length;
        Vector3[] transformedPoints = new Vector3[verticeCount];
        for (int i = 0; i < verticeCount; i++)
            transformedPoints[i] = cam.WorldProjection(mesh, i, (float)radians);

        // model space into view space (how does that position and transformation look to the camera) 
        // World Space -> View Space
        Vector3[] viewPoints = new Vector3[verticeCount];
        for (int i = 0; i < verticeCount; i++)
            viewPoints[i] = cam.ViewProjection(transformedPoints[i]);

        //project onto the 2D screen based off of z-depth
        //View Space -> Screen Space
        Vector2[] projectedPoint = new Vector2[verticeCount];
        for (int i = 0; i < verticeCount; i++)
            projectedPoint[i] = cam.ProjectionMatrix(viewPoints[i], d, centre, sideLength);


        //draw the points
        for (int i = 0; i < verticeCount; i++)
        {
            float x = projectedPoint[i].X;
            float y = projectedPoint[i].Y;
            Raylib.DrawPixel((int)x, (int)y, Color.Red);
        }
            
        //draw the lines connecting them together
        for(int i = 0, n = mesh.Faces.Length; i < n; i++)
        {
            //Console.WriteLine($"Faces: {mesh.Faces[i].A} {mesh.Faces[i].B} {mesh.Faces[i].C}");
            var vertexA = projectedPoint[mesh.Faces[i].A];
            var vertexB = projectedPoint[mesh.Faces[i].B];
            var vertexC = projectedPoint[mesh.Faces[i].C];

            DrawLine.DrawLineC(vertexA, vertexB, Color.Black);
            DrawLine.DrawLineC(vertexB, vertexC, Color.Black);
            DrawLine.DrawLineC(vertexC, vertexA, Color.Black);
        }
    }
}               