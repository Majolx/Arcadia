using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arcadia.Screen
{
    class CreditsScreen : GameScreen
    {
        private float startY = 600;
        private float speed = .25f;
        private float boost = 1.5f;
        private bool isBoosting = false;

        private string[] credits = { "Arcadia",
                                     "",
                                     "",
                                     "",
                                     "Created by",
                                     "",
                                     "Chips and caffeine",
                                     "",
                                     "",
                                     "Developed by",
                                     "",
                                     "Norlan Prudente",
                                     "Mathew Larribas" };

        private Vector2[] v2Position;

        public CreditsScreen()
        {
            v2Position = new Vector2[credits.Length];

            
        }

        public override void LoadContent()
        {
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            SpriteFont font = ScreenManager.Font;

            int space = 5;
            float lineSpacing = font.MeasureString("T").Y + space;
            for (int i = 0; i < v2Position.Length; i++)
            {
                v2Position[i].X = (viewport.Width - font.MeasureString(credits[i]).X) / 2;
                v2Position[i].Y = startY + i * lineSpacing;
            }

            base.LoadContent();
        }

        public override void HandleInput(InputState input)
        {
            if (input.IsKeyDown(Keys.Space))
            {
                if (!isBoosting)
                    isBoosting = true;
            }
            else
                isBoosting = false;

            PlayerIndex playerIndex;
            if (input.IsNewKeyPress(Keys.Escape, null, out playerIndex))
                this.ExitScreen();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (v2Position[v2Position.Length - 1].Y > 0)
            {
                for (int i = 0; i < v2Position.Length; i++)
                {
                    v2Position[i].Y -= speed;
                    if (isBoosting)
                    {
                        v2Position[i].Y -= boost;
                    }
                }
            }
            else
            {
                this.ExitScreen();
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;
            Color color = Color.White;

            spriteBatch.Begin();
            
            for (int i = 0; i < credits.Length; i++)
            {
                spriteBatch.DrawString(font, credits[i], v2Position[i], color);
            }

            spriteBatch.End();
        }

    }
}
