using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Arcadia.Graphics
{
    class SpriteAnimation
    {

        #region Fields


        /// <summary>
        /// The texture that holds the images for this sprite.
        /// </summary>
        private Texture2D t2dTexture;


        /// <summary>
        /// True if animations are being played.
        /// </summary>
        private bool bAnimating = true;


        /// <summary>
        /// If set to anything other than Color.White, will colorize
        /// the sprite with that color.
        /// </summary>
        private Color colorTint = Color.White;


        /// <summary>
        /// Screen position of the sprite.
        /// </summary>
        private Vector2 v2Position = new Vector2(0, 0);
        private Vector2 v2LastPosition = new Vector2(0, 0);


        /// <summary>
        /// Dictionary holding all of the FrameAnimation objects
        /// associated with this sprite.
        /// </summary>
        private Dictionary<string, FrameAnimation> faAnimations = new Dictionary<string, FrameAnimation>();


        /// <summary>
        /// The currently playing animation from the dictionary.
        /// </summary>
        private string sCurrentAnimation = null;


        /// <summary>
        /// If true, the sprite will automatically rotate to align itself
        /// with the angle difference between its new position and
        /// its previous position.  In this case, the 0 rotation point
        /// is to the right (so the sprite should start out facing to
        /// the right).
        /// </summary>
        private bool bRotateByPosition = false;


        /// <summary>
        /// The angle (in radians) that the sprite should be rotated by when drawn.
        /// </summary>
        private float fRotation = 0f;


        /// <summary>
        /// Calculated center of the sprite.
        /// </summary>
        private Vector2 v2Center;


        /// <summary>
        /// Calculated width and height of the sprite.
        /// </summary>
        private int iWidth;
        private int iHeight;


        #endregion

        #region Properties


        /// <summary>
        /// Vector2 representing the position of the sprite's upper left
        /// corner pixel.
        /// </summary>
        public Vector2 Position
        {
            get { return v2Position; }
            set
            {
                v2LastPosition = v2Position;
                v2Position = value;
                UpdateRotation();
            }
        }


        /// <summary>
        /// The X position of the sprite's upper left corner pixel.
        /// </summary>
        public int X
        {
            get { return (int)v2Position.X; }
            set
            {
                v2LastPosition.X = v2Position.X;
                v2Position.X = value;
                UpdateRotation();
            }
        }


        /// <summary>
        /// The Y position of the sprite's upper left corner pixel.
        /// </summary>
        public int Y
        {
            get { return (int)v2Position.Y; }
            set
            {
                v2LastPosition.Y = v2Position.Y;
                v2Position.Y = value;
                UpdateRotation();
            }
        }


        /// <summary>
        /// Width (in pixels) of the sprite animation frames.
        /// </summary>
        public int Width
        {
            get { return iWidth; }
        }


        /// <summary>
        /// Height (in pixels) of the sprite animation frames.
        /// </summary>
        public int Height
        {
            get { return iHeight; }
        }


        /// <summary>
        /// If true, the sprite will automatically rotate in the direction
        /// of motion whenever the sprite's Position changes.
        /// </summary>
        public bool AutoRotate
        {
            get { return bRotateByPosition; }
            set { bRotateByPosition = value; }
        }


        /// <summary>
        /// The degree of rotation (in radians) to be applied to the
        /// sprite when drawn.
        /// </summary>
        public float Rotation
        {
            get { return fRotation; }
            set { fRotation = value; }
        }


        /// <summary>
        /// Screen coordinates of the bounding box surrounding this sprite.
        /// </summary>
        public Rectangle BoundingBox
        {
            get { return new Rectangle(X, Y, iWidth, iHeight); }
        }


        /// <summary>
        /// The texture associated with this sprite.
        /// </summary>
        /// <remarks>
        /// All FrameAnimations will be relative to this texture.
        /// </remarks>
        public Texture2D Texture
        {
            get { return t2dTexture; }
        }


        /// <summary>
        /// Color value to tint the sprite with when drawing.  Color.White
        /// (the default) indicates no tinting.
        /// </summary>
        public Color Tint
        {
            get { return colorTint; }
            set { colorTint = value; }
        }


        /// <summary>
        /// True if the sprite is (or should be) playing animation frames.  If this value is set
        /// to false, the sprite will not be drawn (a sprite needs at least 1 single frame animation
        /// in order to be displayed).
        /// </summary>
        public bool IsAnimating
        {
            get { return bAnimating; }
            set { bAnimating = value; }
        }


        /// <summary>
        /// The FrameAnimation object of the currently playing animation.
        /// </summary>
        public FrameAnimation CurrentFrameAnimation
        {
            get
            {
                if (!string.IsNullOrEmpty(sCurrentAnimation))
                    return faAnimations[sCurrentAnimation];
                else
                    return null;
            }
        }


        /// <summary>
        /// The string name of the currently playing animation.  Setting the animation
        /// resets the CurrentFrame and PlayCount properties to zero.
        /// </summary>
        public string CurrentAnimation
        {
            get { return sCurrentAnimation; }
            set
            {
                if (faAnimations.ContainsKey(value))
                {
                    sCurrentAnimation = value;
                    faAnimations[sCurrentAnimation].CurrentFrame = 0;
                    faAnimations[sCurrentAnimation].PlayCount = 0;
                }
            }
        }


        #endregion

        #region Initialization


        /// <summary>
        /// Creates a new sprite animation with the given sprite sheet.
        /// </summary>
        /// <param name="Texture">The sprite sheet holding the animation frames.</param>
        public SpriteAnimation(Texture2D Texture)
        {
            t2dTexture = Texture;
        }


        #endregion

        #region Manipulation Methods


        void UpdateRotation()
        {
            if (bRotateByPosition)
            {
                fRotation = (float)Math.Atan2(v2Position.Y - v2LastPosition.Y, v2Position.X - v2LastPosition.X);
            }
        }


        public void AddAnimation(string Name, int X, int Y, int Width, int Height, int Frames, float FrameLength)
        {
            faAnimations.Add(Name, new FrameAnimation(X, Y, Width, Height, Frames, FrameLength));
            iWidth = Width;
            iHeight = Height;
            v2Center = new Vector2(iWidth / 2, iHeight / 2);
        }


        public void AddAnimation(string Name, int X, int Y, int Width, int Height, int Frames,
            float FrameLength, string NextAnimation)
        {
            faAnimations.Add(Name, new FrameAnimation(X, Y, Width, Height, Frames, FrameLength, NextAnimation));
            iWidth = Width;
            iHeight = Height;
            v2Center = new Vector2(iWidth / 2, iHeight / 2);
        }


        public FrameAnimation GetAnimationByName(string Name)
        {
            if (faAnimations.ContainsKey(Name))
            {
                return faAnimations[Name];
            }
            else
            {
                return null;
            }
        }


        public void MoveBy(int x, int y)
        {
            v2LastPosition = v2Position;
            v2Position.X += x;
            v2Position.Y += y;
            UpdateRotation();
        }


        #endregion

        #region Update and Draw


        public void Update(GameTime gameTime)
        {
            // Don't do anything if the sprite is not animating
            if (bAnimating)
            {
                // If there is not a currently active animation
                if (faAnimations.Count > 0)
                {
                    // Set the active animation to the first animation
                    // associated with this sprite
                    string[] sKeys = new string[faAnimations.Count];
                    faAnimations.Keys.CopyTo(sKeys, 0);
                    CurrentAnimation = sKeys[0];
                }
                else
                {
                    return;
                }
            }

            // Run the Animation's update method
            CurrentFrameAnimation.Update(gameTime);

            // Check to see if there is a "follow up" animation named for this animation
            if (!String.IsNullOrEmpty(CurrentFrameAnimation.NextAnimation))
            {
                // If there is, see if the currently playing animation has
                // completed a full animation loop
                if (CurrentFrameAnimation.PlayCount > 0)
                {
                    // If it has, set up the next animation
                    CurrentAnimation = CurrentFrameAnimation.NextAnimation;
                }
            }
        }


        public void Draw(SpriteBatch spriteBatch, int XOffset, int YOffset)
        {
            if (bAnimating)
                spriteBatch.Draw(t2dTexture, (v2Position + new Vector2(XOffset, YOffset) + v2Center),
                                CurrentFrameAnimation.FrameRectangle, colorTint,
                                fRotation, v2Center, 1f, SpriteEffects.None, 0);
        }

        #endregion

    }
}
