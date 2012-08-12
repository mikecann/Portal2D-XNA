#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

using Portal2D.GameEngine.Scene;
using Portal2D.GameEngine.Input;
using Portal2D.GameEngine.Sprites;
using Portal2D.GameEngine.Physics;
using Portal2D.GameEngine.Portals;
using Portal2D.GameEngine.Audio;
using Portal2D.GameEngine.Particles;
using Portal2D.GameEngine.PostProcessing;
using Portal2D.GameEngine.Lights;
//using Portal2D.GameEngine.Effects;
using Portal2D.Font;

#endregion

namespace Portal2D.Menus
{
    /// <summary>
    /// This class repersents the player object 
    /// </summary>
    class cMenuOptionsWindowButton : iButtonClickListener
    {

        /// <summary>
        /// Variables
        /// </summary>
        GraphicsDeviceManager _graphics;
        private ContentManager _contentManager;
        public static BitmapFont _tabFont;
        public cMenuButton _tcontrolsTab;
        public cMenuButton _tdisplayTab;
        public cMenuButton _tcancelButton;
        public cMenuButton _tsoundTab;
        private int _screenWidth = 800;
        private int _screenHeight = 600;
        private MouseState mouseState;

        private static cMenuOptionsWindowButton _instance;
        public static cMenuOptionsWindowButton Instance { get { return _instance; } }

        private List<cMenuButton> _optionsbuttons;

        private byte _windowTint = 0;


        public cMenuOptionsWindowButton(GraphicsDeviceManager gd, ContentManager cm)
        {
            _instance = this;
            _graphics = gd;
            _contentManager = cm;
            mouseState = new MouseState();
            _tabFont = cFontManager.Instance.getFont("Treb8");
            _optionsbuttons = new List<cMenuButton>();

            cSpriteManager.Instance.addTexture("Resources/Menu/optionswindowtab", "controls_tab");
            _tcontrolsTab = new cMenuButton("controls_tab");
            _tcontrolsTab.setOnClickListener(this);
            _tcontrolsTab.Position = new Vector2(800 / 5 + 62, 600 / 2 - 157);
            _optionsbuttons.Add(_tcontrolsTab);
            cSpriteManager.Instance.addTexture("Resources/Menu/optionswindowtab", "sound_tab");
            _tsoundTab = new cMenuButton("sound_tab");
            _tsoundTab.setOnClickListener(this);
            _tsoundTab.Position = new Vector2(800 / 5 + 132, 600 / 2 - 157);
            _optionsbuttons.Add(_tsoundTab);
            cSpriteManager.Instance.addTexture("Resources/Menu/optionswindowtab", "display_tab");
            _tdisplayTab = new cMenuButton("display_tab");
            _tdisplayTab.setOnClickListener(this);
            _tdisplayTab.Position = new Vector2(800 / 5 + 202, 600 / 2 - 157);
            _optionsbuttons.Add(_tdisplayTab);
            cSpriteManager.Instance.addTexture("Resources/Menu/optionswindowtab", "cancel_button");
            _tcancelButton = new cMenuButton("cancel_button");
            _tcancelButton.setOnClickListener(this);
            _tcancelButton.Position = new Vector2(_screenWidth - 231, _screenHeight / 2 + 183);
            _optionsbuttons.Add(_tcancelButton);

        }

        public void update(GameTime gameTime, int _optionsSelection, cMenuButton button)
        {

            mouseState = Mouse.GetState();
            for( int i = 0; i < 4; i++)
            {
                _optionsbuttons[i].update(gameTime);
                _optionsbuttons[i].Tint = new Color(255, 255, 255, _windowTint);
            }

            onButtonClicked(button);
            _optionsSelection = Portal2D.Instance._optionsSelection;

        }

        public void onButtonClicked(cMenuButton button)
        {
            GraphicsDevice gd = _graphics.GraphicsDevice;



            if (button == _tcontrolsTab)
            {

                Portal2D.Instance._optionsSelection = 1;
                Debug.WriteLine(Portal2D.Instance._optionsSelection);

            }
            else if (button == _tsoundTab)
            {

                Portal2D.Instance._optionsSelection = 2;
                //Debug.WriteLine(Portal2D.Instance._optionsSelection);

            }
            else if (button == _tdisplayTab)
            {

                Portal2D.Instance._optionsSelection = 3;
                //Debug.WriteLine(Portal2D.Instance._optionsSelection);

            }
            else if (button == _tcancelButton)
            {

                cMenu.Instance._drawOptionsWindow = !cMenu.Instance._drawOptionsWindow;
                //Debug.WriteLine(Portal2D.Instance._optionsSelection);

            }

        }

        public void drawTabs(SpriteBatch batch)
        {
            int nWidth = _tabFont.MeasureString("");
            _tabFont.KernEnable = false;
            _tabFont.DrawString(478 / 2 - 38, 254 / 2 + 10, "Controls", nWidth);
            _tabFont.DrawString(478 / 2 + 28, 254 / 2 + 10, "Sound", nWidth);
            _tabFont.DrawString(478 / 2 + 98, 254 / 2 + 10, "Display", nWidth);

            for (int i = 0; i < _optionsbuttons.Count; i++)
            {
                _optionsbuttons[i].draw(batch);

            }

        }




    }
}

