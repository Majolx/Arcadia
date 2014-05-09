using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Arcadia.Graphics;

namespace Arcadia.Gamestates.Pong
{
    class Ball : StaticSprite
    {
        private float fSpeed;
        private float fDirection;



        public float Speed
        {
            get { return fSpeed; }
            set { fSpeed = value; }
        }

        public float Direction
        {
            get { return fDirection; }
            set 
            { 
                fDirection = value;
                fDirection %= 2*(float)Math.PI;
            }
        }

        public Ball(Texture2D texture)
        {
            Texture = texture;
            CollisionBox = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Position = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
            Direction = 3*(float)Math.PI/4;
            Speed = 5f;
        }

        public void HitPaddle()
        {
            Direction += (float)Math.PI;


            // Normalize space on -1 to 1
            // B
            bounce();
        }

        public float NormalHitValue(float x)
        {
            float y;
            float A = 0f;  
            float B = 80f; // Height of paddle plus height of ball
            float C = -1f; // Lower bound
            float D = 1f;  // Upper bound

            y = C + (x - A) * (D - C) / (B - A);

            if (y < -.95) y = -.95f;
            if (y > .95) y = .95f;

            return y;
        }


        public void HitWall()
        {
            bounce();
        }

        private void bounce() 
        {
            Direction = 2*(float)Math.PI - fDirection;
        }

        public void ReverseDirection()
        {
            Direction += (float)Math.PI;
        }

        public void Update(GameTime gameTime)
        {
            float posX = Position.X + fSpeed * ((float)Math.Cos(fDirection));
            float posY = Position.Y + fSpeed * ((float)Math.Sin(fDirection));
            Position = new Vector2(posX, posY);
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y,
                                         CollisionBox.Width,
                                         CollisionBox.Height);

        }
    }
}
