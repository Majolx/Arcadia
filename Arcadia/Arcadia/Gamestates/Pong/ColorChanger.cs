// ColorChanger.cs
// from robotfootgames.com/xna-tutorials/60-xna-color-interpolation-mario-style

using Microsoft.Xna.Framework;

namespace Arcadia.Gamestates.Pong
{
    public sealed class ColorChanger
    {
        private readonly Color[] colors;
        private float elapsed;
        private int nextColorIndex = 1;
        private readonly float timeBetweenColors;

        /// <summary>
        /// Gets the current color of the object.
        /// </summary>
        public Color CurrentColor
        {
            get;
            private set;
        }

        /// <summary>
        /// Get or Set whether or not the ColorChanger is active.
        /// </summary>
        public bool IsActive
        {
            get;
            set;
        }

        public ColorChanger(float timeBetweenColors, bool isActive, params Color[] colors)
        {
            this.colors = colors;
            this.timeBetweenColors = timeBetweenColors;
            this.IsActive = isActive;

            Reset(IsActive);
        }

        public void Reset(bool isActive)
        {
            // Set the initial color.
            if (!IsActive || colors.Length <= 0)
            {
                CurrentColor = Color.White;
            }
            else
            {
                CurrentColor = colors[0];
            }

            nextColorIndex = 1;
            elapsed = 0f;
            IsActive = isActive;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive || colors.Length == 1)
                return;

            elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsed >= timeBetweenColors)
            {
                if (++nextColorIndex >= colors.Length)
                    nextColorIndex = 0;

                elapsed = 0f;
            }

            CurrentColor = Color.Lerp(
                CurrentColor,
                colors[nextColorIndex],
                elapsed / timeBetweenColors);
        }
    }
}
