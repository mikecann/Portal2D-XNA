using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Portal2D.Font;

namespace Portal2D.Menus
{
    class cMenuTextButton : cMenuButton
    {
        private string _text;
        public BitmapFont _font;

        /// <summary>
        /// Very basic text button alot more could be added to this,
        /// but for now most of the properties such as the font are hard coded in
        /// </summary>
        /// <param name="image"></param>
        /// <param name="text"></param>
        public cMenuTextButton(string image, string text) : base(image)
        {
            _text = text;
            _font = cFontManager.Instance.getFont("Treb11"); 
        }

        public override void draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            Color tint = this.Tint;
            this.Tint = Color.White;
            base.draw(batch);
            this.Tint = tint;
            int nWidth = _font.MeasureString(_text);
            int x = (int)(this.Position.X - nWidth/2);
            int y = (int)(this.Position.Y + 1 - _font.LineHeight/2);
            _font.KernEnable = true;
            _font.DrawString(x, y, tint, _text, nWidth);
        }

    }
}
