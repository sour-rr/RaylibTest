using System;
using System.Numerics;

public struct Matrix
{
    public float[,] M;
    public Matrix(float[,] m)
    {
        this.M = m;
    }

    public static Vector3 Mulitply(Matrix matrix, Vector3 vector) //multiply a vector by a matrix 
    {
        //compute matrix mulltipkicaion 
        Vector3 newVector = new Vector3();
        for (int i = 0; i < 3; i++) //row
            for (int j = 0; j < 3; j++) //each element in the row 
                //foreach element in the row mulitply by the corresponding vector element 
                newVector[i] += matrix.M[i, j] * vector[j];
        return newVector;
    }

    public static Matrix Multiply(Matrix m1, Matrix m2) //matrix multiplicaiton m2 then m1 
    {
        float[,] newMatrix = new float[3, 3];
        for (int i = 0; i < 3; i++) //row
            for (int j = 0; j < 3; j++) //each element in the row 
                newMatrix[i, j] = m1.M[i, 0] * m2.M[0, j] + m1.M[i, 1] * m2.M[1, j] + m1.M[i, 2] * m2.M[2, j];
        // [0,0] = [0,0] * [0,0] + [0,1] * [1,0] + [0,2] * [2,0]
        return new Matrix(newMatrix);
    }
}

public class MatrixRotation
{
    public static Matrix X(double rotation)
    {
        float cos = (float)Math.Cos(rotation);
        float sin = (float)Math.Sin(rotation);

        return new Matrix(new float[,]
        {
            {1, 0, 0},
            {0, cos, -sin },
            {0, sin, cos}
        });
    }

    public static Matrix Y(double rotation)
    {
        float cos = (float)Math.Cos(rotation);
        float sin = (float)Math.Sin(rotation);

        return new Matrix(new float[,]
        {
            {cos, 0, sin},
            {0, 1, 0},
            {-sin, 0 ,cos}
        });
    }

    public static Matrix Z(double rotation)
    {
        float cos = (float)Math.Cos(rotation);
        float sin = (float)Math.Sin(rotation);

        return new Matrix(new float[,]
        {
            {cos, -sin, 0},
            {sin, cos, 0},
            {0,0,1},
        });
    }
}

public class MatrixTranslation
{
    public static Vector3 Translate(Vector3 point, float unitX, float unitY, float unitZ) 
        { return new Vector3(point.X += unitX, point.Y += unitY, point.Z += unitZ);  }
    public static Vector3 X(Vector3 point, float units) { return new Vector3(point.X += units, point.Y, point.Z); }
    public static Vector3 Y(Vector3 point, float units) { return new Vector3(point.X, point.Y += units, point.Z); }
    public static Vector3 Z(Vector3 point, float units) { return new Vector3(point.X, point.Y, point.Z += units); }
}
