#region File Description
//-----------------------------------------------------------------------------
// Game.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Arcadia.Screen;
using Arcadia.Gamestates.Menu;
#endregion

namespace Arcadia
{
    /// <summary>
    /// Sample showing how to manage different game states, with transitions
    /// between menu screens, a loading screen, the game itself, and a pause
    /// menu. This main game class is extremely simple: all the interesting
    /// stuff happens in the ScreenManager component.
    /// </summary>
    public class ArcadiaGame : Microsoft.Xna.Framework.Game
    {
        #region Fields

        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        public List<DisplayMode> SupportedDisplayModes
        {
            get { return supportedDisplayModes; }
        }
        List<DisplayMode> supportedDisplayModes;

        // By preloading any assets used by UI rendering, we avoid framerate glitches
        // when they suddenly need to be loaded in the middle of a menu transition.
        static readonly string[] preloadAssets =
        {
            "gradient",
        };

        
        #endregion

        #region Initialization


        /// <summary>
        /// The main game constructor.
        /// </summary>
        public ArcadiaGame()
        {
            supportedDisplayModes = new List<DisplayMode>();

            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;

            // Create the screen manager component.
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            // Activate the first screens.
            // Format:
            // screenManager.AddScreen( Screen to Add, Controlling Player)
            screenManager.AddScreen(new MenuBackground(), null);
        }


        /// <summary>
        /// Loads graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            // Get a list of all supporte display modes
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                supportedDisplayModes.Add(mode);
            }

            // Load the preload assets
            foreach (string asset in preloadAssets)
            {
                Content.Load<object>("Sprite/" + asset);
            }
        }


        #endregion

        #region Update

        protected override void Update(GameTime gameTime)
        {
            if (screenManager.RequestedResolutionChange)
            {
                SetResolution(screenManager.RequestedResolutionX, screenManager.RequestedResolutionY);
                screenManager.RequestedResolutionChange = false;
            }
            base.Update(gameTime);
        }

        public void SetResolution(int width, int height)
        {
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();
        }

        #endregion

        #region Draw


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }


        #endregion
    }


    #region Entry Point

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static class Program
    {
        static void Main()
        {
            using (ArcadiaGame game = new ArcadiaGame())
            {
                game.Run();
            }
        }
    }

    #endregion
}
