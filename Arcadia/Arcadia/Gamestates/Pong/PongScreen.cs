using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Arcadia;
using Arcadia.Graphics;
using Arcadia.Screen;


namespace Arcadia.Gamestates.Pong
{
    class PongScreen : GameScreen
    {
        #region Fields

        Ball ball;

        // Game settings
        int paddleSpeed = 4;
        int ballSpeed = 8;

        
        Paddle[] paddles = new Paddle[2];
        Color[] paddleColors = new Color[2];
        Vector2 v2StartingBallPos;

        DrawableScore score;

        int bound = 5;

        Color p1Color = Color.Green;
        Color p2Color = Color.Red;

        Arena arena;

        Texture2D t2dBall;
        
        SpriteFont scoreFontOne;
        SpriteFont scoreFontTwo;
        SpriteFont TextFont;

        SoundEffect beep;   // For wall collisions
        SoundEffect boop;   // For paddle collisions
        SoundEffect brrr;   // Score buzzer

        public ContentManager content;

        #endregion

        #region Initialization


        public PongScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager( ScreenManager.Game.Services, "Content" );

            string ContentLoadDir = "Sprite/Pong/";

            t2dBall = content.Load<Texture2D>( ContentLoadDir + "pongball" );
            scoreFontOne = content.Load<SpriteFont>("Font/PongScoreGreen");
            scoreFontTwo = content.Load<SpriteFont>("Font/PongScoreBlue");
            TextFont = ScreenManager.ArcadeFont;
            Initialize();

            base.LoadContent();
        }

        public void Initialize()
        {
            GraphicsDevice gd = ScreenManager.Game.GraphicsDevice;
            Viewport vp = gd.Viewport;

            // Set up ball
            ball = new Ball(t2dBall);
            ball.Position = new Vector2(vp.Width / 2 - ball.Texture.Width / 2,
                                        vp.Height / 2 - ball.Texture.Height / 2);
            ball.CollisionBox = new Rectangle((int)ball.Position.X, 
                                              (int)ball.Position.Y, 
                                              (int)ball.Texture.Width, 
                                              (int)ball.Texture.Height);
            ball.Speed = ballSpeed;

            // Save the state of the ball's starting position for respawn
            v2StartingBallPos = ball.Position;

            // Set up paddles
            paddles[0] = new Paddle( );
            paddles[0].Position = new Vector2(3 * bound, vp.Height / 2 - (paddles[0].CollisionBox.Height / 2));
            paddles[0].Speed = paddleSpeed;
            paddles[0].OuterColor = Color.LimeGreen;
            paddles[0].InnerColor = Color.Black;
            paddles[0].FinalizeTexture(gd);

            paddles[1] = new Paddle( );
            paddles[1].Position = new Vector2(vp.Width - 3*bound - paddles[1].CollisionBox.Width, vp.Height / 2 - (paddles[1].CollisionBox.Height / 2));
            paddles[1].Speed = paddleSpeed;
            paddles[1].OuterColor = Color.Blue;
            paddles[1].InnerColor = Color.Black;
            paddles[1].FinalizeTexture(gd);
            paddles[1].AiEnabled = true;


            // Set up arena
            ArenaComponent[] components = new ArenaComponent[3];

            components[0] = new ArenaComponent();
            components[0].IsCollidable = true;
            components[0].Position = new Vector2(0, 15);
            components[0].CollisionBox = new Rectangle(0, 15, vp.Width, 35);
            components[0].Color = Color.White;
            components[0].FinalizeTexture(gd);

            components[1] = new ArenaComponent();
            components[1].IsCollidable = true;
            components[1].CollisionBox = new Rectangle(0, vp.Height - 70, vp.Width, 35);
            components[1].Position = new Vector2(0, components[1].CollisionBox.Y);
            components[1].Color = Color.White;
            components[1].FinalizeTexture(gd);

            components[2] = new ArenaComponent();
            components[2].IsDotted = true;
            components[2].CollisionBox = new Rectangle(vp.Width / 2 -10, components[0].CollisionBox.Bottom, 20,
                                                       components[1].CollisionBox.Top - components[0].CollisionBox.Bottom);
            components[2].Position = new Vector2(components[2].CollisionBox.X, components[2].CollisionBox.Y);
            components[2].Color = Color.LightGray;
            components[2].FinalizeTexture(gd);

            arena = new Arena(components);

            // Set up score
            score = new DrawableScore(2);
            score.Positions[0] = new Vector2(vp.Width / 2 - scoreFontOne.MeasureString(score.Scores[0].ToString()).X - 15, 50); ;
            score.Positions[1] = new Vector2(vp.Width / 2 + 15, 50);
            score.Fonts[0] = scoreFontOne;
            score.Fonts[1] = scoreFontTwo;

            // Initialize audio
            beep = content.Load<SoundEffect>("Sounds/beep");
            boop = content.Load<SoundEffect>("Sounds/boop");
            brrr = content.Load<SoundEffect>("Sounds/brrr");
            
            
        }


        #endregion

        public void ResetLevel()
        {
            
            ball.ReverseDirection();
            ball.Position = v2StartingBallPos;
        }

        #region Update and Draw


