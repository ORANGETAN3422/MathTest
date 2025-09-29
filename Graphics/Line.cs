using MathTest;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MathTest.Graphics;
public class Line
{

    public Vector2 Start { get; set; }
    public Vector2 End { get; set; }
    public Color Color { get; set; }
    public float Thickness { get; set; }

    public Line(Vector2 start, Vector2 end, Color color, float thickness = 1f)
    {
        Start = start;
        End = end;
        Color = color;
        Thickness = thickness;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Vector2 edge = End - Start;
        float angle = (float)Math.Atan2(edge.Y, edge.X);
        float length = edge.Length();

        Vector2 origin = new Vector2(0, Thickness / 4f);

        spriteBatch.Draw(
            Globals.BlankTex,
            new Rectangle((int)Start.X, (int)Start.Y, (int)length, (int)Thickness),
            null,
            Color,
            angle,
            origin,
            SpriteEffects.None,
            0f
        );

    }
}