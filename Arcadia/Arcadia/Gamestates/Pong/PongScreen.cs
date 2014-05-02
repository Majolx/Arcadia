using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arcadia;
using Arcadia.Graphics;
using Arcadia.Screen;


namespace Arcadia.Gamestates.Pong
{
    class PongScreen : GameScreen
    {
        #region Fields

        Ball ball;

        int paddleSpeed = 5;
        Vector2[] v2Player = new Vector2[2];
        Paddle[] paddles = new Paddle[2];
        Color[] paddleColors = new Color[2];
        Vector2 v2StartingBallPos;

        int[] score = { 0, 0 };


        int bound = 5;

        Color p1Color = Color.Green;
        Color p2Color = Color.Red;

        Arena arena;

        Texture2D t2dBall;
        
        SpriteFont font;

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
            font = content.Load<SpriteFont>("Font/gamefont");
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

            // Save the state of the ball's starting position for respawn
            v2StartingBallPos = ball.Position;

            // Set up paddles
            paddles[0] = new Paddle( );
            paddles[0].Position = new Vector2(3 * bound, vp.Height / 2 - (paddles[0].CollisionBox.Height / 2));
            paddles[0].OuterColor = Color.LimeGreen;
            paddles[0].InnerColor = Color.Black;
            paddles[0].FinalizeTexture(gd);

            paddles[1] = new Paddle( );
            paddles[1].Position = new Vector2(vp.Width - 3*bound - paddles[1].CollisionBox.Width, vp.Height / 2 - (paddles[1].CollisionBox.Height / 2));
            paddles[1].OuterColor = Color.Blue;
            paddles[1].InnerColor = Color.Black;
            paddles[1].FinalizeTexture(gd);


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
            components[2].CollisionBox = new Rectangle(vp.Width / 2 - 20, components[0].CollisionBox.Bottom, 40,
                                                       components[1].CollisionBox.Top - components[0].CollisionBox.Bottom);
            components[2].Position = new Vector2(components[2].CollisionBox.X, components[2].CollisionBox.Y);
            components[2].Color = Color.LightGray;
            components[2].FinalizeTexture(gd);

            arena = new Arena(components);
        }


        #endregion

        public void ResetLevel()
        {
            ball.ReverseDirection();
            ball.Position = v2StartingBallPos;
        }

        #region Update and Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            double pi = Math.PI;

            if (ball.CollisionBox.Intersects(arena.Components[0].CollisionBox) )
            { 
                pi = Math.PI;
            }

            // Hits the celing
            if (ball.CollisionBox.Intersects(arena.Components[0].CollisionBox) &&
                ball.Direction > pi)
            {
                ball.Bounce();
            }
            
            // Hits the floor
            else if (ball.CollisionBox.Intersects(arena.Components[1].CollisionBox) &&
                ball.Direction < pi)
            {
                ball.Bounce();
            }

            // Hits the left paddle
            else if (ball.CollisionBox.Intersects(paddles[0].CollisionBox) &&
                ball.Direction > pi/2 &&
                ball.Direction < 3*pi/2)
            {
                ball.Direction += (float)pi;
                ball.Bounce();
            }

            // Hits the right paddle
            else if (ball.CollisionBox.Intersects(paddles[1].CollisionBox) &&
                (ball.Direction < pi ||
                 ball.Direction > 3*pi/2))
            {
                ball.Direction += (float)pi;
                ball.Bounce();
            }

            // Hits the left side
            if (ball.CollisionBox.Left < 0)
            {
                score[1]++;
                ResetLevel();
            }

            // Hits the right side
            if (ball.CollisionBox.Right > ScreenManager.Game.GraphicsDevice.Viewport.Width)
            {
                score[0]++;
                ResetLevel();
            }

            ball.Update(gameTime);

            for (int i = 0; i < paddles.Length; i++)
            {
                paddles[i].Update();
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }


        public override void HandleInput(InputState input)
        {
            if (input.IsKeyDown(Keys.S) && paddles[0].CollisionBox.Bottom < arena.Components[1].CollisionBox.Top)
                paddles[0].Position = new Vector2(paddles[0].Position.X, paddles[0].Position.Y + paddleSpeed);

            if (input.IsKeyDown(Keys.W) && paddles[0].CollisionBox.Top > arena.Components[0].CollisionBox.Bottom)
                paddles[0].Position = new Vector2(paddles[0].Position.X, paddles[0].Position.Y - paddleSpeed);

            if (input.IsKeyDown(Keys.Down) && paddles[1].CollisionBox.Bottom < arena.Components[1].CollisionBox.Top)
                paddles[1].Position = new Vector2(paddles[1].Position.X, paddles[1].Position.Y + paddleSpeed);

            if (input.IsKeyDown(Keys.Up) && paddles[1].CollisionBox.Top > arena.Components[0].CollisionBox.Bottom)
                paddles[1].Position = new Vector2(paddles[1].Position.X, paddles[1].Position.Y - paddleSpeed);

            base.HandleInput(input);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = ScreenManager.SpriteBatch;

            sb.Begin();

            // Draw the arena
            arena.Draw(sb);

            // Draw the score (in a very hacky way, fix me!! fiiiiix meeee.......)
            int x = 0;
            foreach (int s in score)
            {
                x += 255;
                
                sb.DrawString(font, s.ToString(), new Vector2(x, 50), Color.White);
            }

            sb.DrawString(font, ball.Direction.ToString(), new Vector2(200, 200), Color.Green);
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
