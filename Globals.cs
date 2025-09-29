using MathTest.GraphDrawer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using FontStashSharp;
using System.IO;
using MathTest.MathUtils;
using System.Diagnostics;

namespace MathTest;

public static class Globals
{
    public static int WindowWidth = 1366;
    public static int WindowHeight = 758;

    public static Texture2D BlankTex { get; private set; }
    public static FontSystem FontSystem;
    public static DynamicSpriteFont MainFont { get; private set; }
    public static float FontSize = 24;

    public static Grid Grid { get; private set; }

    public static void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        BlankTex = new Texture2D(graphicsDevice, 1, 1);
        BlankTex.SetData(new[] { Color.White });

        FontSystem = new FontSystem();
        FontSystem.AddFont(File.ReadAllBytes("Content/Fonts/cmunrm.ttf"));
        MainFont = FontSystem.GetFont(FontSize);

        Grid = new Grid();

        LinearDrawer.AddLine("y=3x+2", Grid.lineColor[0]);
        LinearDrawer.AddLine("y=1/2x", Grid.lineColor[1]);
        LinearDrawer.AddLine("y=2", Grid.lineColor[2]);
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        if (Grid != null)
            Grid.Draw(spriteBatch);
    }
}
