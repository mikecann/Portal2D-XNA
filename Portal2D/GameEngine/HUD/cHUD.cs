using System;
using System.Collections.Generic;
using System.Text;
using Portal2D.GameEngine.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Portal2D.GameEngine.Scene;
using Microsoft.Xna.Framework;
using Portal2D.Font;

namespace Portal2D.GameEngine.HUD
{ 
    class cHUD
    {
        private static cHUD _instance;
        public static cHUD Instance { get { return _instance; } }
        public BitmapFont _font;
        private cSprite _lives;
        private cSprite _hudBoarder;

        public cHUD()
        {
            _instance = this;
            cSpriteManager.Instance.addTexture(@"Resources/HUD/hud_lives", "hud_lives");
            _lives = new cSprite("hud_lives");
            _lives.ScreenFixed = true;
            _lives.Position = new Vector2((_lives.Width / 2) + 16, cCamera.Instance.WidthHeight.Y - (_lives.Height / 2) - 6);
            _font = cFontManager.Instance.getFont("Treb11");

            cSpriteManager.Instance.addTexture(@"Resources/HUD/hudboarder", "hudboarder");            
            _hudBoarder = new cSprite("hudboarder");
            _hudBoarder.Position = new Vector2(400, 300);
            _hudBoarder.ScreenFixed = true;
            
        }

        public void draw(SpriteBatch batch)
        {
            // Draw the hud boarder
            _hudBoarder.draw(batch);

            // Draw the lives part
            _lives.draw(batch);
            _font.Batch = batch;
            int len = _font.MeasureString(cPlayer.Instance.LivesRemain+"");
            _font.DrawString(_lives.Width, (int)(cCamera.Instance.WidthHeight.Y - 26), Color.Orange, cPlayer.Instance.LivesRemain+"", len);

            // Draw the time part
            int sec = cEngine.GameTimer/1000;
            int min = (sec / 60);
            string minnn = min+"";
            string seccc = (sec - (min * 60))+"";
            if (minnn.Length == 1) { minnn = "0" + min; }
            if (seccc.Length == 1) { seccc = "0" + seccc; }
            string time = minnn + ":" + seccc;            
            len = _font.MeasureString( time );
            _font.DrawString((int)(cCamera.Instance.WidthHeight.X / 2)-10, 6, Color.Orange, time, len);
        }

    }
}
