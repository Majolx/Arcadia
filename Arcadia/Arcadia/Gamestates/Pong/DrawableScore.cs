using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcadia.Gamestates.Pong
{
    class DrawableScore : ScoreKeeper
    {
        private Vector2[] v2Positions;
        private Color[] cColors;
        private SpriteFont[] sfFonts;


        public Vector2[] Positions
        {
            get { return v2Positions; }
            set { v2Positions = value; }
        }

        public Color[] Colors
        {
            get { return cColors; }
            set { cColors = value; }
        }

        public SpriteFont[] Fonts
        {
            get { return sfFonts; }
            set { sfFonts = value; }
        }

        public DrawableScore()
            : base()
        {
            v2Positions = null;
            cColors = null;
            sfFonts = null;
        }

        public DrawableScore(int numOfPlayers)
            : base(numOfPlayers)
        {
            v2Positions = new Vector2[Players];
            cColors = new Color[Players];
            sfFonts = new SpriteFont[Players];

            for (int i = 0; i < Players; i++)
            {
                v2Positions[i] = new Vector2(0, 0);
                cColors[i] = Color.White;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Players; i++)
            {
                spriteBatch.DrawString(Fonts[i], Scores[i].ToString(), Positions[i], Colors[i]);
            }
        }
    }
}
