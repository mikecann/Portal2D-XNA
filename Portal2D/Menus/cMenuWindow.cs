using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Portal2D.GameEngine.Sprites;
using Portal2D.Font;

namespace Portal2D.Menus
{
    class cMenuWindow : cSprite
    {
        private cMenuWindowButton _MenuWindowButton;
        private cMenuOptionsWindowButton _MenuOptionsWindowButton;
        public static BitmapFont _titleFont;
        public static BitmapFont _mainFont;
        public cTextParser _textParser;

        private static cMenu _menu;
        private cMenuWindow _instance;
       
        private cSprite _tQuickGameWindow;
        private cSprite _tQuickGameWindow2;
        private cSprite _tQuickGameHighlight;
        private cSprite _tOptionsWindow;
        private cSprite _tControlsWindow;
        private static Vector2 _pQuickGameWindow;
        private static Vector2 _pQuickGameHighlight1;
        private static Vector2 _pQuickGameHighlight2;
        private static Vector2 _pQuickGameHighlight3;
        private static Vector2 _pOptionsWindow;
        private static Vector2 _pControlsWindow;
        
        private int _screenWidth = 800;
        private int _screenHeight = 600;
        private int levelselect = 1;
        //int _optionsSelection = 2;
        private byte _windowTint = 128;
        private string[] _aLevel;
   

        //public void startMenus()
        //{
        //    _fadeInTimer = 0;
        //    _fadeIntro = true;
        //    _drawOptionsWindow = _drawQuickGameWindow = false;
        //    _globalAlpha = 0;
        //    cCamera.Instance.Position = new Vector2(0, 0);
        //    mouseState = new MouseState();
        //}

        public cMenuWindow(GraphicsDeviceManager gd, ContentManager cm)
        {
            _MenuWindowButton = new cMenuWindowButton(gd, cm);
            _MenuOptionsWindowButton = new cMenuOptionsWindowButton(gd, cm);
            _textParser = new cTextParser();
            //_button = new cMenuButton("Resources/Menu/button");
            _aLevel = new string[6];
            _aLevel[0] = "Training";
            _aLevel[1] = "Training2";
            _aLevel[2] = "Training3";
            //_quickgamebuttons = new List<cMenuButton>();
            //_optionsSelection = new List<cMenuWindow>();

            _titleFont = cFontManager.Instance.getFont("Treb8"); 
            _mainFont = cFontManager.Instance.getFont("Treb11"); 

            cSpriteManager.Instance.addTexture("Resources/Menu/quickgamewindow", "quick_game_window");
            _tQuickGameWindow = new cSprite("quick_game_window");
            cSpriteManager.Instance.addTexture("Resources/Menu/quickgamewindow2", "quick_game_window2");
            _tQuickGameWindow2 = new cSprite("quick_game_window2");
            cSpriteManager.Instance.addTexture("Resources/Menu/quickgamehighlight", "quick_game_highlight");
            _tQuickGameHighlight = new cSprite("quick_game_highlight");
            cSpriteManager.Instance.addTexture("Resources/Menu/optionswindow", "options_window");
            _tOptionsWindow = new cSprite("options_window");
            cSpriteManager.Instance.addTexture("Resources/Menu/controlswindow", "controls_window");
            _tControlsWindow = new cSprite("controls_window");

            
        }

        public void update(GameTime gameTime, int _quickgameSelection, int _optionsSelection, int _levelselect, cMenuButton button)
        {
            _MenuWindowButton.update(gameTime, _quickgameSelection, _levelselect, button);
            _MenuOptionsWindowButton.update(gameTime, _optionsSelection, button);
            

        }

        public void drawQuickGameWindow(SpriteBatch batch)
        {
            int nWidth = _titleFont.MeasureString("");
            _titleFont.KernEnable = false;
            _titleFont.DrawString(156, 164, "QUICK GAME", nWidth);
            _titleFont.DrawString(157, 164, "QUICK GAME", nWidth);
            _titleFont.DrawString(_tControlsWindow.Width / 2 + 279, _tControlsWindow.Height / 2 + 282, "Play Quick Game", nWidth);
            _titleFont.DrawString(_tControlsWindow.Width / 2 + 414, _tControlsWindow.Height / 2 + 282, "Cancel", nWidth);
            _pQuickGameWindow = new Vector2 (144 + _tQuickGameWindow.Width / 2, 150 + _tQuickGameWindow.Height / 2);
            _tQuickGameWindow.Position = _pQuickGameWindow;
            _tQuickGameWindow2.Position = _pQuickGameWindow;


            if (Portal2D.Instance._quickgameSelection == 1)
            {
                _titleFont.DrawString(_tControlsWindow.Width / 2 + 414, _tControlsWindow.Height / 2 + 230, "Next", nWidth);
                _tQuickGameWindow.Tint = new Color(255, 255, 255, _windowTint);
                _tQuickGameWindow.draw(batch);

                if (Mouse.GetState().X > _screenWidth / 2 - 229 && Mouse.GetState().X < _screenWidth / 2 - 54 && Mouse.GetState().Y > _screenHeight / 2 - 60 && Mouse.GetState().Y < _screenHeight / 2 + 48)
                {

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        Portal2D.Instance._levelselect = 1;
                    }

                }


                if (Mouse.GetState().X > _screenWidth / 2 - 41 && Mouse.GetState().X < _screenWidth / 2 + 134 && Mouse.GetState().Y > _screenHeight / 2 - 60 && Mouse.GetState().Y < _screenHeight / 2 + 48)
                {

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        Portal2D.Instance._levelselect = 2;
                    }

                }

