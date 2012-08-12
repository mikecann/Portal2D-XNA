using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.DirectoryServices.ActiveDirectory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

using Portal2D.GameEngine;
using Portal2D.Menus;
using Portal2D.Videos;
using Portal2D.GameEngine.Sprites;
using Portal2D.GameEngine.Input;
using Portal2D.GameEngine.Audio;
using Portal2D.GameEngine.Scene;
using Portal2D.Font;

namespace Portal2D
{
    class Portal2D : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private ContentManager _contentManager;
        private cSpriteManager _spriteManager;
        private cInput _input;
        private cAudio _audio;      
        private SpriteBatch _batch;
        private cCamera _camera;
        private cFontManager _fontManager;

        private cEngine _engine;
        private cMenu _menu;
        private cVideo _video;
        

        private bool _gameRunning;
        private bool _videoRunning;

        private static Portal2D _instance;

        // This is probably bad practice, but sod it
        public static ContentManager CM { get { return _instance._contentManager; } }
        public static GraphicsDevice GD { get { return _instance._graphics.GraphicsDevice; } }
        public static Portal2D Instance { get { return _instance; } }
        public bool GameRunning { get { return _gameRunning; } set { _gameRunning = value; } }
        //private static GameWindow gw;

        public int _optionsSelection = 1;
        public int _quickgameSelection = 1;
        public int _levelselect = 1;

        public Portal2D() 
        {
            _instance = this;
            _spriteManager = new cSpriteManager();
            _graphics = new GraphicsDeviceManager(this);
            _contentManager = new ContentManager(Services);             
            _input = new cInput();
            _audio = new cAudio();
            _fontManager = new cFontManager();
            _fontManager.addFont(@"Resources/Fonts/Treb11.xml", "Treb11");
            _fontManager.addFont(@"Resources/Fonts/Treb8.xml", "Treb8");

            _gameRunning = false;
            _videoRunning = false;
         
            // Allow resizing - SET TO FLASE FOR NOW
            Window.AllowUserResizing = false;
            Window.ClientSizeChanged += new System.EventHandler(Window_ClientSizeChanged);
        }

        public void launchGame(String level)
        {
            _engine.loadLevel(level);
            _gameRunning = true;
        }

        public void continueGame()
        {
            _engine.continueGame();
            _gameRunning = true;
        }

        public void launchVideo()
        {
            _videoRunning = true;
        }

        protected override void Initialize()
        {
            _batch = new SpriteBatch(_graphics.GraphicsDevice);  
            base.Initialize();
        }
        
        protected override void LoadGraphicsContent(bool loadAllContent) 
        { 
            GraphicsDevice gd = _graphics.GraphicsDevice;
            cFontManager.Instance.LoadGraphicsContent(gd);

            if (loadAllContent)
            {
                _camera = new cCamera(0, 0, 800, 600); 

                _engine = new cEngine(_graphics, _contentManager);               
                _menu = new cMenu(_graphics, _contentManager, Window);
                _menu.startMenus();
                //_video = new cVideo(Window);
            }

            base.LoadGraphicsContent(loadAllContent);
        }
               
        protected override void Update(GameTime gameTime) 
        {
            // Allows the default game to exit on Xbox 360 and Windows
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            _camera.update(gameTime);
            _input.update(gameTime);
            _audio.update(gameTime);
            if (_gameRunning) { _engine.Update(gameTime); }
            else { _menu.Update(gameTime, _quickgameSelection, _optionsSelection, _levelselect); }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) 
        {
            if (_gameRunning) { _engine.Draw(_batch); }
            else
            {
                if (_videoRunning)
                {
                    //_video.playVideo(@"Resources/Video/helloworldintro.avi");
                    _videoRunning = false;
                }
                else
                {
                    //_video.mydispose();
                    _menu.Draw(_batch);
                }
            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// Callback function for window resize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            // Update the backbuffer
            _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;

            // Update the camera
            //cCameraManager.UpdateViewports(_graphics.GraphicsDevice.Viewport);
        }

    }
}
