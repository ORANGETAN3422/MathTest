using MathTest.GraphDrawer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using FontStashSharp;
using System.IO;

namespace MathTest;

public static class Globals
{
    public static int WindowWidth = 1366;
    public static int WindowHeight = 758;

    public static Texture2D BlankTex { get; private set; }
    public static FontSystem FontSystem;
    public static DynamicSpriteFont MainFont { get; private set; }
    public static float FontSize = 18;

    public static Grid Grid { get; private set; }
    public static GridDisplayControls GridDisplayControls { get; private set; }

    public static void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        BlankTex = new Texture2D(graphicsDevice, 1, 1);
        BlankTex.SetData(new[] { Color.White });

        FontSystem = new FontSystem();
        FontSystem.AddFont(File.ReadAllBytes("Content/Fonts/Asana-Math.otf"));
        MainFont = FontSystem.GetFont(FontSize);

        Grid = new Grid();
        GridDisplayControls = new GridDisplayControls();
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        if (Grid != null)
            Grid.Draw(spriteBatch);
        
        if (GridDisplayControls != null)
            GridDisplayControls.Draw(spriteBatch);
    }
}
