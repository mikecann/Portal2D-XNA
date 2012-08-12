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

using Portal2D.GameEngine;
using Portal2D.GameEngine.Scene;
using Portal2D.GameEngine.Input;
using Portal2D.GameEngine.Sprites;
using Portal2D.GameEngine.Physics;
using Portal2D.GameEngine.Portals;
using Portal2D.GameEngine.Audio;
using Portal2D.GameEngine.Particles;
using Portal2D.GameEngine.PostProcessing;
using Portal2D.GameEngine.Lights;

using Portal2D.Videos;
using Portal2D.Font;
#endregion

namespace Portal2D.Menus
{
    /// <summary>
    /// This class repersents the player object 
    /// </summary>
    class cMenu : iButtonClickListener
    {

        /// <summary>
        /// Variables
        /// </summary>
        GraphicsDeviceManager _graphics;
        private ContentManager _contentManager;
        //private cVideo _video;
        private cMenuWindow _MenuWindow;

        // Singleton
        private static cMenu _instance;
        public static cMenu Instance { get{return _instance;}  }

        private cSprite _tPortalLogo;
        private BitmapFont _bitmapFont;

        private cMenuButton _tNewGame;
        private cMenuButton _tContinue;
        private cMenuButton _tQuickGame;
        private cMenuButton _tOptions;
        private cMenuButton _tQuit;
        private cMenuButton button;

        private cSprite _tMousePointer;
        private static Vector2 _pPortalLogo;
    
        private static Vector2 _pMousePointer;

        byte num;
        int alphatime;
        bool play_video = true;

        private byte _globalAlpha;

        private int _screenWidth = 800;
        private int _screenHeight = 600;
        private int count;

        private const int FADE_IN_TIME = 1000;
        private int _fadeInTimer;
        private bool _fadeIntro;

        private List<cMenuButton> _buttons;

        MouseState mouseState;

        // TEMPTEMPTEMPTEMP -- These variables will need to change sonic


        public bool _drawOptionsWindow = false;
        public bool _drawQuickGameWindow = false;

        public cMenu(GraphicsDeviceManager gd, ContentManager cm, GameWindow gw)
        {
            _instance = this;
            _graphics = gd;
            _contentManager = cm;
            //_video = new cVideo(gw);
            
            _screenWidth = (int)cCamera.Instance.Bounds.Y;
            _screenHeight = (int)cCamera.Instance.Bounds.Z;
            _buttons = new List<cMenuButton>();
            _MenuWindow = new cMenuWindow(gd, cm);
     
            _bitmapFont = cFontManager.Instance.getFont("Treb11"); 
            //_bitmapFont.Reset(gd.GraphicsDevice);
            LoadGraphicsContent();
        }
                
        /// <summary>
        /// Load graphics 
        /// </summary>
        /// <param name="gd">the graphics device</param>
        private void LoadGraphicsContent()
        {
            GraphicsDevice gd = _graphics.GraphicsDevice;


            Color c = new Color(17, 194, 253);

            cSpriteManager.Instance.addTexture("Resources/Menu/PortalLogoHiRes", "portal_logo");
            _tPortalLogo = new cSprite("portal_logo");            
          
            cSpriteManager.Instance.addTexture("Resources/Menu/textnewgame", "new_game");
            _tNewGame = new cMenuButton("new_game");
            _tNewGame.setHoverOverColor(c);
            _tNewGame.setOnClickListener(this);
            _tNewGame.Position = new Vector2(800 / 5 - 73, 600 / 2+39);
            _buttons.Add(_tNewGame);

            cSpriteManager.Instance.addTexture("Resources/Menu/textcontinue", "continue");
            _tContinue = new cMenuButton("continue");
            _tContinue.setHoverOverColor(c);
            _tContinue.setOnClickListener(this);
            _tContinue.Position = new Vector2(800 / 5 - 73, 600 / 2 + 59);
            _buttons.Add(_tContinue);

            cSpriteManager.Instance.addTexture("Resources/Menu/textquickgame", "quick_game");
            _tQuickGame = new cMenuButton("quick_game");
            _tQuickGame.setHoverOverColor(c);
            _tQuickGame.setOnClickListener(this);
            _tQuickGame.Position = new Vector2(800 / 5 - 73, 600 / 2 + 79);
            _buttons.Add(_tQuickGame);

            cSpriteManager.Instance.addTexture("Resources/Menu/textoptions", "options");
            _tOptions = new cMenuButton("options");
            _tOptions.setHoverOverColor(c);
            _tOptions.setOnClickListener(this);
            _tOptions.Position = new Vector2(800 / 5 - 73, 600 / 2 + 99);
            _buttons.Add(_tOptions);

            cSpriteManager.Instance.addTexture("Resources/Menu/textquit", "quit");
            _tQuit = new cMenuButton("quit");
            _tQuit.setHoverOverColor(c);
            _tQuit.setOnClickListener(this);
            _tQuit.Position = new Vector2(800 / 5 - 73, 600 / 2 + 119);
            _buttons.Add(_tQuit);
                 
            cSpriteManager.Instance.addTexture("Resources/Menu/half-life2_pointer", "mouse_pointer");
            _tMousePointer = new cSprite("mouse_pointer");            
            
        }


