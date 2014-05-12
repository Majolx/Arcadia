#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Arcadia.Screen;
#endregion

namespace Arcadia.Graphics
{
    class StaticSprite
    {
        #region Fields

        private bool bIsCollidable;
        private bool bIsVisible;
        private bool bWillStretchToCollisionBox;
        private Rectangle rCollisionBox;
        private Texture2D t2dTexture;
        private Vector2 v2Position;
        private Vector2 v2Velocity;
        private Color cColor;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether the sprite is collidable or not.
        /// False by default.
        /// </summary>
        public bool IsCollidable
        {
            get { return bIsCollidable; }
            set { bIsCollidable = value; }
        }


        /// <summary>
        /// Indicates whether the sprite is visible or not.
        /// True by default.
        /// </summary>
        public bool IsVisible
        {
            get { return bIsVisible; }
            set { bIsVisible = value; }
        }


        /// <summary>
        /// Indicates whether the sprite will stretch to its collision box.
        /// False by default.
        /// </summary>
        public bool WillStretchToCollisionBox
        {
            get { return bWillStretchToCollisionBox; }
            set { bWillStretchToCollisionBox = value; }
        }


        /// <summary>
        /// The collision box of the sprite.
        /// </summary>
        public Rectangle CollisionBox
        {
            get { return rCollisionBox; }
            set { rCollisionBox = value; }
        }


        /// <summary>
        /// The texture used on the sprite.
        /// </summary>
        public Texture2D Texture
        {
            get { return t2dTexture; }
            set { t2dTexture = value; }
        }


        /// <summary>
        /// The position of the sprite.
        /// </summary>
        public Vector2 Position
        {
            get { return v2Position; }
            set
            {
                v2Position.X = value.X;
                v2Position.Y = value.Y;
            }
        }


        /// <summary>
        /// The velocity of the sprite.
        /// </summary>
        public Vector2 Velocity
        {
            get { return v2Velocity; }
            set
            {
                v2Velocity.X = value.X;
                v2Velocity.Y = value.Y;
            }
        }


        /// <summary>
        /// The color overlay of the sprite.
        /// </summary>
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
            IsVisible = true;
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
            if (bIsVisible)
            {
                if (bWillStretchToCollisionBox)
                {
                    spriteBatch.Draw(t2dTexture, rCollisionBox, cColor);
                }
                else
                {
                    spriteBatch.Draw(t2dTexture, v2Position, cColor);
                }
            }
        }

        #endregion
    }
}