                if (Mouse.GetState().X > _screenWidth / 2 + 147 && Mouse.GetState().X < _screenWidth / 2 + 322 && Mouse.GetState().Y > _screenHeight / 2 - 60 && Mouse.GetState().Y < _screenHeight / 2 + 48)
                {

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        Portal2D.Instance._levelselect = 3;
                    }

                }

                if (Portal2D.Instance._levelselect == 1)
                {
                    drawLevelSelect1(batch);
                    _MenuWindowButton.drawQuickGameOkbutton1(batch);

                }

                if (Portal2D.Instance._levelselect == 2)
                {
                    drawLevelSelect2(batch);
                    _MenuWindowButton.drawQuickGameOkbutton2(batch);
                }

                if (Portal2D.Instance._levelselect == 3)
                {
                    drawLevelSelect3(batch);
                    _MenuWindowButton.drawQuickGameOkbutton3(batch);
                }
            }
            else if (Portal2D.Instance._quickgameSelection == 2)
            {
                _tQuickGameWindow2.Tint = new Color(255, 255, 255, _windowTint);
                _tQuickGameWindow2.draw(batch);
                _titleFont.DrawString(_tControlsWindow.Width / 2 - 62, _tControlsWindow.Height / 2 + 230, "Previous", nWidth);
                _MenuWindowButton._tButton8.draw(batch);

                if (Mouse.GetState().X > _screenWidth / 2 - 229 && Mouse.GetState().X < _screenWidth / 2 - 54 && Mouse.GetState().Y > _screenHeight / 2 - 60 && Mouse.GetState().Y < _screenHeight / 2 + 48)
                {

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        Portal2D.Instance._levelselect = 1;
                    }

                }


                if (Mouse.GetState().X > _screenWidth / 2 - 41 && Mouse.GetState().X < _screenWidth / 2 + 134 && Mouse.GetState().Y > _screenHeight / 2 - 60 && Mouse.GetState().Y < _screenHeight / 2 + 48)
                {

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        Portal2D.Instance._levelselect = 2;
                    }

                }

                if (Mouse.GetState().X > _screenWidth / 2 + 147 && Mouse.GetState().X < _screenWidth / 2 + 322 && Mouse.GetState().Y > _screenHeight / 2 - 60 && Mouse.GetState().Y < _screenHeight / 2 + 48)
                {

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        Portal2D.Instance._levelselect = 3;
                    }

                }

                if (Portal2D.Instance._levelselect == 1)
                {
                    drawLevelSelect1(batch);
                    _MenuWindowButton.drawQuickGameOkbutton4(batch);

                }

                if (Portal2D.Instance._levelselect == 2)
                {
                    drawLevelSelect2(batch);
                    _MenuWindowButton.drawQuickGameOkbutton5(batch);
                }

                if (Portal2D.Instance._levelselect == 3)
                {
                    drawLevelSelect3(batch);
                    _MenuWindowButton.drawQuickGameOkbutton6(batch);
                }
            }
        }

        public void drawOptionsWindow(SpriteBatch batch)
        {
            
            int nWidth = _titleFont.MeasureString("");
            _titleFont.KernEnable = false;
            _mainFont.KernEnable = false;
            _titleFont.DrawString(198, 110, "OPTIONS", nWidth);
            _titleFont.DrawString(199, 110, "OPTIONS", nWidth);
            _pOptionsWindow = new Vector2(186 + _tOptionsWindow.Width / 2, 100 + _tOptionsWindow.Height / 2);
            _tOptionsWindow.Position = _pOptionsWindow;
            _tOptionsWindow.Tint = new Color(255, 255, 255, _windowTint);
            _MenuOptionsWindowButton.drawTabs(batch);
            _tOptionsWindow.draw(batch);

            _pControlsWindow = new Vector2(202 + _tControlsWindow.Width / 2, 165 + _tControlsWindow.Height / 2);
            _tControlsWindow.Position = _pControlsWindow;
            _tControlsWindow.Tint = new Color(255, 255, 255, _windowTint);
            _tControlsWindow.draw(batch);
            //_titleFont.DrawString(_tControlsWindow.Width / 2 - 37, _tControlsWindow.Height / 2 + 10, "Controls");
            //_titleFont.DrawString(_tControlsWindow.Width / 2 + 28, _tControlsWindow.Height / 2 + 10, "Sound", nWidth);
            //_titleFont.DrawString(_tControlsWindow.Width / 2 + 98, _tControlsWindow.Height / 2 + 10, "Display", nWidth);
            
            
            if (Portal2D.Instance._optionsSelection == 1)
            {
                _MenuOptionsWindowButton.drawTabs(batch);
                _titleFont.DrawString(_tControlsWindow.Width / 2 - 29, _tControlsWindow.Height / 2 + 45, "MOVEMENT", nWidth);
                _titleFont.DrawString(_tControlsWindow.Width / 2 + 229, _tControlsWindow.Height / 2 + 45, "KEY / BUTTON", nWidth);
                _mainFont.DrawString(_tControlsWindow.Width / 2 - 24, _tControlsWindow.Height / 2 + 65, "Move Left", nWidth);
                _mainFont.DrawString(_tControlsWindow.Width / 2 - 24, _tControlsWindow.Height / 2 + 85, "Move Right", nWidth);
                _mainFont.DrawString(_tControlsWindow.Width / 2 - 24, _tControlsWindow.Height / 2 + 105, "Jump", nWidth);
                _titleFont.DrawString(_tControlsWindow.Width / 2 - 29, _tControlsWindow.Height / 2 + 210, "INTERACTION", nWidth);
                _titleFont.DrawString(_tControlsWindow.Width / 2 + 229, _tControlsWindow.Height / 2 + 210, "KEY / BUTTON", nWidth);
                _mainFont.DrawString(_tControlsWindow.Width / 2 - 24, _tControlsWindow.Height / 2 + 230, "Open Portal One", nWidth);
                _mainFont.DrawString(_tControlsWindow.Width / 2 - 24, _tControlsWindow.Height / 2 + 250, "Open Portal Two", nWidth);
                _titleFont.DrawString(_tControlsWindow.Width / 2 + 219, _tControlsWindow.Height / 2 + 350, "OK", nWidth);
                _titleFont.DrawString(_tControlsWindow.Width / 2 + 299, _tControlsWindow.Height / 2 + 350, "Cancel", nWidth);
                _titleFont.DrawString(_tControlsWindow.Width / 2 + 379, _tControlsWindow.Height / 2 + 350, "Apply", nWidth);

                List<keyboardControlData> controlScript = _textParser.getText("Resources/Menu/text.xml");

                _mainFont.DrawString(controlScript[0].xpos, controlScript[0].ypos, controlScript[0].key);
                _mainFont.DrawString(controlScript[1].xpos, controlScript[1].ypos, controlScript[1].key);
                _mainFont.DrawString(controlScript[2].xpos, controlScript[2].ypos, controlScript[2].key);
                _mainFont.DrawString(controlScript[3].xpos, controlScript[3].ypos, controlScript[3].key);
                _mainFont.DrawString(controlScript[4].xpos, controlScript[4].ypos, controlScript[4].key);

                //_instance.drawTabs(batch);

                //Debug.WriteLine(Portal2D.Instance._optionsSelection);

            }
            else if (Portal2D.Instance._optionsSelection == 2)
            {
                _MenuOptionsWindowButton.drawTabs(batch);
                _titleFont.DrawString(_tControlsWindow.Width / 2 + 379, _tControlsWindow.Height / 2 + 350, "Apply", nWidth);
                //Debug.WriteLine(Portal2D.Instance._optionsSelection);
            }
            
        }

        public void drawLevelSelect1(SpriteBatch batch)
        {
            _pQuickGameHighlight1 = new Vector2(_screenWidth / 2 - 147, _screenHeight / 2 - 15);
            _tQuickGameHighlight.Position = _pQuickGameHighlight1;
            _tQuickGameHighlight.draw(batch);
        }

        public void drawLevelSelect2(SpriteBatch batch)
        {
            _pQuickGameHighlight2 = new Vector2(_screenWidth / 2 + 42, _screenHeight / 2 - 15);
            _tQuickGameHighlight.Position = _pQuickGameHighlight2;
            _tQuickGameHighlight.draw(batch);

        }

        public void drawLevelSelect3(SpriteBatch batch)
        {
            _pQuickGameHighlight3 = new Vector2(_screenWidth / 2 + 230, _screenHeight / 2 - 15);
            _tQuickGameHighlight.Position = _pQuickGameHighlight3;
            _tQuickGameHighlight.draw(batch);

        }


    }
}
