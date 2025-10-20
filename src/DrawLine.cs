using System;
using System.Numerics;
using Raylib_cs;

public class DrawLine
{
    //Draw the line using draw point and equation of a line given the two points on screen 
    //Equation of the line => y = m * (x - x_1) + y_1, to get the y-coordinate of the corresponding x coordinate 
    public static void DrawLineC(Vector2 startPoint, Vector2 endPoint, Color color)
    {
        float x1 = startPoint.X;
        float y1 = startPoint.Y;
        float x2 = endPoint.X;
        float y2 = endPoint.Y;

        //Drawing a vertical line will result in dividing by zero which is not good 
        //So do a bool check before this 
        bool isVertical = x2 - x1 == 0;
        float m = isVertical ? 0 : (y2 - y1) / (x2 - x1);

        for(int i = (int)x1; i <= x2; i++)
        {
            int y = (int)(m * (i - x1) + y1);
            //if (isVertical) Raylib.DrawCircle((int)x1, (int)x1, 1, color);
            Raylib.DrawCircle(i, y, 1, color);
        }
    }
}