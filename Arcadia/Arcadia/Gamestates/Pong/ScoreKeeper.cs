using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcadia.Gamestates.Pong
{
    class ScoreKeeper
    {
        private int iPlayers;
        private int[] iScores;

        public int Players
        {
            get { return iPlayers; }
            set { iPlayers = value; }
        }

        public int[] Scores
        {
            get { return iScores; }
        }

        public ScoreKeeper()
        {
            iPlayers = 0;
            iScores = null;
        }

        public ScoreKeeper(int numOfPlayers)
        {
            iPlayers = numOfPlayers;
            iScores = new int[Players];

            ResetScore();
        }

        public void AddScore(int player)
        {
            if (player >= 0 && player < Players)
            {
                iScores[player] += 1;
            }
        }

        public void AddScore(int player, int score)
        {
            if (player >= 0 && player < Players)
            {
                iScores[player] += score;
            }
        }

        public void ResetScore()
        {
            for (int i = 0; i < Players; i++)
            {
                iScores[i] = 0;
            }
        }

        public void ResetScore(int player)
        {
            if (player >= 0 && player < Players)
            {
                iScores[player] = 0;
            }
        }

    }
}
