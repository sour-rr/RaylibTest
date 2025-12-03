public class Cube : Mesh
{
    public Cube(string name, int verticeCount, int faceCount) : base(name, verticeCount, faceCount)
    {
        CreateUnitCube();
    }

    public void CreateUnitCube()
    {
        //Initialise(8, 12);
        AddVertex(new(1, 1, 1)); //0
        AddVertex(new(1, 1, -1)); //1
        AddVertex(new(-1, 1, -1)); //2
        AddVertex(new(-1, 1, 1)); //3
        AddVertex(new(1, -1, 1)); //4
        AddVertex(new(1, -1, -1)); //5
        AddVertex(new(-1, -1, -1)); //6
        AddVertex(new(-1, -1, 1)); //7

        AddFace(0, 1, 2); //top faces
        AddFace(2, 3, 0);

        AddFace(4, 5, 6); //bottom faces
        AddFace(6, 7, 4);

        AddFace(0, 1, 5); //side face 1 
        AddFace(5, 4, 0);

        AddFace(3, 7, 4); //side face 2
        AddFace(4, 0, 3);

        AddFace(1, 5, 6); //side face 3
        AddFace(6, 2, 1);

        AddFace(2, 6, 7); //side face 4
        AddFace(7, 3, 2);
    }
}