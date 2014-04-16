using System;
using Microsoft.Xna.Framework;


namespace Arcadia.Graphics
{
    class FrameAnimation : ICloneable
    {

        #region Fields


        ///<summary>
        /// The first frame of the Animation.  Other frames are
        /// calculated based on this frame.
        /// </summary>
        private Rectangle rectInitialFrame;


        /// <summary>
        /// Number of frames in the Animation.
        /// </summary>
        private int iFrameCount = 1;


        /// <summary>
        /// The frame currently being displayed.
        /// The value ranges from 0 to iFrameCount-1
        /// </summary>
        private int iCurrentFrame = 0;


        /// <summary>
        /// Amount of time (in seconds) to display each frame.
        /// </summary>
        private float fFrameLength = 0.2f;


        /// <summary>
        /// Amount of time that has passed since we last animated.
        /// </summary>
        private float fFrameTimer = 0.0f;


        /// <summary>
        /// The number of times this animation has been played.
        /// </summary>
        private int iPlayCount = 0;


        /// <summary>
        /// The animation that should be played after this animation.
        /// </summary>
        private string sNextAnimation = null;


        #endregion

        #region Properties


        /// <summary>
        /// The number of frames the animation contains.
        /// </summary>
        public int FrameCount
        {
            get { return iFrameCount; }
            set { iFrameCount = value; }
        }


        /// <summary>
        /// The time (in seconds) to display each frame.
        /// </summary>
        public float FrameLength
        {
            get { return fFrameLength; }
            set { fFrameLength = value; }
        }


        /// <summary>
        /// The frame number currently being displayed
        /// </summary>
        public int CurrentFrame
        {
            get { return iCurrentFrame; }
            set { iCurrentFrame = (int)MathHelper.Clamp(value, 0, iFrameCount - 1); }
        }

        
        /// <summary>
        /// The width of the frame.
        /// </summary>
        public int FrameWidth
        {
            get { return rectInitialFrame.Width; }
        }


        /// <summary>
        /// The height of the frame.
        /// </summary>
        public int FrameHeight
        {
            get { return rectInitialFrame.Height; }
        }


        /// <summary>
        /// The rectangle associated with the current animation frame.
        /// </summary>
        public Rectangle FrameRectangle
        {
            get
            {
                return new Rectangle(
                    rectInitialFrame.X + (rectInitialFrame.Width + iCurrentFrame),
                    rectInitialFrame.Y, rectInitialFrame.Width, rectInitialFrame.Height);
            }
        }


        /// <summary>
        /// The number of times this animation has been played.
        /// </summary>
        public int PlayCount
        {
            get { return iPlayCount; }
            set { iPlayCount = value; }
        }


        /// <summary>
        /// The animation that should be played after this animation.
        /// </summary>
        public string NextAnimation
        {
            get { return sNextAnimation; }
            set { sNextAnimation = value; }
        }


        #endregion

        #region Initialization

        /// <summary>
        /// Constructor which uses default frame length and no following animation.
        /// </summary>
        /// <param name="FirstFrame">The first bounding rectangle of the animation.</param>
        /// <param name="Frames">The number of frames in the animation.</param>
        public FrameAnimation(Rectangle FirstFrame, int Frames)
        {
            rectInitialFrame = FirstFrame;
            iFrameCount = Frames;
        }


        /// <summary>
        /// Constructor which uses default frame length and no following animation.
        /// </summary>
        /// <param name="X">The X position of the first frame.</param>
        /// <param name="Y">The Y position of the first frame.</param>
        /// <param name="Width">The width of the frame.</param>
        /// <param name="Height">The height of the frame.</param>
        /// <param name="Frames">The number of frames in the animation.</param>
        public FrameAnimation(int X, int Y, int Width, int Height, int Frames)
        {
            rectInitialFrame = new Rectangle(X, Y, Width, Height);
            iFrameCount = Frames;
        }


        /// <summary>
        /// Constructor which has no following animation.
        /// </summary>
        /// <param name="X">The X position of the first frame.</param>
        /// <param name="Y">The Y position of the first frame.</param>
        /// <param name="Width">The width of the frame.</param>
        /// <param name="Height">The height of the frame.</param>
        /// <param name="Frames">The number of frames in the animation.</param>
        /// <param name="FrameLength">The length of time each frame is shown.</param>
        public FrameAnimation(int X, int Y, int Width, int Height, int Frames, float FrameLength)
        {
            rectInitialFrame = new Rectangle(X, Y, Width, Height);
            iFrameCount = Frames;
            fFrameLength = FrameLength;
        }


        /// <summary>
        /// Constructor for an animation with a follow-up animation.
        /// </summary>
        /// <param name="X">The X position of the first frame.</param>
        /// <param name="Y">The Y position of the first frame.</param>
        /// <param name="Width">The width of the frmae.</param>
        /// <param name="Height">The height of the frame.</param>
        /// <param name="Frames">The number of frames in the animation.</param>
        /// <param name="FrameLength">The length of time each frame is shown.</param>
        /// <param name="strNextAnimation">The animation to play after this animation's death.</param>
        public FrameAnimation(int X, int Y, int Width, int Height,
                              int Frames, float FrameLength, string strNextAnimation)
        {
            rectInitialFrame = new Rectangle(X, Y, Width, Height);
            iFrameCount = Frames;
            fFrameLength = FrameLength;
            sNextAnimation = strNextAnimation;
        }


        #endregion

        #region Update


        public void Update(GameTime gameTime)
        {
            fFrameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (fFrameTimer > fFrameLength)
            {
                fFrameTimer = 0.0f;
                iCurrentFrame = (iCurrentFrame + 1) % iFrameCount;
                if (iCurrentFrame == 0)
                    iPlayCount = (int)MathHelper.Min(iPlayCount + 1, int.MaxValue);
            }
        }


        #endregion

        #region Clone


        public object ICloneable.Clone()
        {
            return new FrameAnimation(this.rectInitialFrame.X, this.rectInitialFrame.Y,
                                      this.rectInitialFrame.Width, this.rectInitialFrame.Height,
                                      this.iFrameCount, this.fFrameLength, sNextAnimation);
        }


        #endregion
    }
}
