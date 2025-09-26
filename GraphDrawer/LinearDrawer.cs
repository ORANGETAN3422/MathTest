using MathTest.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MathTest.GraphDrawer;

public static class LinearDrawer
{
    public static List<Line> Lines = new List<Line>();

    public static void DrawLine(string function)
    {
        ParseIntoFunction("y = 4x + 2");
    }

    private static void ParseIntoFunction(string f)
    {
        //break into terms
        string[] terms = f.Split(' ');
        string xTerm = "";
        int m = 0;

        //find x
        foreach (string s in terms)
        {
            if (s.ToLower().Contains("x"))
            {
                xTerm = s; 
                break;
            }
        }

        //figure out gradient
        if (xTerm != string.Empty)
        {
            string stringGradient = xTerm.Replace("x", "");

            if (stringGradient.Contains("/"))
            {
                string[] gradientParts = stringGradient.Split("/");
                if (gradientParts.Length <= 1) return;
                double baseNumber = int.Parse(gradientParts[0]);

                for (int i = 1; i < gradientParts.Length; i++)
                {
                    double n = int.Parse(gradientParts[i]);
                    if (n == 0) break;
                    baseNumber /= n;
                }
            }
            else if (stringGradient.Contains("*"))
            {

            }
            else
            {
                m = int.Parse(stringGradient);
            }
        }
        //figure out y intercept
        // buh idk i gotta collect every other term

        Debug.WriteLine(terms);
        Debug.WriteLine(xTerm);
        Debug.WriteLine(m);
    }

    private static Vector2 DetermineStart()
    {
        return Vector2.Zero;
    }

    private static Vector2 DetermineEnd()
    {
        return Vector2.Zero;
    }
}