        public void startMenus()
        {
            _fadeInTimer = 0;
            _fadeIntro = true;
            _drawOptionsWindow = _drawQuickGameWindow = false;
            _globalAlpha = 0;
            num = 0;
            cCamera.Instance.Position = new Vector2(0, 0);
            mouseState = new MouseState();
            
        }

        /// <summary>
        /// main update loop
        /// </summary>
        /// <param name="gameTime">game time object</param>
        //public void Update(GameTime gameTime, int _optionsSelection)
        //{
        public void Update(GameTime gameTime, int _quickgameSelection, int _optionsSelection, int _levelselect)
        {
            mouseState = Mouse.GetState();
            cCamera.Instance.Position = Vector2.Zero;

            if (_fadeIntro)
            {


                    _fadeInTimer += gameTime.ElapsedGameTime.Milliseconds;

                    float ratio = (255.0f / FADE_IN_TIME);

                    _globalAlpha = (byte)(_fadeInTimer * ratio);

                    if (_fadeInTimer >= FADE_IN_TIME)
                    {
                        _fadeIntro = false;
                        _globalAlpha = 255;
                    }

                    for (int i = 0; i < _buttons.Count; i++)
                    {
                        _buttons[i].Tint = new Color(255, 255, 255, _globalAlpha);

                    }

                }
                else
                {
                    for (int i = 0; i < _buttons.Count; i++)
                    {
                        _buttons[i].update(gameTime);
                    }
                }


                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    _graphics.ToString();
                }


                _MenuWindow.update(gameTime, _quickgameSelection, _optionsSelection, _levelselect, button);
            


                if (_fadeInTimer <= FADE_IN_TIME)
                {
                    num++;
                    alphatime = gameTime.ElapsedGameTime.Milliseconds;
                }
                if (num >= 192)
                {
                    num = 192;
                }
            


        }

        public void Draw(SpriteBatch batch)
        {
            GraphicsDevice gd = _graphics.GraphicsDevice;
            gd.Clear(Color.Black);

            Portal2D.Instance.launchVideo();

                
                for (count = 0; count < 100; count++)
                {
                    
                }
                if (count >= 100)
                {
                    //Debug.WriteLine(count);
                    batch.Begin();
                    drawLogo(batch);

                    for (int i = 0; i < _buttons.Count; i++)
                    {
                        _buttons[i].draw(batch);
                    }

                    // This is a quick and dirty solution, im too tired


                    batch.End();
                    batch.Begin();                 
                    if (_drawOptionsWindow) { _MenuWindow.drawOptionsWindow(batch); } //drawOptionsWindow(batch); }
                    if (_drawQuickGameWindow) { _MenuWindow.drawQuickGameWindow(batch); }
                    drawMousePointer(batch);
                    batch.End();
                }



                

            
        }      
            
        public void drawLogo(SpriteBatch batch)
        {

            _pPortalLogo = new Vector2(800 / 2 - 75, 600 / 3 + 19);
            _tPortalLogo.Position = _pPortalLogo;
            _tPortalLogo.Tint = new Color(255, 255, 255, num);
            _tPortalLogo.draw(batch);
        }

        public void onButtonClicked(cMenuButton button)
        {
            GraphicsDevice gd = _graphics.GraphicsDevice;
            if (button == _tNewGame)
            {
                Portal2D.Instance.launchGame("Training");

            }
            else if (button == _tContinue)
            {
                Portal2D.Instance.continueGame();

            }
            else if (button == _tOptions)
            {

                if (!_drawQuickGameWindow) { _drawOptionsWindow = !_drawOptionsWindow; }

            }
            else if (button == _tQuickGame)
            {
                //if (!_drawOptionsWindow) { _drawQuickGameWindow = !_drawQuickGameWindow; }
                _drawQuickGameWindow = !_drawQuickGameWindow;
            }
            else if (button == _tQuit)
            {
                Portal2D.Instance.Exit();
            }


           
            
        }

        //private void Exit()
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        public void drawMousePointer(SpriteBatch batch)
        {          
            if (_globalAlpha >= 255)
            {
                _pMousePointer = new Vector2(mouseState.X + 6, mouseState.Y +12);
                _tMousePointer.Tint = new Color(255, 255, 255);
                _tMousePointer.Position = new Vector2(cInput.Instance.Mouse.X + _tMousePointer.Width, cInput.Instance.Mouse.Y + _tMousePointer.Width);
                _tMousePointer.Position = _pMousePointer + cCamera.Instance.Position;
                _tMousePointer.draw(batch);
                
            }
        }

    }
}
