using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathTest.GraphDrawer
{
    public class GridDisplayControls
    {
        private Rectangle ControlBounds;
        private Rectangle[] ButtonBounds;
        private const int Padding = 10;

        public GridDisplayControls()
        {
            int panelWidth = 200;
            int panelHeight = 100;

            ControlBounds = new Rectangle(
                Padding,
                Globals.WindowHeight - panelHeight - Padding,
                panelWidth,
                panelHeight
            );

            ButtonBounds = new Rectangle[4];

            int buttonWidth = (panelWidth - Padding * 5) / 2;
            int buttonHeight = (panelHeight - Padding * 3) / 2;

            for (int i = 0; i < 4; i++)
            {
                int row = i / 2;
                int col = i % 2;

                int x = ControlBounds.X + Padding + col * (buttonWidth + Padding);
                int y = ControlBounds.Y + Padding + row * (buttonHeight + Padding);

                ButtonBounds[i] = new Rectangle(x, y, buttonWidth, buttonHeight);
            }
        }

        public void Update()
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Globals.BlankTex, ControlBounds, Color.White);

            foreach (Rectangle btn in ButtonBounds)
            {
                spriteBatch.Draw(Globals.BlankTex, btn, Color.LightBlue);
            }
        }
    }
}
