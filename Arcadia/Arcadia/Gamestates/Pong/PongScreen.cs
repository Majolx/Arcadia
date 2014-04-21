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

        Viewport vp;
        int speed = 5;

        MobileSprite ball;
        Vector2 v2BallDirection = new Vector2(5, 5);

        Vector2[] v2Player = new Vector2[2];
        Rectangle[] rPlayer = new Rectangle[2];
        int[] score = { 0, 0 };


        int bound = 5;

        int paddleHeight = 75;
        int paddleWidth = 10;

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
            vp = ScreenManager.Game.GraphicsDevice.Viewport;

            // Set up ball
            ball = new MobileSprite(t2dBall);
            ball.Sprite.AddAnimation("ball", 0, 0, 16, 16, 1, 1f);
            ball.Position = new Vector2(0, 75);
            ball.IsPathing = false;

            ball.Sprite.CurrentAnimation = "ball";

            // Set up paddles
            v2Player[0] = new Vector2(3*bound, vp.Height / 2 - (paddleHeight / 2));

            v2Player[1] = new Vector2(vp.Width - 3*bound - paddleWidth, 
                                      vp.Height / 2 - (paddleHeight / 2));

            rPlayer[0] = new Rectangle((int)v2Player[0].X, (int)v2Player[0].Y, 
                                        paddleWidth, paddleHeight);

            rPlayer[1] = new Rectangle((int)v2Player[1].X, (int)v2Player[1].Y, 
                                        paddleWidth, paddleHeight);


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

            if (ball.Position.Y + ball.Sprite.Texture.Height > rArena[2].Y ||
                ball.Position.Y < rArena[0].Y + rArena[0].Height)
            {
                v2BallDirection *= new Vector2(1, -1);
            }

            if (ball.CollisionBox.Left < rPlayer[0].X + rPlayer[0].Width &&
                ball.CollisionBox.Bottom > rPlayer[0].Y &&
                ball.CollisionBox.Top < rPlayer[0].Y + rPlayer[0].Height)
            {
                v2BallDirection *= new Vector2(-1, 1);
            }

            if (ball.CollisionBox.Right > ScreenManager.Game.GraphicsDevice.Viewport.Width)
            {
                v2BallDirection *= new Vector2(-1, 1);
            }

            ball.Update(gameTime);
            
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
        public override void HandleInput(InputState input)
        {
            if (input.IsKeyDown(Keys.S) && v2Player[0].Y < v2Arena[2].Y - rPlayer[0].Height)
                v2Player[0].Y += speed;

            if (input.IsKeyDown(Keys.W) && v2Player[0].Y > v2Arena[0].Y + rArena[0].Height)
                v2Player[0].Y -= speed;

            UpdateRectangles();

            base.HandleInput(input);
        }

        public void UpdateRectangles()
        {
            for (int i = 0; i < rPlayer.Length; i++)
            {
                rPlayer[i].Y = (int)v2Player[i].Y;
            }
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
            sb.Draw(whiteRectangle, rPlayer[0], Color.White);
            sb.Draw(whiteRectangle, rPlayer[1], Color.White);

            sb.End();

            base.Draw(gameTime);
        }


        #endregion
    }
}
