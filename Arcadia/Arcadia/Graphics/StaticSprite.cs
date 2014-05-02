#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Arcadia.Graphics
{
    class StaticSprite
    {
        #region Fields

        private bool bIsCollidable;
        private Rectangle rCollisionBox;
        private Texture2D t2dTexture;
        private Vector2 v2Position;
        private Vector2 v2Velocity;
        private Color cColor;

        #endregion

        #region Properties

        /// <summary>
        /// T/F: The collision box of this sprite is collidable.
        /// Default: False
        /// </summary>
        public bool IsCollidable
        {
            get { return bIsCollidable; }
            set { bIsCollidable = value; }
        }

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

        public Color Color
        {
            get { return cColor; }
            set { cColor = value; }
        }

        #endregion

        #region Initialization

        public StaticSprite()
        {
            IsCollidable = false;
            CollisionBox = new Rectangle(0, 0, 1, 1);
            Position = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
            Color = Color.White;
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
            spriteBatch.Draw(t2dTexture, v2Position, cColor);
        }

        #endregion
    }
}
