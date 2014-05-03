using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Arcadia.Graphics;

namespace Arcadia.Gamestates.Pong
{
    class ArenaComponent : StaticSprite
    {
        #region Fields


        private bool bIsDotted;


        #endregion

        #region Properties


        public bool IsDotted
        {
            get { return bIsDotted; }
            set { bIsDotted = value; }
        }


        #endregion

        #region Initialization


        public ArenaComponent()
        {
            bIsDotted = false;
        }

        public ArenaComponent(bool isDotted)
        {
            bIsDotted = isDotted;
        }


        #endregion

        #region This shit needs to be moved into its own helper class at some point in the future


        public void FinalizeTexture(GraphicsDevice gd)
        {
            Texture = new Texture2D(gd, CollisionBox.Width, CollisionBox.Height);
            

            // Colorize the rectangle texture
            Color[] textureData = new Color[Texture.Width * Texture.Height];
            Texture.GetData<Color>( textureData );

            int dottedDepth = 10;
            bool penDown = true;

            int count = 0;
            for (int y = 0; y < Texture.Height; y++)
            {
                for (int x = 0; x < Texture.Width; x++)
                {
                    if (bIsDotted && y % dottedDepth == 0 && x == 0)
                    {
                        if (penDown == true)
                        {
                            penDown = false;
                        }
                        else
                        {
                            penDown = true;
                        }
                    }

                    if (penDown)
                    {
                        textureData[count] = Color;
                    }
                    else
                    {
                        textureData[count] = Color.Black;
                    }

                    count++;
                }
            }

            Texture.SetData<Color>(textureData);
        }


        #endregion
    }
}
