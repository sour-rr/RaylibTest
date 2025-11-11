using Raylib_cs;
using System.Numerics;
using System;

public class Camera
{
    private Vector3 position;
    private Vector3 worldUp = new(0, 1, 0);
    private Vector3 forward = new(0, 0, -1);
    private Vector3 up;
    private Vector3 right = new(1, 0, 0);
    private float pitch = 0; //x-axis of rotation
    private float yaw = 0; //y-axis of rotation
    public Vector3 Position => position;
    public float Pitch => pitch;
    public float Yaw => yaw;
    private float movementSpeed = 10f;
    private float rotateSpeed = 5f;

    public Camera(Vector3 pos)
    {
        this.position = pos;
    }

    public void Move(Vector3 translate) => this.position += translate;
    public void RotateX(float units) { this.pitch += units; this.pitch = Math.Clamp(pitch, (float)-Math.PI * 89/180, (float)Math.PI * 89/180); Console.WriteLine($"Pitch: {this.pitch}"); }
    public void RotateY(float units) => this.yaw += units;

    public void Update(float time)
    {
        Vector2 mousePos = Raylib.GetMouseDelta();
        //update the forwards relative to yaw and pitch 
        float x = MathF.Cos(pitch) * MathF.Sin(yaw);
        float y = MathF.Sin(pitch);
        float z = MathF.Cos(pitch) * MathF.Cos(yaw);
        forward = new(x, y, z);

        //calculate right and camera-up relative to forward 
        right = Vector3.Normalize(Vector3.Cross(forward, worldUp));
        up = Vector3.Cross(right, forward);
        Vector3 posForward = Vector3.Normalize(Vector3.Cross(worldUp, right));

        //Input - Movement 
        if (Raylib.IsKeyDown(KeyboardKey.W)) position += posForward * movementSpeed * time; //move forwards (increase z)
        if (Raylib.IsKeyDown(KeyboardKey.S)) position += -posForward * movementSpeed * time; //move backwards (decrease z)
        if (Raylib.IsKeyDown(KeyboardKey.D)) position += -right * movementSpeed * time; //move right
        if (Raylib.IsKeyDown(KeyboardKey.A)) position += right * movementSpeed * time; //move left
        if (Raylib.IsKeyDown(KeyboardKey.Space)) position += -worldUp * movementSpeed * time; //move cam up, so move world down
        if (Raylib.IsKeyDown(KeyboardKey.LeftShift)) position += worldUp * movementSpeed * time; //move cam down, so move world up

        //Input - Rotation 
        if (Raylib.IsKeyDown(KeyboardKey.Right)) RotateY(rotateSpeed * time);
        if (Raylib.IsKeyDown(KeyboardKey.Left)) RotateY(-rotateSpeed * time);
        if (Raylib.IsKeyDown(KeyboardKey.Up)) RotateX(rotateSpeed * time);
        if (Raylib.IsKeyDown(KeyboardKey.Down)) RotateX(-rotateSpeed * time);
    }

    public Vector3 ViewProjection(Vector3 vertex)
    {
        float x = vertex.X;
        float y = vertex.Y;
        float z = vertex.Z;
        //translation of world space points making the camera its origin now and moving relative to the camera
        vertex = new(x -= Position.X, y -= Position.Y, z -= Position.Z);
        //rotate relative to the camera
        vertex = Matrix.Mulitply(Matrix.Multiply(MatrixRotation.X(-Pitch), MatrixRotation.Y(-Yaw)), vertex); //m2 then m1
        return vertex;
    }
}
