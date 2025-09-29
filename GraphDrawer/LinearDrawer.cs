using Microsoft.Xna.Framework;
using MathTest.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MathTest.MathUtils;

namespace MathTest.GraphDrawer;

public static class LinearDrawer
{
    public static List<Line> Lines = new List<Line>();

    public static void AddLine(string function, Color color)
    {
        decimal m = ParseIntoFunction(function)[0];
        decimal b = ParseIntoFunction(function)[1];

        Line l = new Line(DetermineStart(m, b), DetermineEnd(m, b), color, thickness: 3);
        Lines.Add(l);
    }

    private static decimal[] ParseIntoFunction(string f)
    {
        f = f.Replace(" ", "");
        if (!f.StartsWith("y="))
            throw new ArgumentException("Function must start with 'y='");

        string expression = f.Substring(2);
        decimal m = 0m;
        decimal b = 0m;

        int xIndex = expression.IndexOf('x');
        if (xIndex == -1)
        {
            b = decimal.Parse(expression);
            m = 0m;
            return new decimal[] { m, b };
        }

        string mStr = expression.Substring(0, xIndex);
        if (string.IsNullOrEmpty(mStr))
        {
            m = 1m; // x alone
        }
        else if (mStr == "-")
        {
            m = -1m; // -x
        }
        else
        {
            if (mStr.StartsWith("(") && mStr.EndsWith(")"))
                mStr = mStr.Substring(1, mStr.Length - 2);

            m = Simplification.SimplifyConstantExpression(mStr);
        }

        string bStr = expression.Substring(xIndex + 1);
        if (!string.IsNullOrEmpty(bStr))
            b = decimal.Parse(bStr);

        return new decimal[] { m, b };
    }

    public static Vector2 DetermineStart(decimal m, decimal b)
    {
        bool negativeGradient = Math.Sign(m) == -1;

        // intercepts in terms of grid coordinates, not pixels
        decimal targY = negativeGradient ? Globals.Grid.TopOfPlane : Globals.Grid.BottomOfPlane;
        decimal targX = Globals.Grid.LeftOfPlane;

        // back to pixels
        float x, y;
        if (m == 0)
        {
            float px = Globals.Grid.xCoordToPosition((float)targX);
            float py = Globals.Grid.yCoordToPosition((float)b);
            return new Vector2(MathF.Max(px, 0), py);
        }
        else
        {
            x = Globals.Grid.xCoordToPosition((float)((targY - b) / m));
            y = Globals.Grid.yCoordToPosition((float)(m * targX + b));
        }

        return new Vector2(MathF.Max(x, 0), x < 0 ? y : Globals.Grid.yCoordToPosition((float)targY));
    }

    public static Vector2 DetermineEnd(decimal m, decimal b)
    {
        bool negativeGradient = Math.Sign(m) == -1;

        // intercepts in terms of grid coordinates, not pixels
        decimal targY = !negativeGradient ? Globals.Grid.TopOfPlane : Globals.Grid.BottomOfPlane;
        decimal targX = Globals.Grid.RightOfPlane;

        // back to pixels
        float x, y;
        if (m == 0)
        {
            float px = Globals.Grid.xCoordToPosition((float)targX);
            float py = Globals.Grid.yCoordToPosition((float)b);
            return new Vector2(MathF.Min(Globals.WindowWidth, px), py);
        }
        else
        {
            x = Globals.Grid.xCoordToPosition((float)((targY - b) / m));
            y = Globals.Grid.yCoordToPosition((float)(m * targX + b));
        }

        return new Vector2(MathF.Min(Globals.WindowWidth, x), x > Globals.WindowWidth ? y : Globals.Grid.yCoordToPosition((float)targY));
    }
}
