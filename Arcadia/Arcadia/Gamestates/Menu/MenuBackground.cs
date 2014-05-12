using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Arcadia.Screen;
using Arcadia.Graphics;

namespace Arcadia.Gamestates.Menu
{
    class MenuBackground : GameScreen
    {
        private StaticSprite logo;
        private StaticSprite shimmer;
        private StaticSprite background;
        private bool isScrolling = true;

        public MenuBackground()
        {

        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            Viewport vp = ScreenManager.GraphicsDevice.Viewport;

            logo = new StaticSprite();
            logo.Texture = content.Load<Texture2D>("Sprite/Arcadia");
            logo.Position = new Vector2((vp.Width - logo.Texture.Width) / 2, 600);
            logo.IsVisible = true;

            shimmer = new StaticSprite();
            shimmer.Texture = content.Load<Texture2D>("Sprite/ArcadiaShimmer");
            shimmer.Position = logo.Position;
            shimmer.IsVisible = false;

            background = new StaticSprite();
            background.Texture = content.Load<Texture2D>("Sprite/backGroundForArcadia");
            background.WillStretchToCollisionBox = true;
            background.IsVisible = false;
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            // Scroll the screen
            if (isScrolling)
            {
                logo.Position = new Vector2(logo.Position.X, logo.Position.Y - 4);
                shimmer.Position = logo.Position;

                if (logo.Position.Y <= 50)
                {
                    background.IsVisible = true;
                    shimmer.IsVisible = true;
                    isScrolling = false;
                    ScreenManager.AddScreen(new MainMenuScreen(), null);
                }
            }
            

            // coveredByOtherScreen set to false by default so we don't erase
            // this screen when it is covered by another screen.
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            background.CollisionBox = fullscreen;

            spriteBatch.Begin();

            background.Draw(spriteBatch);
            logo.Draw(spriteBatch);
            shimmer.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
