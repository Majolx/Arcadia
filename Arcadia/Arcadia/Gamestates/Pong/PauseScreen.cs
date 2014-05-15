using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Arcadia.Screen;

namespace Arcadia.Gamestates.Pong
{
    class PauseScreen : GameScreen
    {
        #region Fields

        // Message text that goes underneath the pause screen title.
        string message;

        #endregion

        #region Events

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        #endregion

        #region Initialization

        public PauseScreen(string message)
            : this(message, true)
        { }

        public PauseScreen(string message, bool includeUsageText)
        {
            const string usageText = "\nPress Esc to unpause" +
                                     "\nPress Enter or Space to quit";

            if (includeUsageText)
                this.message = message + usageText;
            else
                this.message = message;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        #endregion

        #region Handle Input


        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;

            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                // Raise the accepted event, then exit the message box.
                if (Accepted != null)
                    Accepted(this, new PlayerIndexEventArgs(playerIndex));

                // Reset the global pause state to false
                ScreenManager.IsPaused = false;

                // Restart the game
                ScreenManager.RestartGame();
            }
            else if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                // Raise the cancelled event, then exit the message box.
                if (Cancelled != null)
                    Cancelled(this, new PlayerIndexEventArgs(playerIndex));

                // Reset the global pause state to false.
                ScreenManager.IsPaused = false;

                ExitScreen();
            }
        }

        #endregion

        #region Draw
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.ArcadeFont;

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;

            spriteBatch.Begin();

            spriteBatch.DrawString(font, message, textPosition, Color.White);

            spriteBatch.End();
        }

        #endregion
    }
}
