#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Portal2D.GameEngine.Scene;
#endregion

namespace Portal2D.GameEngine.Sprites
{
    /// <summary>
    /// a simple 2d, non-animated sprite
    /// </summary>
    public class cSprite
    {
        /// <summary>
        /// variables
        /// </summary>
        protected Texture2D _texture;
        protected bool _hozFlip;
        protected bool _vertFlip;
        protected Vector2 _origin;
        protected Vector2 _pos;
        protected float _orientation;
        protected bool _visible;
        protected String _texName;
        protected Color _tint;
        protected int _width;
        protected int _height;
        protected bool _screenFixed;

        public Color Tint
        {
            get { return _tint; }
            set { _tint = value; }
        }

        public bool HorizontalFlip
        {
            get { return _hozFlip; }
            set { _hozFlip = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width=value; }  
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        public Vector2 Position
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public float Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        public bool ScreenFixed
        {
            get { return _screenFixed; }
            set { _screenFixed = value; }
        }

        public cSprite()
        {
            _hozFlip = false;
            _vertFlip = false;
            _origin = Vector2.Zero;
            _visible = true;
            _tint = new Color(255,255,255,255);
            _screenFixed = false;
        }

        public cSprite(String textureName)
        {
            _hozFlip = false;
            _vertFlip = false;
            _texName = textureName;
            _texture = cSpriteManager.Instance.getTexture(_texName).texture;
            _origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);
            _visible = true;            
            _tint = new Color(255, 255, 255, 255);
            _width = _texture.Width;
            _height = _texture.Height;
            _screenFixed = false;
        }      

        public virtual void draw(SpriteBatch batch)
        {
            if (_visible)
            {
                
                Vector2 cam = cCamera.Instance.Position;
                Rectangle r = new Rectangle((int)(_pos.X), (int)(_pos.Y), _width, _height); ;
                if (!_screenFixed) {r.X -= (int)cam.X; r.Y -= (int)cam.Y; }          
                
                SpriteEffects effects = SpriteEffects.None;                
                if (_hozFlip) { effects = SpriteEffects.FlipHorizontally; }
                batch.Draw(_texture, r, null, _tint, _orientation, _origin, effects, 0f);
            }
        }
    
        public virtual void update(GameTime gameTime)
        {
        }

        public cSprite clone()
        {
            cSprite s = new cSprite(_texName);
            s.HorizontalFlip = HorizontalFlip;
            s.Orientation = Orientation;
            s.Position = Position;
            s.Visible = Visible;
            return s;
        }
    }
}
