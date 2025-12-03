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

        //ecs
        Object new_sphere = new Object();
        Colour color = new Colour();
        Sphere mesh1 = new Sphere("Sphere", new(0,0,0), 60, 60, 30);
        mesh1.CreateSphere();
        color.UpdateColour(Color.Blue);

        new_sphere.AddComponent<Colour>(color);
        new_sphere.AddComponent<Transform>(new Transform());
        new_sphere.AddComponent<Mesh>(mesh1);

        List<Object> objects = new List<Object>();
        objects.Add(new_sphere);

        //Update loop
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

            //change colour test
            if (Raylib.IsKeyPressed(KeyboardKey.I))
                color.UpdateColour(Color.Red);
            else if (Raylib.IsKeyPressed(KeyboardKey.O))
                color.UpdateColour(Color.DarkPurple);

            double radians = time; //one radian of rotation per second            

            foreach (var obj in objects)
            {
                obj.Update();
                if (Raylib.IsKeyPressed(KeyboardKey.R))
                {
                    obj.GetComponent<Transform>().RotateY(MathF.PI/2);
                }
                Render(obj.GetComponent<Mesh>(), radians);
            }

            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }
    
    private static void Render(Mesh mesh, double radians)
    {
        int verticeCount = mesh.Vertices.Length;
 
        Vector3[] viewPoints = new Vector3[verticeCount];
        for (int i = 0; i < verticeCount; i++)
            viewPoints[i] = cam.ViewProjection(mesh.Vertices[i]);

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
            var vertexA = projectedPoint[mesh.Faces[i].A];
            var vertexB = projectedPoint[mesh.Faces[i].B];
            var vertexC = projectedPoint[mesh.Faces[i].C];

            DrawLine.DrawLineC(vertexA, vertexB, mesh.Object.GetComponent<Colour>().Color);
            DrawLine.DrawLineC(vertexB, vertexC, mesh.Object.GetComponent<Colour>().Color);
            DrawLine.DrawLineC(vertexC, vertexA, mesh.Object.GetComponent<Colour>().Color);
        }
    }
}               