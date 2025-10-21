using System;
using System.Dynamic;
using System.Numerics;

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
}

public struct Face {
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