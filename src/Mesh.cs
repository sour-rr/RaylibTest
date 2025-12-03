using System.Numerics;

public class Mesh : Component
{
    public string Name { get; private set; }
    public Vector3[] Vertices { get; private set; }
    public Face[] Faces { get; private set; }
    public Vector3 WorldPosition { get; private set; }

    public Mesh(string name, Vector3 pos)
    {
        this.Name = name;
        this.WorldPosition = pos;
    }

    public virtual void Initialise(int verticeCount, int faceCount)
    {
        this.Vertices = new Vector3[verticeCount];
        for (int i = 0; i < verticeCount; i++)
            Vertices[i] = new(float.NaN, float.NaN, float.NaN);

        this.Faces = new Face[faceCount];
        for (int i = 0; i < faceCount; i++)
            Faces[i] = new(-1, -1, -1);
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

    public  Mesh CreateUnitCube(Vector3 position)
    {
        Mesh cube = new Mesh("Cube", position);
        Initialise(8, 6);
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

        return cube;
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