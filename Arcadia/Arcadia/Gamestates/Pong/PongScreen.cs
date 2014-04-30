using System;
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

        int speed = 5;

        Ball ball;
        Vector2 v2BallDirection = new Vector2(5, 5);

        Vector2[] v2Player = new Vector2[2];
        Paddle[] paddles = new Paddle[2];
        Color[] paddleColors = new Color[2];
        Vector2[] v2StartingPos = { new Vector2(15, 200),
                                    new Vector2(645, 200) };

        int[] score = { 0, 0 };


        int bound = 5;

        int paddleHeight = 75;
        int paddleWidth = 10;
        Color p1Color = Color.Green;
        Color p2Color = Color.Red;

        Vector2[] v2Arena = new Vector2[3];
        Rectangle[] rArena = new Rectangle[3];
        Texture2D whiteRectangle;
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

            // Set up paddles
            paddles[0] = new Paddle( );//new Rectangle(0, 0, paddleWidth, paddleHeight));
            paddles[0].Position = new Vector2(3 * bound, vp.Height / 2 - (paddleHeight / 2));
            paddles[0].OuterColor = Color.LimeGreen;
            paddles[0].InnerColor = Color.Black;
            paddles[0].FinalizeTexture(gd);

            paddles[1] = new Paddle( );//new Rectangle(0, 0, paddleWidth, paddleHeight));
            paddles[1].Position = new Vector2(vp.Width - 3*bound - paddleWidth, vp.Height / 2 - (paddleHeight / 2));
            paddles[1].OuterColor = Color.Blue;
            paddles[1].InnerColor = Color.Black;
            paddles[1].FinalizeTexture(gd);


            // Set up arena
            v2Arena[0] = new Vector2(0, 2*bound);
            v2Arena[1] = new Vector2(vp.Width / 2 - bound / 2, v2Arena[0].Y);
            v2Arena[2] = new Vector2(0, vp.Height - 3*bound);

            rArena[0] = new Rectangle((int)v2Arena[0].X, (int)v2Arena[0].Y, vp.Width, bound);
            rArena[1] = new Rectangle((int)v2Arena[1].X, (int)v2Arena[1].Y, bound, (int)v2Arena[2].Y - (int)v2Arena[1].Y);
            rArena[2] = new Rectangle((int)v2Arena[2].X, (int)v2Arena[2].Y, vp.Width, bound);

            // Set white texture
            whiteRectangle = new Texture2D(ScreenManager.Game.GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
        }


        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            ball.Position += v2BallDirection;

            // Hits the floor or celing
            if (ball.Position.Y + ball.Texture.Height > rArena[2].Y ||
                ball.Position.Y < rArena[0].Y + rArena[0].Height)
            {
                v2BallDirection *= new Vector2(1, -1);
            }

            // Hits the left paddle
            if (ball.CollisionBox.Left <= paddles[0].CollisionBox.Right &&
                ball.CollisionBox.Left >= paddles[0].CollisionBox.Right- 4*ball.Speed &&
                ball.CollisionBox.Bottom > paddles[0].CollisionBox.Top &&
                ball.CollisionBox.Top < paddles[0].CollisionBox.Bottom &&
                v2BallDirection.X < 0)
            {
                v2BallDirection *= new Vector2(-1, 1);
            }

            // Hits the right side
            if (ball.CollisionBox.Right > ScreenManager.Game.GraphicsDevice.Viewport.Width &&
                v2BallDirection.X > 0)
            {
                v2BallDirection *= new Vector2(-1, 1);
            }

            ball.Update();

            for (int i = 0; i < paddles.Length; i++)
            {
                paddles[i].Update();
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }


        public override void HandleInput(InputState input)
        {
            if (input.IsKeyDown(Keys.S) && paddles[0].Position.Y < v2Arena[2].Y - paddles[0].CollisionBox.Height)
                paddles[0].Position = new Vector2(paddles[0].Position.X, paddles[0].Position.Y + speed);

            if (input.IsKeyDown(Keys.W) && paddles[0].Position.Y > v2Arena[0].Y + rArena[0].Height)
                paddles[0].Position = new Vector2(paddles[0].Position.X, paddles[0].Position.Y - speed);

            base.HandleInput(input);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = ScreenManager.SpriteBatch;

            sb.Begin();

            // Draw the arena
            for (int i = 0; i < v2Arena.Length; i++)
                sb.Draw(whiteRectangle, rArena[i], Color.White);

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
