using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcadia.Graphics
{
    public enum PrintDirection
    {
        LeftToRight,
        RightToLeft,
        TopToBottom,
        BottomToTop
    }

    class TextureGenerator
    {
        private PrintDirection[] printOrder = new PrintDirection[2];

        public TextureGenerator()
        {
            printOrder[0] = PrintDirection.LeftToRight;
            printOrder[1] = PrintDirection.TopToBottom;
        }

        public TextureGenerator(PrintDirection dir1, PrintDirection dir2)
        {
            printOrder[0] = dir1;
            printOrder[1] = dir2;
        }

        public Texture2D GenerateTexture(GraphicsDevice graphicsDevice, int width, int height)
        {
            Texture2D texture = new Texture2D(graphicsDevice, width, height);

            Color[] textureData = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(textureData);

            return texture;
        }
    }
}
