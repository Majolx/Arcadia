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
        Sprite border;
        

        //arrays
        Sprite[]    redB;
        Sprite[]    blueB;
        Sprite[]    greenB;
        Sprite[]    greyB;
        Sprite[]    purpleB;
        Sprite[]    orangeB;
        Sprite[]    yellowB;
        Sprite[]    currentArray;
        Sprite[]    leftBorder;
        Sprite[]    rightBorder;
        Sprite[]    bottomBorder;
        Sprite[,]   collisionArray;
        

        //tells if block reach its collision
        bool done = true;
        bool collide = false;
        bool setup = false;

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

        //score holder
        int score = 0;

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
            font            = content.Load<SpriteFont>("Font/asteroidFont");

            //blocks
            redBlock        = new Sprite(content.Load<Texture2D>(ContentLoadDir + "redTile"));
            greyBlock       = new Sprite(content.Load<Texture2D>(ContentLoadDir + "silverTile"));
            blueBlock       = new Sprite(content.Load<Texture2D>(ContentLoadDir + "blueTile"));
            greenBlock      = new Sprite(content.Load<Texture2D>(ContentLoadDir + "greenTile"));
            purpleBlock     = new Sprite(content.Load<Texture2D>(ContentLoadDir + "purpleTile"));
            orangeBlock     = new Sprite(content.Load<Texture2D>(ContentLoadDir + "orangeTile"));
            yellowBlock     = new Sprite(content.Load<Texture2D>(ContentLoadDir + "yellowTile"));
            border          = new Sprite(content.Load<Texture2D>(ContentLoadDir + "whiteBorder"));
            
            //initialize arrays
            redB            = new Sprite[4];
            blueB           = new Sprite[4];
            greenB          = new Sprite[4];
            greyB           = new Sprite[4];
            purpleB         = new Sprite[4];
            orangeB         = new Sprite[4];
            yellowB         = new Sprite[4];
            currentArray    = new Sprite[4];
            collisionArray  = new Sprite[20,16];
            leftBorder      = new Sprite[22];
            rightBorder     = new Sprite[22];
            bottomBorder    = new Sprite[40];

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
                if (setup && !done)
                {
                    //collision for left border
                    for (int j = 0; j < 4; j++)
                    {
                        if (leftBorder[0].Position.X + 40 > currentArray[j].Position.X)
                        {
                            currentArray[0].Position = new Vector2(currentArray[0].Position.X + 20, currentArray[0].Position.Y);
                            currentArray[1].Position = new Vector2(currentArray[1].Position.X + 20, currentArray[1].Position.Y);
                            currentArray[2].Position = new Vector2(currentArray[2].Position.X + 20, currentArray[2].Position.Y);
                            currentArray[3].Position = new Vector2(currentArray[3].Position.X + 20, currentArray[3].Position.Y);
                        }
                    }
   
                    //collision in two dimmentional array
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 20; j++)
                        {
                            for (int k = 0; k < 16; k++)
                            {
                                if (collisionArray[j, k] != null)
                                {
                                    if (collisionArray[j, k].Position.Y < currentArray[i].Position.Y + 10 &&
                                        collisionArray[j, k].Position.Y + 20 > currentArray[i].Position.Y + 10 &&
                                        collisionArray[j, k].Position.X + 40 > currentArray[i].Position.X &&
                                        collisionArray[j, k].Position.X + 40 < currentArray[i].Position.X + 10)
                                    {
                                        currentArray[0].Position = new Vector2(currentArray[0].Position.X + 20, currentArray[0].Position.Y);
                                        currentArray[1].Position = new Vector2(currentArray[1].Position.X + 20, currentArray[1].Position.Y);
                                        currentArray[2].Position = new Vector2(currentArray[2].Position.X + 20, currentArray[2].Position.Y);
                                        currentArray[3].Position = new Vector2(currentArray[3].Position.X + 20, currentArray[3].Position.Y);
                                    }//if collisionArray
                                }//if !null
                            }//k
                        }//j
                    }//i

                    
                    currentArray[0].Position = new Vector2(currentArray[0].Position.X - 20, currentArray[0].Position.Y);
                    currentArray[1].Position = new Vector2(currentArray[1].Position.X - 20, currentArray[1].Position.Y);
                    currentArray[2].Position = new Vector2(currentArray[2].Position.X - 20, currentArray[2].Position.Y);
                    currentArray[3].Position = new Vector2(currentArray[3].Position.X - 20, currentArray[3].Position.Y);
                }
            }
            
            if(currentKBState.IsKeyDown(Keys.Right) && previousKBState.IsKeyUp(Keys.Right))
            {
                if (setup && !done)
                {
                    //collision for right border
                    for (int j = 0; j < 4; j++)
                    {
                        if (rightBorder[0].Position.X - 20 < currentArray[j].Position.X)
                        {
                            currentArray[0].Position = new Vector2(currentArray[0].Position.X - 20, currentArray[0].Position.Y);
                            currentArray[1].Position = new Vector2(currentArray[1].Position.X - 20, currentArray[1].Position.Y);
                            currentArray[2].Position = new Vector2(currentArray[2].Position.X - 20, currentArray[2].Position.Y);
                            currentArray[3].Position = new Vector2(currentArray[3].Position.X - 20, currentArray[3].Position.Y);
                        }
                    }

                    //collision in two dimmentional array
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 20; j++)
                        {
                            for (int k = 0; k < 16; k++)
                            {
                                if (collisionArray[j, k] != null)
                                {
                                    if (collisionArray[j, k].Position.Y < currentArray[i].Position.Y + 10 &&
                                        collisionArray[j, k].Position.Y + 20 > currentArray[i].Position.Y + 10 &&
                                        collisionArray[j, k].Position.X - 20 < currentArray[i].Position.X &&
                                        collisionArray[j, k].Position.X - 20 > currentArray[i].Position.X + 10)
                                    {
                                        currentArray[0].Position = new Vector2(currentArray[0].Position.X - 20, currentArray[0].Position.Y);
                                        currentArray[1].Position = new Vector2(currentArray[1].Position.X - 20, currentArray[1].Position.Y);
                                        currentArray[2].Position = new Vector2(currentArray[2].Position.X - 20, currentArray[2].Position.Y);
                                        currentArray[3].Position = new Vector2(currentArray[3].Position.X - 20, currentArray[3].Position.Y);
                                    }//if collisionArray
                                }//if !null
                            }//k
                        }//j
                    }//i
                    currentArray[0].Position = new Vector2(currentArray[0].Position.X + 20, currentArray[0].Position.Y);
                    currentArray[1].Position = new Vector2(currentArray[1].Position.X + 20, currentArray[1].Position.Y);
                    currentArray[2].Position = new Vector2(currentArray[2].Position.X + 20, currentArray[2].Position.Y);
                    currentArray[3].Position = new Vector2(currentArray[3].Position.X + 20, currentArray[3].Position.Y);
                }
            }

            if (currentKBState.IsKeyDown(Keys.Down))
                speed = 10;
            if (previousKBState.IsKeyUp(Keys.Down))
                speed = 2;
             
            if (currentKBState.IsKeyDown(Keys.Space) && previousKBState.IsKeyUp(Keys.Space))
            {
                //for square
                if (randomNumber == 1 && shape == 1)
                {
                    shape = 1;
                }
                //rotate the line shape
                else if (randomNumber == 2 && shape == 1)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X, currentArray[1].Position.Y - 20);
                    currentArray[3].Position = new Vector2(currentArray[1].Position.X, currentArray[2].Position.Y - 20);
                    
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
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X, currentArray[1].Position.Y - 20);
                    currentArray[3].Position = new Vector2(currentArray[1].Position.X - 20, currentArray[1].Position.Y);

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
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X, currentArray[1].Position.Y - 20);
                    currentArray[3].Position = new Vector2(currentArray[1].Position.X + 20, currentArray[1].Position.Y);

                    shape = 4;
                }
                else if (randomNumber == 3 && shape == 4)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X - 20, currentArray[0].Position.Y - 20);
                    currentArray[2].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);
                    currentArray[3].Position = new Vector2(currentArray[0].Position.X + 20, currentArray[0].Position.Y - 20);

                    shape = 1;
                }
                //rotate z shape
                else if (randomNumber == 4 && shape == 1)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X + 20, currentArray[1].Position.Y);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X, currentArray[2].Position.Y - 20);

                    shape = 2;
                }
                else if (randomNumber == 4 && shape == 2)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X + 20, currentArray[0].Position.Y);
                    currentArray[2].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X - 20, currentArray[2].Position.Y);

                    shape = 1;
                }
                //rotate s shape
                else if (randomNumber == 5 && shape == 1)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X - 20, currentArray[1].Position.Y);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X, currentArray[2].Position.Y - 20);

                    shape = 2;
                }
                else if (randomNumber == 5 && shape == 2)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X - 20, currentArray[0].Position.Y);
                    currentArray[2].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X + 20, currentArray[2].Position.Y);

                    shape = 1;
                }
                //rotate L shape
                else if (randomNumber == 6 && shape == 1)
                {
                    currentArray[0].Position = new Vector2(currentArray[0].Position.X - 20, currentArray[0].Position.Y);
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X + 20, currentArray[1].Position.Y);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X + 20, currentArray[2].Position.Y);

                    shape = 2;
                }
                else if (randomNumber == 6 && shape == 2)
                {
                    currentArray[0].Position = new Vector2(currentArray[0].Position.X + 20, currentArray[0].Position.Y);
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X, currentArray[1].Position.Y - 20);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X - 20, currentArray[2].Position.Y);

                    shape = 3;
                }
                else if (randomNumber == 6 && shape == 3)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X - 20, currentArray[0].Position.Y);
                    currentArray[2].Position = new Vector2(currentArray[0].Position.X + 20, currentArray[0].Position.Y);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X, currentArray[2].Position.Y - 20);

                    shape = 4;
                }
                else if (randomNumber == 6 && shape == 4)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X, currentArray[1].Position.Y - 20);
                    currentArray[3].Position = new Vector2(currentArray[0].Position.X + 20, currentArray[0].Position.Y);

                    shape = 1;
                }
                //rotate backward L shape
                else if (randomNumber == 7 && shape == 1)
                {
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X + 20, currentArray[0].Position.Y);
                    currentArray[2].Position = new Vector2(currentArray[0].Position.X - 20, currentArray[0].Position.Y);
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
                    currentArray[0].Position = new Vector2(currentArray[0].Position.X + 20, currentArray[0].Position.Y);
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X - 20, currentArray[1].Position.Y);
                    currentArray[3].Position = new Vector2(currentArray[2].Position.X - 20, currentArray[2].Position.Y);

                    shape = 4;
                }
                else if (randomNumber == 7 && shape == 4)
                {
                    currentArray[0].Position = new Vector2(currentArray[0].Position.X - 20, currentArray[0].Position.Y);
                    currentArray[1].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);
                    currentArray[2].Position = new Vector2(currentArray[1].Position.X, currentArray[1].Position.Y - 20);
                    currentArray[3].Position = new Vector2(currentArray[0].Position.X - 20, currentArray[0].Position.Y);

                    shape = 1;
                }

                //collision for left border
                for (int j = 0; j < 4; j++)
                {
                    if (leftBorder[0].Position.X + 20 > currentArray[j].Position.X)
                    {
                        currentArray[0].Position = new Vector2(currentArray[0].Position.X + 20, currentArray[0].Position.Y);
                        currentArray[1].Position = new Vector2(currentArray[1].Position.X + 20, currentArray[1].Position.Y);
                        currentArray[2].Position = new Vector2(currentArray[2].Position.X + 20, currentArray[2].Position.Y);
                        currentArray[3].Position = new Vector2(currentArray[3].Position.X + 20, currentArray[3].Position.Y);
                    }
                }

                //collision for right border
                for (int j = 0; j < 4; j++)
                {
                    if (rightBorder[0].Position.X < currentArray[j].Position.X)
                    {
                        currentArray[0].Position = new Vector2(currentArray[0].Position.X - 20, currentArray[0].Position.Y);
                        currentArray[1].Position = new Vector2(currentArray[1].Position.X - 20, currentArray[1].Position.Y);
                        currentArray[2].Position = new Vector2(currentArray[2].Position.X - 20, currentArray[2].Position.Y);
                        currentArray[3].Position = new Vector2(currentArray[3].Position.X - 20, currentArray[3].Position.Y);
                    }
                }
            }
            previousKBState = currentKBState;
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (!setup)
            {
                for (int i = 0; i < 40; i++)
                {
                    bottomBorder[i] = new Sprite(border.Texture);
                    bottomBorder[i].Position = new Vector2(i * 20, (ScreenManager.GraphicsDevice.Viewport.Height - 200));
                }
                for (int i = 0; i < 21; i++)
                {
                    leftBorder[i] = new Sprite(border.Texture);
                    rightBorder[i] = new Sprite(border.Texture);
                    leftBorder[i].Position = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width - 560), i * 20);
                    rightBorder[i].Position = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width - 240), i * 20);
                }
                setup = true;
            }

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


                //check if a line is formed then delete it
                checkArraySlot();

                //collisions
                for (int i = 0; i < 4; i++)
                {
                    //check blocks in 2 dimentional array first
                    if (currentArray[i].Position.Y > 0 && currentArray[i].Position.Y < 400)
                    {
                        twoDimentionalArrayCollision();
                            i = 4;
                    }
                    //check bottom border
                    else if (currentArray[i].Position.Y + 10 > bottomBorder[0].Position.Y)
                    {
                        BottomBorderCollision();
                        i = 4;
                    }
                }
            }

            if (collide)
            {
                //resets variable
                count = 0;
                done = true;
                shape = 1;
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
                yellowB[2].Position = new Vector2(yellowB[0].Position.X, yellowB[0].Position.Y - 20);
                yellowB[3].Position = new Vector2(yellowB[0].Position.X + 20, yellowB[0].Position.Y - 20);

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
                greyB[1].Position = new Vector2(greyB[0].Position.X - 20, greyB[0].Position.Y - 20);
                greyB[2].Position = new Vector2(greyB[0].Position.X, greyB[0].Position.Y - 20);
                greyB[3].Position = new Vector2(greyB[0].Position.X + 20, greyB[0].Position.Y -20);

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
                redB[1].Position = new Vector2(redB[0].Position.X + 20, redB[0].Position.Y);
                redB[2].Position = new Vector2(redB[0].Position.X, redB[0].Position.Y - 20);
                redB[3].Position = new Vector2(redB[2].Position.X - 20, redB[2].Position.Y);

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
                blueB[1].Position = new Vector2(blueB[0].Position.X - 20, blueB[0].Position.Y);
                blueB[2].Position = new Vector2(blueB[0].Position.X, blueB[0].Position.Y - 20);
                blueB[3].Position = new Vector2(blueB[2].Position.X + 20, blueB[2].Position.Y);

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
                greenB[1].Position = new Vector2(greenB[0].Position.X, greenB[0].Position.Y - 20);
                greenB[2].Position = new Vector2(greenB[0].Position.X, greenB[1].Position.Y - 20);
                greenB[3].Position = new Vector2(greenB[0].Position.X + 20, greenB[0].Position.Y);

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
                purpleB[1].Position = new Vector2(purpleB[0].Position.X, purpleB[0].Position.Y - 20);
                purpleB[2].Position = new Vector2(purpleB[0].Position.X, purpleB[1].Position.Y - 20);
                purpleB[3].Position = new Vector2(purpleB[0].Position.X - 20, purpleB[0].Position.Y);

                for (int i = 0; i < 4; i++)
                {
                    currentArray[i] = purpleB[i];
                }
            }


        }

        private void twoDimentionalArrayCollision()
        {
            //local variable
            int tempX = 0;
            int tempY = 0;
            int posX = 0;
            int posY = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    for (int k = 0; k < 16; k++)
                    {
                        if (collisionArray[j, k] != null)
                        {
                            if (collisionArray[j, k].Position.Y < currentArray[i].Position.Y + 10 &&
                                collisionArray[j, k].Position.Y + 20 > currentArray[i].Position.Y + 10 &&
                                collisionArray[j, k].Position.X < currentArray[i].Position.X + 10 &&
                                collisionArray[j, k].Position.X + 20 > currentArray[i].Position.X + 10)
                            {
                                currentArray[0].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);
                                currentArray[1].Position = new Vector2(currentArray[1].Position.X, currentArray[1].Position.Y - 20);
                                currentArray[2].Position = new Vector2(currentArray[2].Position.X, currentArray[2].Position.Y - 20);
                                currentArray[3].Position = new Vector2(currentArray[3].Position.X, currentArray[3].Position.Y - 20);

                                for (int h = 0; h < 4; h++)
                                {
                                    tempX = (int)currentArray[h].Position.X;
                                    tempY = (int)currentArray[h].Position.Y;

                                    if (tempX > 260 && tempX < 280)
                                        posX = 0;
                                    else if (tempX > 280 && tempX < 300)
                                        posX = 1;
                                    else if (tempX > 300 && tempX < 320)
                                        posX = 2;
                                    else if (tempX > 320 && tempX < 340)
                                        posX = 3;
                                    else if (tempX > 340 && tempX < 360)
                                        posX = 4;
                                    else if (tempX > 360 && tempX < 380)
                                        posX = 5;
                                    else if (tempX > 380 && tempX < 400)
                                        posX = 6;
                                    else if (tempX > 400 && tempX < 420)
                                        posX = 7;
                                    else if (tempX > 420 && tempX < 440)
                                        posX = 8;
                                    else if (tempX > 440 && tempX < 460)
                                        posX = 9;
                                    else if (tempX > 460 && tempX < 480)
                                        posX = 10;
                                    else if (tempX > 480 && tempX < 500)
                                        posX = 11;
                                    else if (tempX > 500 && tempX < 520)
                                        posX = 12;
                                    else if (tempX > 520 && tempX < 540)
                                        posX = 13;
                                    else if (tempX > 540 && tempX < 560)
                                        posX = 14;
                                    else if (tempX > 560 && tempX < 580)
                                        posX = 15;

                                    if (tempY > 380)
                                        posY = 0;
                                    else if (tempY > 360)
                                        posY = 1;
                                    else if (tempY > 340)
                                        posY = 2;
                                    else if (tempY > 320)
                                        posY = 3;
                                    else if (tempY > 300)
                                        posY = 4;
                                    else if (tempY > 280)
                                        posY = 5;
                                    else if (tempY > 260)
                                        posY = 6;
                                    else if (tempY > 240)
                                        posY = 7;
                                    else if (tempY > 220)
                                        posY = 8;
                                    else if (tempY > 200)
                                        posY = 9;
                                    else if (tempY > 180)
                                        posY = 10;
                                    else if (tempY > 160)
                                        posY = 11;
                                    else if (tempY > 140)
                                        posY = 12;
                                    else if (tempY > 120)
                                        posY = 13;
                                    else if (tempY > 100)
                                        posY = 14;
                                    else if (tempY > 80)
                                        posY = 15;
                                    else if (tempY > 60)
                                        posY = 16;
                                    else if (tempY > 40)
                                        posY = 17;
                                    else if (tempY > 20)
                                        posY = 18;
                                    else if (tempY > 0)
                                        posY = 19;

                                    collisionArray[posY, posX] = currentArray[h];
                                    
                                    //exit out of loop
                                    if (h > 2)
                                    {
                                        k = 16;
                                        j = 20;
                                        i = 4;
                                        collide = true;
                                    }
                                }//h
                            }//if collisionArray
                        }//if !null
                    }//k

                }//j

            }//i
        }

        private void BottomBorderCollision()
        {
            //local variable
            int tempX = 0;
            int tempY = 0;
            int posX = 0;
            int posY = 0;

            for (int i = 0, counting = 0; i < 4; i++)
            {
                if (currentArray[i].Position.Y > bottomBorder[0].Position.Y)
                {
                    currentArray[0].Position = new Vector2(currentArray[0].Position.X, currentArray[0].Position.Y - 20);
                    currentArray[1].Position = new Vector2(currentArray[1].Position.X, currentArray[1].Position.Y - 20);
                    currentArray[2].Position = new Vector2(currentArray[2].Position.X, currentArray[2].Position.Y - 20);
                    currentArray[3].Position = new Vector2(currentArray[3].Position.X, currentArray[3].Position.Y - 20);

                    for (int j = 0; j < 4; j++)
                    {
                        tempX = (int)currentArray[j].Position.X;
                        tempY = (int)currentArray[j].Position.Y;

                        if (tempX > 260 && tempX < 280)
                            posX = 0;
                        else if (tempX > 280 && tempX < 300)
                            posX = 1;
                        else if (tempX > 300 && tempX < 320)
                            posX = 2;
                        else if (tempX > 320 && tempX < 340)
                            posX = 3;
                        else if (tempX > 340 && tempX < 360)
                            posX = 4;
                        else if (tempX > 360 && tempX < 380)
                            posX = 5;
                        else if (tempX > 380 && tempX < 400)
                            posX = 6;
                        else if (tempX > 400 && tempX < 420)
                            posX = 7;
                        else if (tempX > 420 && tempX < 440)
                            posX = 8;
                        else if (tempX > 440 && tempX < 460)
                            posX = 9;
                        else if (tempX > 460 && tempX < 480)
                            posX = 10;
                        else if (tempX > 480 && tempX < 500)
                            posX = 11;
                        else if (tempX > 500 && tempX < 520)
                            posX = 12;
                        else if (tempX > 520 && tempX < 540)
                            posX = 13;
                        else if (tempX > 540 && tempX < 560)
                            posX = 14;
                        else if (tempX > 560 && tempX < 580)
                            posX = 15;

                        if (tempY > 380)
                            posY = 0;
                        else if (tempY > 360)
                            posY = 1;
                        else if (tempY > 340)
                            posY = 2;
                        else if (tempY > 320)
                            posY = 3;
                        else if (tempY > 300)
                            posY = 4;
                        else if (tempY > 280)
                            posY = 5;
                        else if (tempY > 260)
                            posY = 6;
                        else if (tempY > 240)
                            posY = 7;
                        else if (tempY > 220)
                            posY = 8;
                        else if (tempY > 200)
                            posY = 9;
                        else if (tempY > 180)
                            posY = 10;
                        else if (tempY > 160)
                            posY = 11;
                        else if (tempY > 140)
                            posY = 12;
                        else if (tempY > 120)
                            posY = 13;
                        else if (tempY > 100)
                            posY = 14;
                        else if (tempY > 80)
                            posY = 15;
                        else if (tempY > 60)
                            posY = 16;
                        else if (tempY > 40)
                            posY = 17;
                        else if (tempY > 20)
                            posY = 18;
                        else if (tempY > 0)
                            posY = 19;

                        collisionArray[posY, posX] = currentArray[j];
                    }

                    counting = 3;
                    i = 4;
                }

                if (counting > 2)
                    collide = true;
            }
        }

        private void checkArraySlot()
        {
            int filled = 0;
            int temp = 0;
            bool clear = false;

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (collisionArray[i, j] != null && !clear)
                        filled++;

                    if (filled == 15 && !clear)
                    {
                        for (int k = 0; k < 15; k++)
                        {
                            collisionArray[i,k] = null;
                            temp = i;
                            clear = true;
                            j = 0;
                        }
                        score += 50;
                    }

                    if (clear && collisionArray[i,j] != null)
                    {
                        collisionArray[i,j].Position = new Vector2(collisionArray[i,j].Position.X, collisionArray[i,j].Position.Y + 20);
                    }
                }
                filled = 0;
            }

            if (clear)
            {
                for (int k = temp; k < 20; k++)
                {
                    for (int l = 0; l < 15; l++)
                    {
                        if (collisionArray[k, l] != null)
                        {
                            collisionArray[k - 1, l] = collisionArray[k, l];
                            collisionArray[k, l] = null;
                        }
                    }
                }
                clear = false;
            }
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "TETRIS", new Vector2(370, 500), Color.White);
            spriteBatch.DrawString(font, "Score: " + score.ToString(), new Vector2(10, 10), Color.Yellow);

            for (int h = 0; h < 16; h++)
            {
                for (int n = 0; n < 20; n++)
                {
                    if (collisionArray[n, h] != null)
                        spriteBatch.Draw(collisionArray[n, h].Texture,
                            collisionArray[n, h].Position,
                            null,
                            Color.White,
                            collisionArray[n, h].Rotation,
                            collisionArray[n, h].Center,
                            collisionArray[n, h].Scale / 2,
                            SpriteEffects.None,
                            1.0f);
                }
            }

            if (setup)
            {
                for (int i = 0; i < 21; i++)
                {
                    spriteBatch.Draw(leftBorder[i].Texture,
                        leftBorder[i].Position,
                        null,
                        Color.White,
                        leftBorder[i].Rotation,
                        leftBorder[i].Center,
                        leftBorder[i].Scale * 20,
                        SpriteEffects.None,
                        1.0f);
                    spriteBatch.Draw(rightBorder[i].Texture,
                        rightBorder[i].Position,
                        null,
                        Color.White,
                        rightBorder[i].Rotation,
                        rightBorder[i].Center,
                        rightBorder[i].Scale * 20,
                        SpriteEffects.None,
                        1.0f);
                }
                for (int i = 0; i < 40; i++)
                {
                    spriteBatch.Draw(bottomBorder[i].Texture,
                        bottomBorder[i].Position,
                        null,
                        Color.White,
                        bottomBorder[i].Rotation,
                        bottomBorder[i].Center,
                        bottomBorder[i].Scale * 20,
                        SpriteEffects.None,
                        1.0f);
                }
            }

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
