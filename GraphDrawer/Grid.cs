using FontStashSharp;
using MathTest.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MathTest.GraphDrawer;

public class Grid
{

    private int gapIntervals = 30;
    private int axisWidth = 3;
    public Color axisColor = new Color(255, 204, 102);
    public Color gridLineColor = new Color(51, 51, 51);
    public Color[] lineColor = new Color[3] { new Color(102, 255, 255), new Color(255, 102, 255), new Color(102, 255, 102) };

    public Vector2 Origin { get; set; } = new Vector2(Globals.WindowWidth / 2, Globals.WindowHeight / 2);

    private Line xAxis;
    private Line yAxis;

    private Vector2[] xCoords;
    private Vector2[] yCoords;

    public int LeftOfPlane => (int)Origin.X / -gapIntervals;
    public int RightOfPlane => (int)Origin.X / gapIntervals;

    public int TopOfPlane => (int)Origin.Y / gapIntervals;
    public int BottomOfPlane => (int)Origin.Y / -gapIntervals;

    public decimal intervalsPerPixel { get; set; }

    /// <summary>
    /// Creates a cartesian plane
    /// </summary>
    public Grid()
    {
        InitalizePlaneAxes();
        intervalsPerPixel = 1m / gapIntervals;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        DrawGridLines(spriteBatch);

        xAxis.Draw(spriteBatch);
        yAxis.Draw(spriteBatch);

        DrawAxesLabels(spriteBatch);

        DrawLinear(spriteBatch);
    }

    public float xCoordToPosition(float x) => Origin.X + (x * gapIntervals);
    public float yCoordToPosition(float y) => Origin.Y - (y * gapIntervals);

    private void DrawLinear(SpriteBatch spriteBatch)
    {
        foreach (Line l in LinearDrawer.Lines)
        {
            l.Draw(spriteBatch);
        }
    }

    private void InitalizePlaneAxes()
    {
        xCoords = new Vector2[]
        {
        new Vector2(0, Globals.WindowHeight / 2 - axisWidth / 2),
        new Vector2(Globals.WindowWidth, Globals.WindowHeight / 2 - axisWidth / 2)
        };

        yCoords = new Vector2[]
        {
        new Vector2(Globals.WindowWidth / 2 - axisWidth / 2, 0),
        new Vector2(Globals.WindowWidth / 2 - axisWidth / 2, Globals.WindowHeight)
        };

        xAxis = new Line(xCoords[0], xCoords[1], axisColor, thickness: axisWidth);
        yAxis = new Line(yCoords[0], yCoords[1], axisColor, thickness: axisWidth);
    }

    private void DrawGridLines(SpriteBatch spriteBatch)
    {
        // X axis
        for (int y = (int)Origin.Y; y < Globals.WindowHeight; y += gapIntervals)
        {
            Line line = new Line(new Vector2(0, y), new Vector2(Globals.WindowWidth, y), gridLineColor, thickness: 2);
            line.Draw(spriteBatch);
        }
        for (int y = (int)Origin.Y; y > 0; y -= gapIntervals)
        {
            Line line = new Line(new Vector2(0, y), new Vector2(Globals.WindowWidth, y), gridLineColor, thickness: 2);
            line.Draw(spriteBatch);
        }


        // Y axis
        for (int x = (int)Origin.X; x < Globals.WindowWidth; x += gapIntervals)
        {
            Line line = new Line(new Vector2(x, 0), new Vector2(x, Globals.WindowHeight), gridLineColor, thickness: 2);
            line.Draw(spriteBatch);
        }

        for (int x = (int)Origin.X; x > 0; x -= gapIntervals)
        {
            Line line = new Line(new Vector2(x, 0), new Vector2(x, Globals.WindowHeight), gridLineColor, thickness: 2);
            line.Draw(spriteBatch);
        }
    }

    private void DrawAxesLabels(SpriteBatch spriteBatch)
    {
        if (Globals.MainFont == null) return;

        // X axis
        for (int x = (int)Origin.X; x < Globals.WindowWidth; x += gapIntervals)
        {
            int value = (x - (int)Origin.X) / gapIntervals;
            if (value == 0) continue;

            string text = value.ToString();
            var textSize = Globals.MainFont.MeasureString(text);
            Globals.MainFont.DrawText(spriteBatch, text, new Vector2(x - textSize.X / 2, Origin.Y + 5), Color.White);
        }

        for (int x = (int)Origin.X - gapIntervals; x > 0; x -= gapIntervals)
        {
            int value = (x - (int)Origin.X) / gapIntervals;
            string text = value.ToString();
            var textSize = Globals.MainFont.MeasureString(text);
            Globals.MainFont.DrawText(spriteBatch, text, new Vector2(x - textSize.X / 2, Origin.Y + 5), Color.White);
        }

        // Y axis
        for (int y = (int)Origin.Y; y < Globals.WindowHeight; y += gapIntervals)
        {
            int value = -((y - (int)Origin.Y) / gapIntervals);
            if (value == 0) continue;

            string text = value.ToString();
            var textSize = Globals.MainFont.MeasureString(text);
            Globals.MainFont.DrawText(spriteBatch, text, new Vector2(Origin.X + 5, y - textSize.Y / 2), Color.White);
        }

        for (int y = (int)Origin.Y - gapIntervals; y > 0; y -= gapIntervals)
        {
            int value = -((y - (int)Origin.Y) / gapIntervals);
            string text = value.ToString();
            var textSize = Globals.MainFont.MeasureString(text);
            Globals.MainFont.DrawText(spriteBatch, text, new Vector2(Origin.X + 5, y - textSize.Y / 2), Color.White);
        }

        // Origin
        string zero = "0";
        var zeroSize = Globals.MainFont.MeasureString(zero);
        Globals.MainFont.DrawText(spriteBatch, zero, Origin + new Vector2(-zeroSize.X - 8, 5), Color.White);
    }

}
