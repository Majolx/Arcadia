#region Information

/* Asteroid, a game where a player take control of a spaceship
 * and will try to destroy as much asteriods as it can before
 * crashing to one of the asteriods.
 *      Written by: Norlan Prudente
 *      Date: 01/07/2014
 */
  
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arcadia;
using Arcadia.Screen;
using Arcadia.Graphics;
#endregion

namespace Arcadia.Gamestates.Asteroids
{
    class AsteroidGameScreen : GameScreen
    {
        #region Fields

        //Sprite variables
        Sprite ship;
        Sprite shot;

        //List to hold shots
        List<Sprite> shots = new List<Sprite>();

        //List to hold asteroids and its textures
        List<Texture2D> asteroidTexture = new List<Texture2D>();
        List<Sprite> asteroids = new List<Sprite>();

        //keyboard states
        KeyboardState currentKBState;
        KeyboardState previousKBState;
        
        //variable for distance
        float distance;

        //variable for random
        Random random = new Random();

        //variable to hold the levels
        int level = 0;

        //score tracker
        int score = 0;

        //life tracker
        int lives = 0;

        //Font
        SpriteFont scoreFont;

        //boolean for game over
        bool gameOver = false;

        //sounds
        SoundEffect shotEffect;
        SoundEffect backgroundMusic;

        //sound counter
        int musicCounter = 220;

        /// <summary>
        /// A screen-specific content manager.
        /// </summary>
        public ContentManager content;

        #endregion

        #region Initialization
        
        /// <summary>
        /// The constructor for the Asteroid game screen
        /// </summary>
        public AsteroidGameScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            lives = 3;
        }        

        /// <summary>
        /// Loads game content.  Contains its own content manager to keep
        /// the game lightweight.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            string ContentLoadDir = "Sprite/Asteroids/";

            //ship's texture
            ship = new Sprite(content.Load<Texture2D>(ContentLoadDir + "ship"));
            
            //shot's texture
            shot = new Sprite(content.Load<Texture2D>(ContentLoadDir + "shot"));

            //large asteroids
            for (int i = 1; i < 4; i++)
                asteroidTexture.Add(content.Load<Texture2D>(
                    ContentLoadDir + "large" + i.ToString()));

            //medium asteroids
            for (int i = 1; i < 4; i++)
                asteroidTexture.Add(content.Load<Texture2D>(
                    ContentLoadDir + "medium" + i.ToString()));

            //small asteroids
            for (int i = 1; i < 4; i++)
                asteroidTexture.Add(content.Load<Texture2D>(
                    ContentLoadDir + "small" + i.ToString()));

            //Font
            scoreFont = content.Load<SpriteFont>("Font/asteroidFont");

            //sound
            shotEffect = content.Load<SoundEffect>("Sounds/shotSoundEffect");
            backgroundMusic = content.Load<SoundEffect>("Sounds/stage6");

            Initialize();

