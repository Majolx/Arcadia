using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Arcadia.Screen;

namespace Arcadia.Gamestates.Menu
{
    class OptionsMenuScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.
            MenuEntry displayMenuEntry = new MenuEntry("Display");
            MenuEntry creditsEntry = new MenuEntry("Credits");
            MenuEntry backMenuEntry = new MenuEntry("Go Back");

            // Hook up menu event handlers.
            displayMenuEntry.Selected += DisplayMenuEntrySelected;
            creditsEntry.Selected += CreditsEntrySelected;
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(displayMenuEntry);
            MenuEntries.Add(creditsEntry);
            MenuEntries.Add(backMenuEntry);
        }


        #endregion

        #region Handle Input

        /// <summary>
        /// Show the Display menu.
        /// </summary>
        void DisplayMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new DisplayMenuScreen(), e.PlayerIndex);
        }


        void CreditsEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new CreditsScreen(), e.PlayerIndex);
        }
        /// <summary>
        /// Exit this screen.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            this.ExitScreen();
        }


        #endregion
    }
}
