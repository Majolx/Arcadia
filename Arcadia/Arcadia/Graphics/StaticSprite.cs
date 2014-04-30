#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Arcadia.Graphics
{
    class StaticSprite
    {
        #region Fields

        private Rectangle rCollisionBox;
        private Texture2D t2dTexture;
        private Vector2 v2Position;
        private Vector2 v2Velocity;

        #endregion

        #region Properties

        public Rectangle CollisionBox
        {
            get { return rCollisionBox; }
            set { rCollisionBox = value; }
        }

        public Texture2D Texture
        {
            get { return t2dTexture; }
            set { t2dTexture = value; }
        }

        public Vector2 Position
        {
            get { return v2Position; }
            set
            {
                v2Position.X = value.X;
                v2Position.Y = value.Y;
            }
        }

        public Vector2 Velocity
        {
            get { return v2Velocity; }
            set
            {
                v2Velocity.X = value.X;
                v2Velocity.Y = value.Y;
            }
        }

        #endregion

        #region Initialization

        public StaticSprite()
        {
            CollisionBox = new Rectangle(0, 0, 1, 1);
            Position = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
        }

        #endregion

        #region Update and Draw

        public void Update()
        {
            rCollisionBox.X = (int)v2Position.X;
            rCollisionBox.Y = (int)v2Position.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(t2dTexture, v2Position, Color.White);
        }

        #endregion
    }
}
