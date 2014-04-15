#region Information

/*  This class will hold the Sprite and some of its 
 * function needed for the asteroid game.
 *      Written by: Norlan Prudente
 *      Date: 1/7/2014
*/

#endregion

#region Using Statement

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Arcadia.Graphics
{
    public class Sprite
    {
        #region Fields

        //variable to holds texture
        Texture2D texture;

        //vectors for locations
        Vector2 position;
        Vector2 center;
        Vector2 velocity;

        //variables for rotation and scale
        float rotation;
        float scale;

        //to determine if the object is alive
        bool alive;

        int index;

        #endregion

        #region Constructor and Functions

        //constructor that can be use to create
        //a sprite for the asteroid game with a default
        //value
        public Sprite(Texture2D texture)
        {
            this.texture = texture;

            position = Vector2.Zero;
            center = new Vector2(texture.Width / 2, texture.Height / 2);
            velocity = Vector2.Zero;

            rotation = 0.0f;
            scale = 1.0f;

            alive = false;

            index = 0;
        }

        //Function that returns the texture
        public Texture2D Texture
        {
            get { return texture; }
        }

        //Function that can return and set 
        //the value of the position variable
        public Vector2 Position
        {
            get { return position; }
	        set { position = value; }
        }
	 
        //Funtion to return the center of an object
	    public Vector2 Center
	    {
	        get { return center; }
	    }

        //Function that can return and set 
        //the value of the velocity
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }	        
        }

        //Function that can return and set 
        //the value of the rotation
        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                if (rotation < -MathHelper.TwoPi)
                    rotation = MathHelper.TwoPi;
                if (rotation > MathHelper.TwoPi)
                    rotation = -MathHelper.TwoPi;
            }
        }

        //Function that can return and set 
        //the value of the scale
        public float Scale	        
        {
            get { return scale; }
            set { scale = value; }
        }
	 
        //return the value of alive variable
        public bool Alive
        {
            get { return alive; }	        
        }

        //Function that can return and set 
        //the value of index variable
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        //return the value of alive variable as true
        public void Create()
        {
            alive = true;
        }

        //return the value of alive variable as false
        public void Kill()
        {
            alive = false;
        }

        //funtion to return width
        public int Width
        {
            get { return texture.Width; }
        }

        //function to return height
        public int Height
        {
            get { return texture.Height; }
        }

        #endregion
    }
}
