using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Portal2D.GameEngine.Sprites;

using Portal2D.GameEngine.Input;

namespace Portal2D.Menus
{
    class cMenuButton : cSprite
    {
        private Color _hoverOverColor;
        private iButtonClickListener _clickListener;
        private bool _clicking;

       
        public cMenuButton(String image) : base(image)
        {
        }

        public override void update(GameTime gameTime)
        {
            if (!this.Visible) { return; }

            Vector2 mousePos = new Vector2(cInput.Instance.Mouse.X, cInput.Instance.Mouse.Y);
            bool inThis = withinThis(mousePos);

            if (inThis)
            {           
                Tint = _hoverOverColor;
            }
            else
            {
                Tint = new Color(255, 255, 255, 255);
            }

            if (cInput.Instance.Mouse.LeftButton == ButtonState.Pressed)
            {
                if (inThis && !_clicking && _clickListener!=null) 
                { 
                    _clickListener.onButtonClicked(this); 
                }
                _clicking = true;
            }
            else { _clicking = false; }

            base.update(gameTime);
        }

        private bool withinThis(Vector2 v)
        {
            if (v.X > Position.X - Width/2 && v.X < Position.X + Width/2)
            {
                if (v.Y > Position.Y - Height/2 && v.Y < Position.Y + Height/2) { return true; }
            }
            return false;
        }

        public void setHoverOverColor(Color c)
        {
            _hoverOverColor = c;
        }

        public void setOnClickListener(iButtonClickListener listener)
        {
            _clickListener = listener;
        }

    }
}
