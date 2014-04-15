using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Arcadia.Screen;
using Arcadia.Gamestates.Pong;

namespace Arcadia.Gamestates.Menu
{
    class DisplayMenuScreen : MenuScreen
    {
        #region Fields

        List<MenuEntry> displayMenuEntries = new List<MenuEntry>();
        List<DisplayMode> displayModes = new List<DisplayMode>();
        DisplayModeCollection modes;

        #endregion
        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public DisplayMenuScreen()
            : base("Display Options")
        {
            // Save the display modes
            this.modes = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes;

            // Create our menu entries.
            foreach (DisplayMode mode in modes)
            {
                if (mode.Format == GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Format)
                {
                    displayMenuEntries.Add(new MenuEntry(mode.Width + " x " + mode.Height));
                    displayModes.Add(mode);
                }
            }
            MenuEntry backMenuEntry = new MenuEntry("Back");

            // Hook up menu event handlers.
            foreach (MenuEntry entry in displayMenuEntries)
            {
                entry.Selected += DisplayMenuEntrySelected;
            }
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            foreach (MenuEntry entry in displayMenuEntries)
            {
                MenuEntries.Add(entry);
            }
            MenuEntries.Add(backMenuEntry);
        }


        #endregion

        #region Handle Input

        void DisplayMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            DisplayMode newMode = displayModes[this.SelectedEntry];
            ScreenManager.RequestedResolutionX = newMode.Width;
            ScreenManager.RequestedResolutionY = newMode.Height;
            ScreenManager.RequestedResolutionChange = true;
        }

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new PongScreen(), e.PlayerIndex);
        }


        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            this.ExitScreen();
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}
