using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Arcadia.Graphics;

namespace Arcadia.Gamestates.Pong
{
    class Ball : Sprite
    {

        public Ball(Vector2 position, Texture2D texture) : base(texture)
        {
            this.Position = position;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, Color.White);
        }
    }
}
