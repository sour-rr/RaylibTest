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
        bool isSteep = Math.Abs(m) > 1;

        int startY = Math.Min((int)y1, (int)y2);
        int endY = Math.Max((int)y1, (int)y2);
        if (isVertical) //draw the vertical lines (same x-coordinates)
        {
            for (int i = startY; i <= endY; i++)
            {
                Raylib.DrawCircle((int)x1, i, 1, color);
            }
            return;
        }

        int startX = Math.Min((int)x1, (int)x2);
        int endX = Math.Max((int)x1, (int)x2);

        if (isSteep) //inverse x and y to get more pixel integer values across y instead of x due to steepness
        {
            for (int i = startY; i <= endY; i++)
            {
                int x = (int)(1 / m * (i - y1) + x1);
                Raylib.DrawCircle(x, i, 1, color);
            }
        }
        else
        {
            for(int i = startX; i <= endX; i++)
            {
                int y = (int)(m * (i - x1) + y1);
                Raylib.DrawCircle(i, y, 1, color);
            }  
        }
    }
}