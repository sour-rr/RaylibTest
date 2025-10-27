using System;
using System.Drawing;
using System.Dynamic;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

public class Mesh
{
    public string Name { get; private set; }
    public Vector3[] Vertices { get; private set; }
    public Face[] Faces { get; private set; }

    public Mesh(string name, int verticeCount, int faceCount)
    {
        this.Name = name;

        this.Vertices = new Vector3[verticeCount];
        for (int i = 0; i < verticeCount; i++)
            Vertices[i] = new(float.NaN, float.NaN, float.NaN);

        this.Faces = new Face[faceCount];
        for (int i = 0; i < faceCount; i++)
            Faces[i] = new(-1,-1,-1);
    }

    public bool AddVertex(Vector3 point)
    {
        for (int i = 0, n = Vertices.Length; i < n; i++)
        {
            if (!float.IsNaN(Vertices[i].X)) continue;
            Vertices[i] = point;
            return true; //point has been added
        }
        return false; //no point has been added
    }

    public bool AddFace(int a, int b, int c)
    {
        for (int i = 0, n = Faces.Length; i < n; i++)
        {
            if (Faces[i].A != -1) continue; //skip the filled positions
            Faces[i] = new Face(a, b, c);
            return true;
        }
        return false;
    }

    //rotate the points by a specified axis, then translate them
    public Vector3 WorldProjection(Vector3 vertex, AxisRotation axis, float radian, Vector3 translation)
    {
        switch (axis)
        {
            case AxisRotation.x:
                vertex = Matrix.Mulitply(MatrixRotation.X(radian), vertex);
                break;
            case AxisRotation.y:
                vertex = Matrix.Mulitply(MatrixRotation.Y(radian), vertex);
                break;
            case AxisRotation.z:
                vertex = Matrix.Mulitply(MatrixRotation.Z(radian), vertex);
                break;
        }

        vertex = MatrixTranslation.Translate(vertex, translation.X, translation.Y, translation.Z);
        return vertex;
    }

    public Vector2 ProjectionMatrix(Vector3 vertex, float d, Vector2 centre, float sideLength)
    {
        float x = vertex.X;
        float y = vertex.Y;
        float z = vertex.Z;
        if (z <= -d) {
            return new(float.NaN, float.NaN);
        }
        float x_1 = (d / (z + d)) * x;
        float y_1 = (d / (z + d)) * y;
        vertex = new Vector3(centre.X + (x_1 * sideLength), centre.Y + (y_1 * sideLength), 0f);
        return new(vertex.X, vertex.Y);
    }
}

public struct Face
{
    public int A { get; private set; }
    public int B { get; private set; }
    public int C { get; private set; }
    public Face(int a, int b, int c)
    {
        this.A = a;
        this.B = b;
        this.C = c;
    }
}

public enum AxisRotation
{
    x,
    y,
    z
}