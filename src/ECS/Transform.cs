using System.Numerics;

public class Transform : Component
{
    //Local variables
    private Vector3 rotation;
    private Vector3 scale = Vector3.One; 

    //Public Getter Variables
    public Vector3 Rotation => rotation;
    public Vector3 Scale => scale;
    public Vector3 Position {get; private set;} = Vector3.Zero;
    public AxisRotation AxisRotation {get; private set;}
    
    public void Translate(Vector3 vector) => this.Position += vector;

    public void RotateXAxis() => AxisRotation = AxisRotation.x;
    public void RotateYAxis() => AxisRotation = AxisRotation.y;
    public void RotateZAxis() => AxisRotation = AxisRotation.z;
    
    public void Rotate(float radians)
    {
        switch (AxisRotation)
        {
            case AxisRotation.x:
                rotation.X += radians % (2*MathF.PI);
                break;
            case AxisRotation.y:
                rotation.Y += radians % (2*MathF.PI);
                break;
            case AxisRotation.z:
                rotation.Z += radians % (2*MathF.PI);
                break;
        }
    }
    
    public void ScaleX(float scaleX) => this.scale.X = scaleX;
    public void RotateX(float radians)
    {
        rotation.X = radians % (2*MathF.PI);
    }
    public void RotateY(float radians)
    {
        rotation.Y = radians % (2*MathF.PI);
        UpdateMesh();        
    }

    public void RotateZ(float radians)
    {
        rotation.Z = radians % (2*MathF.PI);
    }

    private void UpdateMesh()
    {
        //get the points on the mesh
        Mesh mesh = this.Object.GetComponent<Mesh>();
        for (int i = 0, n = mesh.Vertices.Length; i < n; i++) // iterate over each point of the mesh
        {
            Vector3 vertex = mesh.Vertices[i];
            //scale the object accordingly to the scale vector 
            vertex = ApplyScale(vertex);
            //Rotate the object accordingly to the rotation vector 
            vertex = ApplyRotation(vertex);
            //Translate the object accordingly to the position vector
            vertex = ApplyTranslation(vertex);
            mesh.Vertices[i] = vertex;
        }
    }   

    private Vector3 ApplyScale(Vector3 vertex) => new Vector3(vertex.X * this.Scale.X, vertex.Y * this.Scale.Y, vertex.Z * this.Scale.Z);
    private Vector3 ApplyRotation(Vector3 vertex)
    {
        vertex = Matrix.Mulitply(MatrixRotation.X(this.Rotation.X), vertex);
        vertex = Matrix.Mulitply(MatrixRotation.Y(this.Rotation.Y), vertex);
        vertex = Matrix.Mulitply(MatrixRotation.Z(this.Rotation.Z), vertex);  
        return vertex;    
    }
    private Vector3 ApplyTranslation(Vector3 vertex) => new Vector3(vertex.X + this.Position.X, vertex.Y + this.Position.Y, vertex.Z + this.Position.Z);

}

public enum AxisRotation
{
    x,
    y,
    z
}