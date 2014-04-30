using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Arcadia.Graphics;

namespace Arcadia.Gamestates.Pong
{
    class Ball : StaticSprite
    {
        private int speed = 5;

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public Ball(Texture2D texture)
        {
            Texture = texture;
            CollisionBox = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Position = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
        }
    }
}
