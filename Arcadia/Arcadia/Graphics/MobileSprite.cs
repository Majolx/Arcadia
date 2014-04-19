#region Description
/*
 * The mobile sprite class is responsible for:
 *  - Providing an interface to the SpriteAnimation objects
 *  - Moving sprites at a defined speed towards a targeted point
 *  - Allowing a path of points to be assigned to a sprite
 *  - Providing collision information
 */
#endregion

#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Arcadia.Graphics
{
    class MobileSprite
    {
        #region Fields


        /// <summary>
        /// The SpriteAnimation object that holds the graphical 
        /// and animation data for this object.
        /// </summary>
        private SpriteAnimation asSprite;

        /// <summary>
        /// A queue of pathing vectors to allow the sprite to
        /// move along a path.
        /// </summary>
        private Queue<Vector2> queuePath = new Queue<Vector2>();

        /// <summary>
        /// The location the sprite is currently moving towards.
        /// </summary>
        private Vector2 v2Target;

        /// <summary>
        /// The speed at which the sprite will close with its target.
        /// </summary>
        private float fSpeed = 1f;

        /// <summary>
        /// Represents a clipping range for determining bounding-box
        /// style collisions.  They return the bounding box of the sprite
        /// trimmed by a horizontal and vertical offset to get a collision cushion.
        /// </summary>
        private int iCollisionBufferX = 0;
        private int iCollisionBufferY = 0;

        /// <summary>
        /// Determines the status of the sprite.  An inactive sprite will not be
        /// updated but will be drawn.
        /// </summary>
        private bool bActive = true;

        /// <summary>
        /// Determines if the sprite should track towards a v2Target.  If set to
        /// false, the sprite will not move on its own towards v2Target, and will
        /// not process pathing information.
        /// </summary>
        bool bMovingTowardsTarget = true;

        /// <summary>
        /// Determines if the sprite will follow the path in its Path queue.  If
        /// true, when the sprite has reached v2Target the next path node will be
        /// pulled from the queue and set as the new v2Target.
        /// </summary>
        bool bPathing = true;

        /// <summary>
        /// If true, any pathing node popped from the Queue will be placed back onto
        /// the end of the queue.
        /// </summary>
        bool bLoopPath = true;

        /// <summary>
        /// If true, the sprite can collide with other objects.  Note that this is only
        /// provided as a flag for testing with outside code.
        /// </summary>
        bool bCollidable = true;

        /// <summary>
        /// If true, the sprite will be drawn to the screen.
        /// </summary>
        bool bVisible = true;

        /// <summary>
        /// If true, the sprite will be deactivated when the Pathing Queue is empty.
        /// </summary>
        bool bDeactivateAtEndOfPath = false;

        /// <summary>
        /// If true, bVisible will be set to false when the Pathing Queue is empty.
        /// </summary>
        bool bHideAtEndOfPath = false;

        /// <summary>
        /// If set, when the Pathing Queue is empty, the named animation will be set
        /// as the current animation on the sprite.
        /// </summary>
        string sEndPathAnimation = null;

        
        #endregion

        #region Properties


        public SpriteAnimation Sprite
        {
            get { return asSprite; }
        }

        public Vector2 Position
        {
            get { return asSprite.Position; }
            set { asSprite.Position = value; }
        }

        public Vector2 Target
        {
            get { return v2Target; }
            set { v2Target = value; }
        }

        public int HorizontalCollisionBuffer
        {
            get { return iCollisionBufferX; }
            set { iCollisionBufferX = value; }
        }

        public int VerticalCollisionBuffer
        {
            get { return iCollisionBufferY; }
            set { iCollisionBufferY = value; }
        }

        public bool IsPathing
        {
            get { return bPathing; }
            set { bPathing = value; }
        }

        public bool DeactivateAfterPathing
        {
            get { return bDeactivateAtEndOfPath; }
            set { bDeactivateAtEndOfPath = value; }
        }

        public bool LoopPath
        {
            get { return bLoopPath; }
            set { bLoopPath = value; }
        }

        public string EndPathAnimation
        {
            get { return sEndPathAnimation; }
            set { sEndPathAnimation = value; }
        }

        public bool HideAtEndOfPath
        {
            get { return bHideAtEndOfPath; }
            set { bHideAtEndOfPath = value; }
        }

        public bool IsVisible
        {
            get { return bVisible; }
            set { bVisible = value; }
        }

        public float Speed
        {
            get { return fSpeed; }
            set { fSpeed = value; }
        }

        public bool IsActive
        {
            get { return bActive; }
            set { bActive = value; }
        }

        public bool IsMoving
        {
            get { return bMovingTowardsTarget; }
            set { bMovingTowardsTarget = value; }
        }

        public bool IsCollidable
        {
            get { return bCollidable; }
            set { bCollidable = value; }
        }

        public Rectangle BoundingBox
        {
            get { return asSprite.BoundingBox; }
        }

        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(
                    asSprite.BoundingBox.X + iCollisionBufferX,
                    asSprite.BoundingBox.Y + iCollisionBufferY,
                    asSprite.Width - (2 * iCollisionBufferX),
                    asSprite.Height - (2 * iCollisionBufferY));
            }
        }


        #endregion

        #region Initialization


        public MobileSprite(Texture2D texture)
        {
            asSprite = new SpriteAnimation(texture);
        }


        #endregion

        #region Path Nodes


        public void AddPathNode(Vector2 node)
        {
            queuePath.Enqueue(node);
        }

        public void AddPathNode(int X, int Y)
        {
            queuePath.Enqueue(new Vector2(X, Y));
        }

        public void ClearPathNodes()
        {
            queuePath.Clear();
        }


        #endregion

        #region Update and Draw


        public void Update(GameTime gameTime)
        {
            if (bActive && bMovingTowardsTarget)
            {
                if (!(v2Target == null))
                {
                    // Get a vector pointing from the current location of the sprite
                    // to the destination.
                    Vector2 Delta = new Vector2(v2Target.X - asSprite.X, v2Target.Y - asSprite.Y);

                    if (Delta.Length() > Speed)
                    {
                        Delta.Normalize();
                        Delta *= Speed;
                        Position += Delta;
                    }
                    else
                    {
                        if (v2Target == asSprite.Position)
                        {
                            if (bPathing)
                            {
                                if (queuePath.Count > 0)
                                {
                                    v2Target = queuePath.Dequeue();
                                    if (bLoopPath)
                                    {
                                        queuePath.Enqueue(v2Target);
                                    }
                                }
                                else
                                {
                                    if (!(sEndPathAnimation == null))
                                    {
                                        if (!(Sprite.CurrentAnimation == sEndPathAnimation))
                                        {
                                            Sprite.CurrentAnimation = sEndPathAnimation;
                                        }
                                    }

                                    if (bDeactivateAtEndOfPath)
                                    {
                                        IsActive = false;
                                    }

                                    if (bHideAtEndOfPath)
                                    {
                                        IsVisible = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            asSprite.Position = v2Target;
                        }
                    }
                }
            }
            if (bActive)
                asSprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (bVisible)
            {
                asSprite.Draw(spriteBatch, 0, 0);
            }
        }


        #endregion
    }
}
