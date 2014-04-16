#region File Description
//-----------------------------------------------------------------------------
// NewGameMenu.cs
//
// The menu which handles the New Game selection in MainMenuScreen.cs.
// Provides options to navigate to any of the games we have made.
//
// Written by: Mathew Larribas
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Arcadia.Screen;
using Arcadia.Gamestates.Pong;
using Arcadia.Gamestates.Asteroids;
using Arcadia.Gamestates.Tetris;
#endregion

namespace Arcadia.Gamestates.Menu
{
    class NewGameMenuScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public NewGameMenuScreen()
            : base("Choose a game to play")
        {
            // Create the menu entries
            MenuEntry pongMenuEntry = new MenuEntry("Pong");
            MenuEntry asteroidsMenuEntry = new MenuEntry("Asteroids");
            MenuEntry tetrisMenuEntry = new MenuEntry("Tetris");
            MenuEntry backMenuEntry = new MenuEntry("Back");

            // Hook up menu event handlers
            pongMenuEntry.Selected += PongMenuEntrySelected;
            asteroidsMenuEntry.Selected += AsteroidsMenuEntrySelected;
            tetrisMenuEntry.Selected += TetrisMenuEntrySelected;
            backMenuEntry.Selected += BackMenuEntrySelected;

            // Add entries to the menu
            MenuEntries.Add(pongMenuEntry);
            MenuEntries.Add(asteroidsMenuEntry);
            MenuEntries.Add(tetrisMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Start the Pong game.
        /// </summary>
        void PongMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new PongScreen(), e.PlayerIndex);
        }


        /// <summary>
        /// Start the Asteroids game.
        /// </summary>
        void AsteroidsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new AsteroidGameScreen(), e.PlayerIndex);
        }


        /// <summary>
        /// Start the Tetris game.
        /// </summary>
        void TetrisMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new TetrisGameScreen(), e.PlayerIndex);
        }


        /// <summary>
        /// If the users chooses the "Back" menu entry, exit this screen.
        /// <summary>
        void BackMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            this.ExitScreen();
        }


        #endregion
    }
}
