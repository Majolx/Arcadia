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

        public void Bounce()
        {
            Direction = 2*(float)Math.PI - fDirection;
        }

        public void ReverseDirection()
        {
            Direction += (float)Math.PI;
        }

        public void Update(GameTime gameTime)
        {
            Position = new Vector2(Position.X + fSpeed * ((float)Math.Cos(fDirection)),
                                   Position.Y + fSpeed * ((float)Math.Sin(fDirection)));
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y,
                                         CollisionBox.Width,
                                         CollisionBox.Height);

        }
    }
}