        public override void HandleInput(InputState input)
        {
            // Paddle 1 Controls
            if (input.IsKeyDown(Keys.S) && paddles[0].CollisionBox.Bottom < arena.Components[1].CollisionBox.Top)
                paddles[0].MoveDown();

            if (input.IsKeyDown(Keys.W) && paddles[0].CollisionBox.Top > arena.Components[0].CollisionBox.Bottom)
                paddles[0].MoveUp();

            // Paddle 2 Controls
            if (paddles[1].AiEnabled)
            {
                if (paddles[1].CollisionBox.Center.Y > ball.CollisionBox.Center.Y + 20 &&
                    paddles[1].CollisionBox.Top >= arena.Components[0].CollisionBox.Bottom)
                {
                    paddles[1].MoveUp();
                }
                else if (paddles[1].CollisionBox.Center.Y < ball.CollisionBox.Center.Y - 20 &&
                    paddles[1].CollisionBox.Bottom <= arena.Components[1].CollisionBox.Top)
                {
                    paddles[1].MoveDown();
                }
            }
            else
            {
                if (input.IsKeyDown(Keys.Down) && paddles[1].CollisionBox.Bottom < arena.Components[1].CollisionBox.Top)
                    paddles[1].MoveDown();

                if (input.IsKeyDown(Keys.Up) && paddles[1].CollisionBox.Top > arena.Components[0].CollisionBox.Bottom)
                    paddles[1].MoveUp();
            }

            // Pause game
            PlayerIndex playerIndex;
            if (input.IsNewKeyPress(Keys.Escape, null, out playerIndex))
            {
                ScreenManager.AddScreen(new PauseScreen("PAUSE"), null);
            }

            // Toggle AI
            if (input.IsToggleAISelect(null))
            {
                switch (paddles[1].AiEnabled)
                {
                    case false:
                        paddles[1].AiEnabled = true;
                        break;
                    case true:
                        paddles[1].AiEnabled = false;
                        break;
                }
            }
            base.HandleInput(input);
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            // Hits the celing
            if (ball.CollisionBox.Intersects(arena.Components[0].CollisionBox) &&
                ball.Direction > Math.PI)
            {
                beep.Play();
                ball.HitWall();
            }
            
            // Hits the floor
            else if (ball.CollisionBox.Intersects(arena.Components[1].CollisionBox) &&
                ball.Direction < Math.PI)
            {
                beep.Play();
                ball.HitWall();
            }

            // Hits the left paddle
            else if (ball.CollisionBox.Intersects(paddles[0].CollisionBox) &&
                ball.CollisionBox.Center.X > paddles[0].CollisionBox.Center.X &&
                ball.Direction > MathHelper.PiOver2 &&
                ball.Direction < 3*MathHelper.PiOver2)
            {
                boop.Play(1.0f, 0.0f, -1.0f);
                float x = ball.CollisionBox.Bottom - paddles[0].Position.Y;
                if (x < 0) x = 0;
                if (x > paddles[0].CollisionBox.Height) x = paddles[0].CollisionBox.Height;
                x = ball.NormalHitValue(x);
                x = -x;
                x = (float)Math.Acos(x);
                x += 3*(float)MathHelper.PiOver2;
                x %= (float)MathHelper.TwoPi;
                ball.Direction = x;
            }

            // Hits the right paddle
            else if (ball.CollisionBox.Intersects(paddles[1].CollisionBox) &&
                ball.CollisionBox.Center.X < paddles[1].CollisionBox.Center.X &&
                (ball.Direction < Math.PI ||
                 ball.Direction > 3*MathHelper.PiOver2))
            {
                boop.Play(1.0f, 0.0f, 1.0f);
                float x = ball.CollisionBox.Bottom - paddles[1].Position.Y;
                x = ball.NormalHitValue(x);
                x = (float)Math.Acos(x);
                x += (float)MathHelper.PiOver2;
                x %= (float)MathHelper.TwoPi;
                ball.Direction = x;
            }

            Viewport vp = ScreenManager.GraphicsDevice.Viewport;

            // Hits the left side
            if (ball.CollisionBox.Left < 0)
            {
                brrr.Play(0.5f, 0.0f, -1.0f);
                score.AddScore(1);
                score.Positions[1] = new Vector2(vp.Width / 2 + 15, 50);
                ResetLevel();
            }

            // Hits the right side
            if (ball.CollisionBox.Right > ScreenManager.Game.GraphicsDevice.Viewport.Width)
            {
                brrr.Play(0.5f, 0.0f, 1.0f);
                score.AddScore(0);
                score.Positions[0] = new Vector2(vp.Width / 2 - scoreFontOne.MeasureString(score.Scores[0].ToString()).X - 15, 50);
                ResetLevel();
            }

            // Update Paddles
            // Make sure paddles don't overlap arena
            foreach (Paddle paddle in paddles)
            {
                if (paddle.CollisionBox.Top < arena.Components[0].CollisionBox.Bottom)
                {
                    paddle.Position += new Vector2(0, arena.Components[0].CollisionBox.Bottom - paddle.CollisionBox.Top);
                }
                if (paddle.CollisionBox.Bottom > arena.Components[1].CollisionBox.Top)
                {
                    paddle.Position -= new Vector2(0, paddle.CollisionBox.Bottom - arena.Components[1].CollisionBox.Top);
                }
            }

            for (int i = 0; i < paddles.Length; i++)
            {
                paddles[i].Update();
            }

            // Update ball
            ball.Update(gameTime);


            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = ScreenManager.SpriteBatch;

            sb.Begin();

            // Draw the arena
            arena.Draw(sb);
            
            // Draw the P prompt
            sb.DrawString(TextFont, "Press P to play", new Vector2(600, 25), Color.Black);

            // Draw the score
            score.Draw(sb);

            // Draw the ball
            ball.Draw(sb);

            // Draw the paddles
            foreach (Paddle paddle in paddles)
            {
                paddle.Draw(sb);
            }

            sb.End();

            base.Draw(gameTime);
        }


        #endregion
    }
}
