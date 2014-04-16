#region Information
/* Tetris, a game where a player need to build a complete
 * horizontal line to clear the blocks of line in the field
 * until all the blocks are gone.
 *      Written by: Norlan Prudente
 *      Date: 02/20/2014
 */
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arcadia;
using Arcadia.Screen;
#endregion

namespace Arcadia.Gamestates.Tetris
{
    class TetrisGameScreen : GameScreen
    {
        #region Fields

        public ContentManager content;

        SpriteFont font;

        #endregion

        #region Initialization

        public TetrisGameScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            //Font
            font = content.Load<SpriteFont>("Font/asteroidFont");

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "This is the tetris game screen", new Vector2(400, 300), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion
    }
}
