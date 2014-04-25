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
using Arcadia.Graphics;
#endregion

namespace Arcadia.Gamestates.Tetris
{
    class TetrisGameScreen : GameScreen
    {
        #region Fields

        //fonts
        SpriteFont font;

        //block
        Sprite redBlock;
        Sprite blueBlock;
        Sprite greenBlock;
        Sprite purpleBlock;
        Sprite greyBlock;
        Sprite orangeBlock;
        Sprite yellowBlock;

        //arrays
        Sprite[]    redB;
        Sprite[]    blueB;
        Sprite[]    greenB;
        Sprite[]    greyB;
        Sprite[]    purpleB;
        Sprite[]    orangeB;
        Sprite[]    yellowB;
        Sprite[]    currentArray;
        Sprite[,]  collisionArray;

        //tells if block reach its collision
        bool done = true;
        bool collide = false;

        //number to hold the random number
        int randomNumber = 0;

        //keyboard states
        KeyboardState currentKBState;
        KeyboardState previousKBState;

        //holds shapes
        int shape = 1;

        //counter
        int count = 0;

        //block speed
        int speed = 2;

        public ContentManager content;

        #endregion

        #region Initialization

        public TetrisGameScreen()
        {
            TransitionOnTime  = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            
            string ContentLoadDir = "Sprite/Tetris/";
            
            //Font
            font        = content.Load<SpriteFont>("Font/asteroidFont");

            //blocks
            redBlock    = new Sprite(content.Load<Texture2D>(ContentLoadDir + "redTile"));
            greyBlock   = new Sprite(content.Load<Texture2D>(ContentLoadDir + "silverTile"));
            blueBlock   = new Sprite(content.Load<Texture2D>(ContentLoadDir + "blueTile"));
            greenBlock  = new Sprite(content.Load<Texture2D>(ContentLoadDir + "greenTile"));
            purpleBlock = new Sprite(content.Load<Texture2D>(ContentLoadDir + "purpleTile"));
            orangeBlock = new Sprite(content.Load<Texture2D>(ContentLoadDir + "orangeTile"));
            yellowBlock = new Sprite(content.Load<Texture2D>(ContentLoadDir + "yellowTile"));

            //initialize arrays
            redB = new Sprite[4];
            blueB = new Sprite[4];
            greenB = new Sprite[4];
            greyB = new Sprite[4];
            purpleB = new Sprite[4];
            orangeB = new Sprite[4];
            yellowB = new Sprite[4];
            currentArray = new Sprite[4];
            collisionArray = new Sprite[16,32];

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        #endregion

        #region HandleInput

        public override void HandleInput(InputState input)
        {
            currentKBState  = Keyboard.GetState();

            if (currentKBState.IsKeyDown(Keys.Left) && previousKBState.IsKeyUp(Keys.Left))
            {
                currentArray[0].Position = new Vector2(currentArray[0].Position.X - 20, currentArray[0].Position.Y);
                currentArray[1].Position = new Vector2(currentArray[1].Position.X - 20, currentArray[1].Position.Y);
                currentArray[2].Position = new Vector2(currentArray[2].Position.X - 20, currentArray[2].Position.Y);
                currentArray[3].Position = new Vector2(currentArray[3].Position.X - 20, currentArray[3].Position.Y);
            }
            
            if(currentKBState.IsKeyDown(Keys.Right) && previousKBState.IsKeyUp(Keys.Right))
            {
                currentArray[0].Position = new Vector2(currentArray[0].Position.X + 20, currentArray[0].Position.Y);
                currentArray[1].Position = new Vector2(currentArray[1].Position.X + 20, currentArray[1].Position.Y);
                currentArray[2].Position = new Vector2(currentArray[2].Position.X + 20, currentArray[2].Position.Y);
                currentArray[3].Position = new Vector2(currentArray[3].Position.X + 20, currentArray[3].Position.Y);
            }

            if (currentKBState.IsKeyDown(Keys.Down))
                speed = 10;
            if (previousKBState.IsKeyUp(Keys.Down))
                speed = 2;
             
            if (currentKBState.IsKeyUp(Keys.Space) && previousKBState.IsKeyDown(Keys.Space))
            {
                //for square
                if (randomNumber == 1 && shape == 1)
                {
                    shape = 1;
                }
                //rotate the line shape
                else if (randomNumber == 2 && shape == 1)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y + 20);
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X, currentArray[1].Position.Y + 20);
                    currentArray[3].Position = new Vector2(currentArray[1].Position.X, currentArray[2].Position.Y + 20);
                    
                    shape = 2;
                }
                else if (randomNumber == 2 && shape == 2)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X + 20, currentArray[0].Position.Y);
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X + 20, currentArray[1].Position.Y);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X + 20, currentArray[2].Position.Y);

                    shape = 1;
                }
                //rotate the T shape
                else if (randomNumber == 3 && shape == 1)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);
                    currentArray[2].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y + 20);
                    currentArray[3].Position = new Vector2(currentArray[0].Position.X - 20, currentArray[0].Position.Y);

                    shape = 2;
                }
                else if (randomNumber == 3 && shape == 2)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X + 20, currentArray[0].Position.Y);
                    currentArray[2].Position = new Vector2(currentArray[0].Position.X - 20, currentArray[0].Position.Y);
                    currentArray[3].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);

                    shape = 3;
                }
                else if (randomNumber == 3 && shape == 3)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y + 20);
                    currentArray[2].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);
                    currentArray[3].Position = new Vector2(currentArray[0].Position.X + 20, currentArray[0].Position.Y);

                    shape = 4;
                }
                else if (randomNumber == 3 && shape == 4)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X - 20, currentArray[0].Position.Y);
                    currentArray[2].Position = new Vector2(currentArray[0].Position.X + 20, currentArray[0].Position.Y);
                    currentArray[3].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y + 20);

                    shape = 1;
                }
                //rotate z shape
                else if (randomNumber == 4 && shape == 1)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);
                    currentArray[2].Position = new Vector2(currentArray[0].Position.X - 20, currentArray[0].Position.Y);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X, currentArray[0].Position.Y + 20);

                    shape = 2;
                }
                else if (randomNumber == 4 && shape == 2)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X -20, currentArray[0].Position.Y);
                    currentArray[2].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y + 20);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X + 20, currentArray[2].Position.Y);

                    shape = 1;
                }
                //rotate s shape
                else if (randomNumber == 5 && shape == 1)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y + 20);
                    currentArray[2].Position = new Vector2(currentArray[0].Position.X - 20, currentArray[0].Position.Y);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X, currentArray[0].Position.Y - 20);

                    shape = 2;
                }
                else if (randomNumber == 5 && shape == 2)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X + 20, currentArray[0].Position.Y);
                    currentArray[2].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y + 20);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X - 20, currentArray[2].Position.Y);

                    shape = 1;
                }
                //rotate L shape
                else if (randomNumber == 6 && shape == 1)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X - 20, currentArray[0].Position.Y);
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X - 20, currentArray[1].Position.Y);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X, currentArray[2].Position.Y + 20);

                    shape = 2;
                }
                else if (randomNumber == 6 && shape == 2)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X, currentArray[1].Position.Y - 20);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X - 20, currentArray[2].Position.Y);

                    shape = 3;
                }
                else if (randomNumber == 6 && shape == 3)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X + 20, currentArray[0].Position.Y);
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X + 20, currentArray[1].Position.Y);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X, currentArray[2].Position.Y - 20);

                    shape = 4;
                }
                else if (randomNumber == 6 && shape == 4)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y + 20);
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X, currentArray[1].Position.Y + 20);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X + 20, currentArray[2].Position.Y);

                    shape = 1;
                }
                //rotate backward L shape
                else if (randomNumber == 7 && shape == 1)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X - 20, currentArray[0].Position.Y);
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X - 20, currentArray[1].Position.Y);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X, currentArray[2].Position.Y - 20);

                    shape = 2;
                }
                else if (randomNumber == 7 && shape == 2)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X, currentArray[1].Position.Y - 20);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X + 20, currentArray[2].Position.Y);

                    shape = 3;
                }
                else if (randomNumber == 7 && shape == 3)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X + 20, currentArray[0].Position.Y);
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X + 20, currentArray[1].Position.Y);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X, currentArray[2].Position.Y + 20);

                    shape = 4;
                }
                else if (randomNumber == 7 && shape == 4)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y + 20);
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X, currentArray[1].Position.Y + 20);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X - 20, currentArray[2].Position.Y);

                    shape = 1;
                }
            }

            previousKBState = currentKBState;
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (done)
            {
                CreateShape();
                done = false;
            }
            else if (!done)
            {
                count += speed;
                if (count >= 60)
                {
                    currentArray[0].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y + 20);
                    currentArray[1].Position = new Vector2(currentArray[1].Position.X, currentArray[1].Position.Y + 20);
                    currentArray[2].Position = new Vector2(currentArray[2].Position.X, currentArray[2].Position.Y + 20);
                    currentArray[3].Position = new Vector2(currentArray[3].Position.X, currentArray[3].Position.Y + 20);
                    
                    count = 0;
                }
                if (currentArray[0].Position.Y >= 400)
                {
                    collide = true;
                }
            }

            if (collide)
            {
                count = 0;
                done = true;
                collide = false;
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void CreateShape()
        {
            //local variables
            Random randomNum = new Random();
            int num = randomNum.Next(1, 8);
            if (num == 8)
            {
                num = 7;
            }
            //transfer the random number
            randomNumber = num;

            //yellow Square
            if (num == 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    yellowB[i] = new Sprite(yellowBlock.Texture);
                }

                yellowB[0].Position = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 2) - 10, 10);
                yellowB[1].Position = new Vector2(yellowB[0].Position.X + 20, yellowB[0].Position.Y);
                yellowB[2].Position = new Vector2(yellowB[0].Position.X, yellowB[0].Position.Y + 20);
                yellowB[3].Position = new Vector2(yellowB[0].Position.X + 20, yellowB[0].Position.Y + 20);

                for (int i = 0; i < 4; i++)
                {
                    currentArray[i] = yellowB[i];
                }
            }
            //orange line
            else if (num == 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    orangeB[i] = new Sprite(orangeBlock.Texture);
                }

                orangeB[0].Position = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 2) - 30, 10);
                orangeB[1].Position = new Vector2(orangeB[0].Position.X + 20, orangeB[0].Position.Y);
                orangeB[2].Position = new Vector2(orangeB[1].Position.X + 20, orangeB[0].Position.Y);
                orangeB[3].Position = new Vector2(orangeB[2].Position.X + 20, orangeB[0].Position.Y);

                for (int i = 0; i < 4; i++)
                {
                    currentArray[i] = orangeB[i];
                }
            }
            //grey T
            else if (num == 3)
            {
                for (int i = 0; i < 4; i++)
                {
                    greyB[i] = new Sprite(greyBlock.Texture);
                }

                greyB[0].Position = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 2) - 10, 10);
                greyB[1].Position = new Vector2(greyB[0].Position.X - 20, greyB[0].Position.Y);
                greyB[2].Position = new Vector2(greyB[0].Position.X + 20, greyB[0].Position.Y);
                greyB[3].Position = new Vector2(greyB[0].Position.X, greyB[0].Position.Y + 20);

                for (int i = 0; i < 4; i++)
                {
                    currentArray[i] = greyB[i];
                }
            }
            //red z
            else if (num == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    redB[i] = new Sprite(redBlock.Texture);
                }

                redB[0].Position = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 2) - 10, 10);
                redB[1].Position = new Vector2(redB[0].Position.X - 20, redB[0].Position.Y);
                redB[2].Position = new Vector2(redB[0].Position.X, redB[0].Position.Y + 20);
                redB[3].Position = new Vector2(redB[2].Position.X + 20, redB[2].Position.Y);

                for (int i = 0; i < 4; i++)
                {
                    currentArray[i] = redB[i];
                }
            }
            //blue s
            else if (num == 5)
            {
                for (int i = 0; i < 4; i++)
                {
                    blueB[i] = new Sprite(blueBlock.Texture);
                }

                blueB[0].Position = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 2) - 10, 10);
                blueB[1].Position = new Vector2(blueB[0].Position.X + 20, blueB[0].Position.Y);
                blueB[2].Position = new Vector2(blueB[0].Position.X, blueB[0].Position.Y + 20);
                blueB[3].Position = new Vector2(blueB[2].Position.X - 20, blueB[2].Position.Y);

                for (int i = 0; i < 4; i++)
                {
                    currentArray[i] = blueB[i];
                }
            }
            //green L
            else if (num == 6)
            {
                for (int i = 0; i < 4; i++)
                {
                    greenB[i] = new Sprite(greenBlock.Texture);
                }

                greenB[0].Position = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 2) - 10, 10);
                greenB[1].Position = new Vector2(greenB[0].Position.X, greenB[0].Position.Y + 20);
                greenB[2].Position = new Vector2(greenB[0].Position.X, greenB[1].Position.Y + 20);
                greenB[3].Position = new Vector2(greenB[0].Position.X + 20, greenB[2].Position.Y);

                for (int i = 0; i < 4; i++)
                {
                    currentArray[i] = greenB[i];
                }
            }
            //purple backward L 
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    purpleB[i] = new Sprite(purpleBlock.Texture);
                }

                purpleB[0].Position = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 2) - 10, 10);
                purpleB[1].Position = new Vector2(purpleB[0].Position.X, purpleB[0].Position.Y + 20);
                purpleB[2].Position = new Vector2(purpleB[0].Position.X, purpleB[1].Position.Y + 20);
                purpleB[3].Position = new Vector2(purpleB[0].Position.X - 20, purpleB[2].Position.Y);

                for (int i = 0; i < 4; i++)
                {
                    currentArray[i] = purpleB[i];
                }
            }
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "This is the tetris game screen", new Vector2(400, 300), Color.White);
            if (!done)
            {
                for (int i = 0; i < 4; i++)
                {
                    spriteBatch.Draw(currentArray[i].Texture,
                        currentArray[i].Position,
                        null,
                        Color.White,
                        currentArray[i].Rotation,
                        currentArray[i].Center,
                        currentArray[i].Scale / 2,
                        SpriteEffects.None,
                        1.0f);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}
