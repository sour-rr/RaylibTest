using Raylib_cs;
using System;
using System.Numerics;

class Program
{
    const int width = 1000;
    const int height = 800;
    static Vector2 centre = new(width / 2, height / 2);
    public static Camera cam = new Camera(new(0, 0, 0));
    //define 2D points 
    static int sideLength = 75; //75
    public static float d = 10f; //distance from camera (origin)

    static void Main()
    {
        Raylib.InitWindow(width, height, "Rotating Cube Window");
        Raylib.SetTargetFPS(60);

        //ecs
        List<Object> objects = new List<Object>();
        var prefab_EX = Prefab.CreateObject<Sphere>(new("Sphere", new(60,0,10), 60, 60, 10));
        objects.Add(prefab_EX); 
        
        //Update loop
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            //time
            int fpsCount = Raylib.GetFPS();
            Raylib.DrawText($"FPS: {fpsCount}", 800, 0, 40, Color.Black);

            double time = Raylib.GetTime(); //elapsed time since start
            double dt = Raylib.GetFrameTime(); //delta time
            cam.Update((float)dt);

            double radians = time; //one radian of rotation per second            

            if (Raylib.IsKeyPressed(KeyboardKey.C))
            {
                Console.WriteLine(cam.Position);
                var prefab = Prefab.CreateObject<Sphere>(new("Sphere", cam.Position, 60, 60, 10));
                objects.Add(prefab);    
            }
            foreach (var obj in objects)
            {
                obj.Update();
                obj.GetComponent<Transform>().RotateY((float)dt);
                if (Raylib.IsKeyPressed(KeyboardKey.R))
                {
                    obj.GetComponent<Transform>().RotateY(MathF.PI/4);
                }
                Render(obj.GetComponent<Mesh>(), radians);
            }

            //Draw crosshair
            DrawCrossHair();
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
            // if (projectedPoint[i].X > width || projectedPoint[i].X < 0)
            //     Console.WriteLine(projectedPoint[i]);
            // if(projectedPoint[i].Y > height || projectedPoint[i].Y < 0)
            //     Console.WriteLine(projectedPoint[i]);
            
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

    private static void DrawCrossHair()
    {
        //draw a white cross hair at the centre of the screen
        int size = 10;
        int thickness = 1;
        //draw the vertical section
        Raylib.DrawLineEx(new(centre.X - size,centre.Y), new(centre.X + size,centre.Y), thickness, Color.White);
        Raylib.DrawLineEx(new(centre.X,centre.Y - size), new(centre.X,centre.Y + size), thickness, Color.White);
    }
}               