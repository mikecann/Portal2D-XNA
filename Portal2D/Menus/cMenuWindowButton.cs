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
    class cMenuWindowButton : iButtonClickListener
    {

        /// <summary>
        /// Variables
        /// </summary>
        GraphicsDeviceManager _graphics;
        private ContentManager _contentManager;
        public static BitmapFont _tabFont;
        public cMenuButton _tButton1;
        public cMenuButton _tButton2;
        public cMenuButton _tButton3;
        public cMenuButton _tButton4;
        public cMenuButton _tButton5;
        public cMenuButton _tButton6;
        public cMenuButton _tButton7;
        public cMenuButton _tButton8;
        public cMenuButton _tButton9;
        private int _screenWidth = 800;
        private int _screenHeight = 600;
        private MouseState mouseState;
        private string[] _aLevel;

        private static cMenuWindowButton _instance;
        public static cMenuWindowButton Instance { get { return _instance; } }

        private List<cMenuButton> _quickgamebuttons;

        private byte _windowTint = 0;

        int count;


        public cMenuWindowButton(GraphicsDeviceManager gd, ContentManager cm)
        {
            _instance = this;
            _graphics = gd;
            _contentManager = cm;
            mouseState = new MouseState();
            _tabFont = cFontManager.Instance.getFont("Treb8");
            _aLevel = new string[6];
            _aLevel[0] = "Training";
            _aLevel[1] = "level flame";
            _aLevel[2] = "level platforms";
            _aLevel[3] = "Training";
            _aLevel[4] = "Training";
            _aLevel[5] = "Training";
            _quickgamebuttons = new List<cMenuButton>();

            cSpriteManager.Instance.addTexture("Resources/Menu/button2", "button1");
            _tButton1 = new cMenuButton("button1");
            _tButton1.setOnClickListener(this);
            _tButton1.Position = new Vector2(_screenWidth - 231, _screenHeight / 2 + 115);
            _quickgamebuttons.Add(_tButton1);
            cSpriteManager.Instance.addTexture("Resources/Menu/button2", "button2");
            _tButton2 = new cMenuButton("button2");
            _tButton2.setOnClickListener(this);
            _tButton2.Position = new Vector2(_screenWidth - 231, _screenHeight / 2 + 115);
            _quickgamebuttons.Add(_tButton2);
            cSpriteManager.Instance.addTexture("Resources/Menu/button2", "button3");
            _tButton3 = new cMenuButton("button3");
            _tButton3.setOnClickListener(this);
            _tButton3.Position = new Vector2(_screenWidth - 231, _screenHeight / 2 + 115);
            _quickgamebuttons.Add(_tButton3);
            cSpriteManager.Instance.addTexture("Resources/Menu/button1", "nextbutton");
            _tButton4 = new cMenuButton("nextbutton");
            _tButton4.setOnClickListener(this);
            _tButton4.Position = new Vector2(_screenWidth - 120, _screenHeight / 2 + 64);
            _quickgamebuttons.Add(_tButton4);
            cSpriteManager.Instance.addTexture("Resources/Menu/button2", "button5");
            _tButton5 = new cMenuButton("button5");
            _tButton5.setOnClickListener(this);
            _tButton5.Position = new Vector2(_screenWidth - 231, _screenHeight / 2 + 115);
            _quickgamebuttons.Add(_tButton5);
            cSpriteManager.Instance.addTexture("Resources/Menu/button2", "button6");
            _tButton6 = new cMenuButton("button6");
            _tButton6.setOnClickListener(this);
            _tButton6.Position = new Vector2(_screenWidth - 231, _screenHeight / 2 + 115);
            _quickgamebuttons.Add(_tButton6);
            cSpriteManager.Instance.addTexture("Resources/Menu/button2", "button7");
            _tButton7 = new cMenuButton("button7");
            _tButton7.setOnClickListener(this);
            _tButton7.Position = new Vector2(_screenWidth - 231, _screenHeight / 2 + 115);
            _quickgamebuttons.Add(_tButton7);
            cSpriteManager.Instance.addTexture("Resources/Menu/button1", "previousbutton");
            _tButton8 = new cMenuButton("previousbutton");
            _tButton8.setOnClickListener(this);
            _tButton8.Position = new Vector2(478 / 2 - 35, _screenHeight / 2 + 64);
            _quickgamebuttons.Add(_tButton8);
            cSpriteManager.Instance.addTexture("Resources/Menu/button1", "cancelbutton");
            _tButton9 = new cMenuButton("cancelbutton");
            _tButton9.setOnClickListener(this);
            _tButton9.Position = new Vector2(_screenWidth - 120, _screenHeight / 2 + 115);
            _quickgamebuttons.Add(_tButton9);


            

        }

        public void update(GameTime gameTime, int _quickgameSelection, int _levelSelect, cMenuButton button)
        {
            mouseState = Mouse.GetState();

                for (int i = 0; i <= 8; i++)
                {
                    _quickgamebuttons[i].update(gameTime);
                    _quickgamebuttons[i].Tint = new Color(255, 255, 255, _windowTint);
                }                
            

            //onButtonClicked(button);
            _levelSelect = Portal2D.Instance._levelselect;
            _quickgameSelection = Portal2D.Instance._quickgameSelection;

        }

        public void onButtonClicked(cMenuButton button)
        {
            GraphicsDevice gd = _graphics.GraphicsDevice;

            if (button == _tButton1)
            {
                //Portal2D.Instance.launchGame(_aLevel[0]);
                Portal2D.Instance.launchGame("Training");
                
            }
            else if (button == _tButton2)
            {
                Portal2D.Instance.launchGame(_aLevel[1]);
            }
            else if (button == _tButton3)
            {
                Portal2D.Instance.launchGame(_aLevel[2]);
            }
            else if (button == _tButton4)
            {
                Portal2D.Instance._quickgameSelection = 2;
                Portal2D.Instance._levelselect = 1;
            }
            else if (button == _tButton5)
            {
                Portal2D.Instance.launchGame(_aLevel[3]);
            }
            else if (button == _tButton6)
            {
                Portal2D.Instance.launchGame(_aLevel[4]);
            }
            else if (button == _tButton7)
            {
                Portal2D.Instance.launchGame(_aLevel[5]);
            }
            else if (button == _tButton8)
            {
                Portal2D.Instance._quickgameSelection = 1;
                Portal2D.Instance._levelselect = 1;
                //Portal2D.Instance.launchGame(_aLevel[5]);

            }
            else if (button == _tButton9)
            {

                    cMenu.Instance._drawQuickGameWindow = !cMenu.Instance._drawQuickGameWindow;
                
            }

        }

        public void drawQuickGameOkbutton1(SpriteBatch batch)
        {

            _quickgamebuttons[0].draw(batch);
            

        }
        public void drawQuickGameOkbutton2(SpriteBatch batch)
        {

            _quickgamebuttons[1].draw(batch);

        }
        public void drawQuickGameOkbutton3(SpriteBatch batch)
        {

            _quickgamebuttons[2].draw(batch);

        }

        public void drawQuickGameOkbutton4(SpriteBatch batch)
        {
            _quickgamebuttons[3].draw(batch);
        }

        public void drawQuickGameOkbutton5(SpriteBatch batch)
        {

            _quickgamebuttons[4].draw(batch);


        }
        public void drawQuickGameOkbutton6(SpriteBatch batch)
        {

            _quickgamebuttons[5].draw(batch);

        }
        public void drawQuickGameOkbutton7(SpriteBatch batch)
        {

            _quickgamebuttons[6].draw(batch);

        }







    }
}