            base.LoadContent();
        }

        public void Initialize()
        {
            //reset the rotation of the screen
            ship.Rotation = 0;

            //reset ship's velocity
            ship.Velocity = Vector2.Zero;

            //clear shots
            shots.Clear();

            //get the center of the screen
            ship.Position = new Vector2(ScreenManager.Game.GraphicsDevice.Viewport.Width / 2,
                ScreenManager.Game.GraphicsDevice.Viewport.Height / 2);
            
            //set the ship to be alive
            ship.Create();

        }

        /// <summary>
        /// Unloads game content.  Make sure to close up any files and
        /// free up any resources before exiting.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        #endregion

        #region Handle Input


        /// <summary>
        /// Handles the input for this screen.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            //get the new keyboard state
            currentKBState = Keyboard.GetState();

            if (ship.Alive)
            {
                //make the ship move in the direction it's facing
                if (currentKBState.IsKeyDown(Keys.Up))
                {
                    //full speed
                    AccelerateShip();
                }
                else if (currentKBState.IsKeyUp(Keys.Up))
                {
                    //make the ship stop slowly instead
                    //of full stop when the key is realeased
                    DecelerateShip();
                }

                //to make the ship appear to go in hyperSpace
                //porting in a random place
                //Left ctrl key
                if (currentKBState.IsKeyUp(Keys.LeftControl) &&
                    previousKBState.IsKeyDown(Keys.LeftControl))
                {
                    HyperSpace();
                }
                //Right ctrl key
                if (currentKBState.IsKeyUp(Keys.RightControl) &&
                    previousKBState.IsKeyDown(Keys.RightControl))
                {
                    HyperSpace();
                }

                //make the ship rotate
                //counter clockwise
                if (currentKBState.IsKeyDown(Keys.Left))
                    ship.Rotation -= 0.05f;
                //clockwise
                if (currentKBState.IsKeyDown(Keys.Right))
                    ship.Rotation += 0.05f;

                //space to shoot
                if (currentKBState.IsKeyUp(Keys.Space) && previousKBState.IsKeyDown(Keys.Space))
                {
                    FireShot();
                    shotEffect.Play();
                }
            }

            //when game over
            if (gameOver)
            {
                if (ship.Alive)
                    ship.Kill();

                if (currentKBState.IsKeyDown(Keys.Enter))
                {
                    asteroids.Clear();
                    Initialize();
                    lives = 3;
                    score = 0;
                    level = 0;
                    CreateAsteroids();
                    gameOver = false;
                }
            }
            
            //save the last keyboard state
            previousKBState = currentKBState;

            base.HandleInput(input);
        }

        //Function that will handle the accelaration of the ship
        //it will thrust to where the ship is going.
        //Pre   Up key is pressed
        //Post  Move to the direction the ship is facing
        private void AccelerateShip()
        {
            ship.Velocity += new Vector2(
                (float)(Math.Cos(ship.Rotation - MathHelper.PiOver2) * 0.05f),
                (float)((Math.Sin(ship.Rotation - MathHelper.PiOver2) * 0.05f)));

            if (ship.Velocity.X > 5.0f)
            {
                ship.Velocity = new Vector2(5.0f, ship.Velocity.Y);
            }
            if (ship.Velocity.X < -5.0f)
            {
                ship.Velocity = new Vector2(-5.0f, ship.Velocity.Y);
            }
            if (ship.Velocity.Y > 5.0f)
            {
                ship.Velocity = new Vector2(ship.Velocity.X, 5.0f);
            }
            if (ship.Velocity.Y < -5.0f)
            {
                ship.Velocity = new Vector2(ship.Velocity.X, -5.0f);
            }
        }

        //Function that will handle the decelaration of the ship.
        //Pre   Up key was released
        //Post  Slowly make the ship stop from moving
        private void DecelerateShip()
        {
            if (ship.Velocity.X < 0)
            {
                ship.Velocity = new Vector2(
                    ship.Velocity.X + 0.02f, ship.Velocity.Y);
            }

            if (ship.Velocity.X > 0)
            {
                ship.Velocity = new Vector2(
                    ship.Velocity.X - 0.02f, ship.Velocity.Y);
            }

            if (ship.Velocity.Y < 0)
            {
                ship.Velocity = new Vector2(
                    ship.Velocity.X, ship.Velocity.Y + 0.02f);
            }

            if (ship.Velocity.Y > 0)
            {
                ship.Velocity = new Vector2(
                    ship.Velocity.X, ship.Velocity.Y - 0.02f);
            }
        }

        //Make the ship teleport in a random place
        //Pre   Left or right ctrl key is pressed
        //Post  Teleport on a random place
        private void HyperSpace()
        {
            int positionX;
            int positionY;

            positionX = random.Next(ship.Width, 
                ScreenManager.Game.GraphicsDevice.Viewport.Width - ship.Width);
            positionY = random.Next(ship.Height,
                ScreenManager.Game.GraphicsDevice.Viewport.Height - ship.Height);

            ship.Position = new Vector2(positionX, positionY);

            ship.Velocity = Vector2.Zero;
        }

        #endregion

        #region Update


        /// <summary>
        /// The screen's update loop.  Place any update logic here.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            musicCounter++;

            if (musicCounter > 220)
            {
                backgroundMusic.Play();
                musicCounter = 0;
            }

            //Function call for updating ship movement
            UpdateShip();

            //Function call for updating asteroids
            UpdateAsteroids();

            //Function call for updating the shot
            UpdateShots();

            //Function call when everything is dead
            AllDead();

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        //function when everything is dead.
        //Pre   All asteroids is destroyed
        //Post  Go to the next level
        private void AllDead()
        {
            bool allDead = true;

            foreach (Sprite s in asteroids)
            {
                if (s.Alive)
                    allDead = false;
            }

            if (allDead)
            {
                level++;
                NewLevel();
            }

        }

        private void NewLevel()
        {
            asteroids.Clear();
            Initialize();
            CreateAsteroids();
        }

        //funtion to handle the update for the bullet
        //Pre   A shot was made
        //Post  Make the bullet go to the direction the ship is facing
        private void UpdateShots()
        {
            List<Sprite> destroyed = new List<Sprite>();
            foreach (Sprite s in shots)
            {
                s.Position += s.Velocity;
                foreach (Sprite a in asteroids)
                {
                    if (a.Alive && CheckAsteroidCollision(a, s))
                    {
                        a.Kill();
                        destroyed.Add(a);
                        s.Kill();

                        if (a.Index < 3)
                            score += 25;
                        else if (a.Index < 6)
                            score += 50;
                        else
                            score += 100;
                    }
                }
                if (s.Position.X < 0)
                    s.Kill();
                else if (s.Position.Y < 0)
                    s.Kill();
                else if (s.Position.X > ScreenManager.Game.GraphicsDevice.Viewport.Width)
                    s.Kill();
                else if (s.Position.Y > ScreenManager.Game.GraphicsDevice.Viewport.Height)
                    s.Kill();
            }

            for (int i = 0; i < shots.Count; i++)
            {
                if (!shots[i].Alive)
                {
                    shots.RemoveAt(i);
                    i--;
                }
            }

            //Split the asteroid if possible
            foreach (Sprite a in destroyed)
            {
                SplitAsteroid(a);
            }
        }


        //funtion that check asteroid collision using
        //Pythagerium therom.
        //Pre   Distance is less than the asteroid's width
        //Post  return true else return false
        private bool CheckAsteroidCollision(Sprite asteroid, Sprite shot)
        {
            Vector2 position1 = asteroid.Position;
            Vector2 position2 = shot.Position;

            float Cathetus1 = Math.Abs(position1.X - position2.X);
            float Cathetus2 = Math.Abs(position1.Y - position2.Y);

            Cathetus1 *= Cathetus1;
            Cathetus2 *= Cathetus2;

            distance = (float)Math.Sqrt(Cathetus1 + Cathetus2);

            if ((int)distance < asteroid.Width)
                return true;

            return false;
        }

        //function for handling firing a shot
        //Pre   Shot was made
        //Post  Shot added to the link list
        private void FireShot()
        {
            Sprite newShot = new Sprite(shot.Texture);

            Vector2 velocity = new Vector2(
                (float)Math.Cos(ship.Rotation - MathHelper.PiOver2),
                (float)Math.Sin(ship.Rotation - MathHelper.PiOver2)) * 4.0f + ship.Velocity;
            velocity.Normalize();
            velocity *= 6.0f;

            newShot.Velocity = velocity;

            newShot.Position = ship.Position + newShot.Velocity;
            newShot.Create();
	 
            shots.Add(newShot);
        }

        //Function that handle the the asteroids update
        //Pre   No asteroids on the screen
        //Post  Asteroids Created
        private void CreateAsteroids()
        {
            int value;

            for (int i = 0; i < 4 + level; i++)
            {
                int index = random.Next(0, 3);

                Sprite tempSprite = new Sprite(asteroidTexture[index]);
                asteroids.Add(tempSprite);
                asteroids[i].Index = index;

                double xPos = 0;
                double yPos = 0;

                value = random.Next(0, 4);

                switch (value)
                {
                    case 0:
                    case 1:
                        xPos = asteroids[i].Width + random.NextDouble() * 40;
                        yPos = random.NextDouble() * ScreenManager.Game.GraphicsDevice.Viewport.Height;
                        break;
                    case 2:
                    case 3:
                        xPos = ScreenManager.Game.GraphicsDevice.Viewport.Width - random.NextDouble() * 40;
                        yPos = random.NextDouble() * ScreenManager.Game.GraphicsDevice.Viewport.Height;
                        break;
                    case 4:
                    case 5:
                        xPos = random.NextDouble() * ScreenManager.Game.GraphicsDevice.Viewport.Width;
                        yPos = asteroids[i].Height + random.NextDouble() * 40;
                        break;
                    default:
                        xPos = random.NextDouble() * ScreenManager.Game.GraphicsDevice.Viewport.Width;
                        yPos = ScreenManager.Game.GraphicsDevice.Viewport.Height - random.NextDouble() * 40;
                        break;
                }

                asteroids[i].Position = new Vector2((float)xPos, (float)yPos);

                asteroids[i].Velocity = RandomVelocity();

                asteroids[i].Rotation = (float)random.NextDouble() *
                        MathHelper.Pi * 4 - MathHelper.Pi * 2;

                asteroids[i].Create();
            }
        }

        //function for checking ship collision
        //using Pythagerium therom.
        //Pre   Distance is less than ship's width
        //Post  Return true else return false
        private bool CheckShipCollision(Sprite asteroid)
        {
            Vector2 position1 = asteroid.Position;
            Vector2 position2 = ship.Position;

            float Cathetus1 = Math.Abs(position1.X - position2.X);
            float Cathetus2 = Math.Abs(position1.Y - position2.Y);

            Cathetus1 *= Cathetus1;
            Cathetus2 *= Cathetus2;

            distance = (float)Math.Sqrt(Cathetus1 + Cathetus2);

            if ((int)distance < ship.Width)
                return true;

            return false;
        }

        //Function handling ship position
        //Pre   Ship went further than the edge of the screen
        //Post  Make the ship appear on the opposite side
        public void UpdateShip()
        {
            ship.Position += ship.Velocity;

            if (ship.Position.X + ship.Width < 0)
            {
                ship.Position = new Vector2(ScreenManager.Game.GraphicsDevice.Viewport.Width,
                    ship.Position.Y);
            }
            if (ship.Position.X - ship.Width > ScreenManager.Game.GraphicsDevice.Viewport.Width)
            {
                ship.Position = new Vector2(0, ship.Position.Y);
            }
            if (ship.Position.Y + ship.Height < 0)
            {
                ship.Position = new Vector2(ship.Position.X, ScreenManager.Game.GraphicsDevice.Viewport.Height);
            }
            if (ship.Position.Y - ship.Height > ScreenManager.Game.GraphicsDevice.Viewport.Height)
            {
                ship.Position = new Vector2(ship.Position.X, 0);
            }
        }

        //If the asteroid reach the edge of the screen make it appear on
        //the opposite side.
        //Pre   Asteroid reach the edge of screen
        //Post  Make asteroid appear on the opposite side
        private void UpdateAsteroids()
        {
            List<Sprite> destroyed = new List<Sprite>();
            foreach (Sprite a in asteroids)
            {
                a.Position += a.Velocity;

                //if an asteroid goes to the left side of the screen
                //teleport it to the right side;
                if (a.Position.X + a.Width < 0)
                {
                    a.Position = new Vector2(
                        ScreenManager.Game.GraphicsDevice.Viewport.Width,
                        a.Position.Y);
                }

                //if an asteroid goes to the upper side of the screen
                //teleport it to the bottom side;
                if (a.Position.Y + a.Height < 0)
                {
                    a.Position = new Vector2(
                        a.Position.X,
                        ScreenManager.Game.GraphicsDevice.Viewport.Height);

                }

                //if an asteroid goes to the right side of the screen
                //teleport it to the left side;
                if (a.Position.X - a.Width > ScreenManager.Game.GraphicsDevice.Viewport.Width)
                {
                    a.Position = new Vector2(0, a.Position.Y);
                }

                //if an asteroid goes to the bottom side of the screen
                //teleport it to the upper side;
                if (a.Position.Y - a.Height > ScreenManager.Game.GraphicsDevice.Viewport.Height)
                {
                    a.Position = new Vector2(a.Position.X, 0);

                }

                //collision
                if (a.Alive && CheckShipCollision(a))
                {
                    a.Kill();
                    destroyed.Add(a);
                    lives--;
                    Initialize();
                    if (lives < 1)
                        gameOver = true;
                }
            }

            foreach (Sprite a in destroyed)
                SplitAsteroid(a);
        }

        //when asteroid get hits.
        //Pre   Shot hits the asteroid
        //Post  Split it if possible else destroy it
        private void SplitAsteroid(Sprite a)
        {
            if (a.Index < 3)
            {
                for (int i = 0; i < 2; i++)
                {
                    int index = random.Next(3, 6);
                    NewAsteroid(a, index);
                }
            }
            else if (a.Index < 6)
            {
                for (int i = 0; i < 2; i++)
                {
                    int index = random.Next(6, 9);
                    NewAsteroid(a, index);
                }
            }
        }

        //function for new creating asteroid when it get hit.
        //Pre   Asteroid is hit.
        //Post  New asteroid is created into smaller size
        private void NewAsteroid(Sprite a, int index)
        {
            Sprite tempSprite = new Sprite(asteroidTexture[index]);

            tempSprite.Index = index;
            tempSprite.Position = a.Position;
            tempSprite.Velocity = RandomVelocity();

            tempSprite.Rotation = (float)random.NextDouble() *
                MathHelper.Pi * 4 - MathHelper.Pi * 2;

            tempSprite.Create();
            asteroids.Add(tempSprite);
        }

        //random velocity for asteroids
        private Vector2 RandomVelocity()
        {
            float xVelocity = (float)(random.NextDouble() * 2 + .5);
            float yVelocity = (float)(random.NextDouble() * 2 + .5);

            if (random.Next(2) == 1)
                xVelocity *= -1.0f;

            if (random.Next(2) == 1)
                yVelocity *= -1.0f;

            return new Vector2(xVelocity, yVelocity);
        }

        #endregion

        #region Draw
        /// <summary>
        /// The screen's draw loop.  Draw all artifacts here.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;


            spriteBatch.Begin();

            //for displaying the score
            Vector2 position = new Vector2(10, 10);

            spriteBatch.DrawString(scoreFont, "Score: " + score.ToString(),
                position, Color.White);
            
            //displaying the lives
            Rectangle shipRect;

            for (int i = 0; i < lives; i++)
            {
                shipRect = new Rectangle(i * ship.Width + 10,
                    40, ship.Width, ship.Height);

                spriteBatch.Draw(ship.Texture, shipRect, Color.White);
            }

            //for displaying the ship
            if (ship.Alive)
            {
                spriteBatch.Draw(ship.Texture,
                    ship.Position,
                    null,
                    Color.White,
                    ship.Rotation,
                    ship.Center,
                    ship.Scale,
                    SpriteEffects.None,
                    1.0f);
            }

            //for displaying the shots in screen
            foreach (Sprite s in shots)
            {
                spriteBatch.Draw(s.Texture,
                    s.Position,
                    null,
                    Color.White,
                    s.Rotation,
                    s.Center,
                    s.Scale,
                    SpriteEffects.None,
                    1.0f);
            }
            
            //for displaying asteroids in the screen
            foreach (Sprite a in asteroids)
            {
                if (a.Alive)
                {
                    spriteBatch.Draw(a.Texture,
                        a.Position,
                        null,
                        Color.White,
                        a.Rotation,
                        a.Center,
                        a.Scale,
                        SpriteEffects.None,
                        1.0f);
                }
            }

            spriteBatch.End();

            if (gameOver)
            {
                spriteBatch.Begin();
	 
                Vector2 position2 = new Vector2(0.0f, 20.0f);
	 
                string text = "GAME OVER";
	 
                Vector2 size = scoreFont.MeasureString(text);
	 
                position2 = new Vector2((ScreenManager.Game.GraphicsDevice.Viewport.Width / 2) - (size.X / 2),
                    ScreenManager.Game.GraphicsDevice.Viewport.Height / 2 - (size.Y * 2));
	 
                spriteBatch.DrawString(scoreFont, text, position2, Color.White);
	 
                text = "PRESS <ENTER> TO START";
                size = scoreFont.MeasureString(text);

                position2 = new Vector2((ScreenManager.Game.GraphicsDevice.Viewport.Width / 2) - (size.X / 2),
                    ScreenManager.Game.GraphicsDevice.Viewport.Height / 2 + (size.Y * 2));
	 
                spriteBatch.DrawString(scoreFont, text, position2, Color.White);
	 
                spriteBatch.End();
	 
                return;
            }
            base.Draw(gameTime);
        }


        #endregion
    }
}
