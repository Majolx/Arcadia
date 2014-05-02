using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Arcadia.Graphics;

namespace Arcadia.Gamestates.Pong
{
    class Paddle : StaticSprite
    {
        #region Fields

        private Color cOuterColor;
        private Color cInnerColor;

        #endregion

        #region Properties

        public Color OuterColor
        {
            get { return cOuterColor; }
            set { cOuterColor = value; }
        }

        public Color InnerColor
        {
            get { return cInnerColor; }
            set { cInnerColor = value; }
        }

        #endregion

        #region Initialization

        public Paddle()
        {
            CollisionBox = new Rectangle(0, 0, 10, 80);
            Position = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
            cOuterColor = Color.White;
            cInnerColor = Color.Black;
        }

        public void FinalizeTexture(GraphicsDevice gd)
        {
            Texture = new Texture2D(gd, CollisionBox.Width, CollisionBox.Height);
            

            // Colorize the rectangle texture
            Color[] textureData = new Color[Texture.Width * Texture.Height];
            Texture.GetData<Color>(textureData);

            int border = 2;

            int count = 0;
            for (int y = 0; y < Texture.Height; y++)
            {
                for (int x = 0; x < Texture.Width; x++)
                {
                    if (y <= border || y >= Texture.Height-border-1 || x <= border || x >= Texture.Width-border-1)
                    {
                        textureData[count] = cOuterColor;
                    }
                    else
                    {
                        textureData[count] = cInnerColor;
                    }

                    count++;
                }
            }

            Texture.SetData<Color>(textureData);
        }

        #endregion
    }
}
