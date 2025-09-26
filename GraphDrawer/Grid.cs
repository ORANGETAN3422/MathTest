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
    private int pixelIntervals;
    private int axisWidth = 3;

    private Line xAxis;
    private Line yAxis;

    private Vector2[] xCoords;
    private Vector2[] yCoords;

    /// <summary>
    /// Creates the 1st quadrant of a cartesian plane
    /// </summary>
    /// <param name="width">the width of the grid in pixels</param>
    /// <param name="height">the height of the grid in pixels</param>
    /// <param name="pixelIntervals">the amount of pixels between an increment in x or y</param>
    public Grid()
    {
        InitalizeGridLines();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        xAxis.Draw(spriteBatch);
        yAxis.Draw(spriteBatch);

        //DrawAxesIntervals(spriteBatch);
    }


    private void InitalizeGridLines()
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

        xAxis = new Line(xCoords[0], xCoords[1], Color.White, thickness: axisWidth);
        yAxis = new Line(yCoords[0], yCoords[1], Color.White, thickness: axisWidth);
    }

    /*private void DrawAxesIntervals(SpriteBatch spriteBatch)
    {
        Globals.MainFont.DrawText(spriteBatch, "0", new Vector2(GridSize.X - 12, GridSize.Y + GridSize.Height + 4), Color.White);

        int x = 1;
        for (int i = pixelIntervals; i <= GridSize.Width; i += pixelIntervals)
        {
            float tickX = GridSize.X + i;
            if (tickX > GridSize.Right) break;

            Vector2 start = new Vector2(tickX, GridSize.Bottom);
            Vector2 end = new Vector2(tickX, GridSize.Bottom + 6);
            Line tick = new Line(start, end, Color.White, 1);
            tick.Draw(spriteBatch);

            string label = x.ToString();
            Vector2 labelSize = Globals.MainFont.MeasureString(label);
            float lineHeight = Globals.MainFont.LineHeight;
            Vector2 labelPos = new Vector2(
                tickX - labelSize.X / 2,
                GridSize.Bottom + lineHeight * 0.2f
            );
            DynamicSpriteFont font = Globals.FontSystem.GetFont(Globals.FontSize);
            font.DrawText(spriteBatch, label, labelPos, Color.White);

            x++;
        }

        int y = 1;
        for (int i = pixelIntervals; i <= GridSize.Height; i += pixelIntervals)
        {
            float tickY = GridSize.Bottom - i;
            if (tickY < GridSize.Top) break;

            Vector2 start = new Vector2(GridSize.Left - 6, tickY);
            Vector2 end = new Vector2(GridSize.Left, tickY);
            Line tick = new Line(start, end, Color.White, 1);
            tick.Draw(spriteBatch);

            string label = y.ToString();
            Vector2 labelSize = Globals.MainFont.MeasureString(label);
            float lineHeight = Globals.MainFont.LineHeight;
            Vector2 labelPos = new Vector2(
                GridSize.Left - labelSize.X - 8,
                tickY - lineHeight / 2
            );
            DynamicSpriteFont font = Globals.FontSystem.GetFont(Globals.FontSize);
            font.DrawText(spriteBatch, label, labelPos, Color.White);

            y++;
        }
    }*/
}
