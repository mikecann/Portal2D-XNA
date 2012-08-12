#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System.Xml;
using System.Collections;
using Portal2D.GameEngine.Scene;
#endregion

namespace Portal2D.GameEngine.Sprites
{
    /// <summary>
    /// contains methods for loading an animated sprite from and XML document 
    /// and then rendering it to the screen at a given location.
    /// </summary>
    class cAnimatedSprite : cSprite
    {
        /// <summary>
        /// Method variables
        /// </summary>
               
        private bool _stopped;        
        private double _time;
        private int _curFrame;
        private int _animationIncrement;
        private cAnimation _currentAnimation;
        private cFrame _currentFrame;

        private cAnimatedTexture _animationData;

        public bool IsStopped
        {
            get { return _stopped; }
            set { _stopped = value; }
        }
        
   
        /// <summary>
        /// Creates a new animated sprite
        /// </summary>
        /// <param name="gd">GraphicsDevice the graphics device with which to use to load the texture</param>
        /// <param name="XMLURL">String the location to the XML document which describes this sprite</param>
        public cAnimatedSprite(String animationName) : base()
        {
            _curFrame = 0;
            _animationIncrement = 1;
            _texName = animationName;
            _animationData = cSpriteManager.Instance.getAnimatedTexture(_texName);
            _texture = _animationData.texture;
            _stopped = true;         
        }    

        /// <summary>
        /// Plays a given animation. If the animation is already playing it returns doing nothing.
        /// If there is no animation by that name it does nothing.
        /// </summary>
        /// <param name="animationName">the name of the animation to play</param>                
        public void play(String animationName)
        {
            for (int i = 0; i < _animationData.animations.Count; i++)
            {
                cAnimation a = _animationData.animations[i];
      
                if (a.name == animationName)
                {
                    if (_currentAnimation != null)
                    {
                        if (_currentAnimation.Equals(a)) { return; } // if we are already playing this animation dont do anything
                    }

                    // Else we need to reset the variables
                    _currentAnimation = a;
                    _time = 0;
                    _currentFrame = _currentAnimation.frames[0];
                    _curFrame = 0;
                    _stopped = false;                                
                }
            }
        }
        
        /// <summary>
        /// Standard update loop, only updates if !_stopped
        /// </summary>
        /// <param name="elapsed">elapsed frame time</param>        
        public override void update(GameTime gameTime)
        {
            if (!_stopped)
            {
                _time += gameTime.ElapsedGameTime.Milliseconds;

                if (_time > _currentFrame.l)
                {
                    _curFrame += _animationIncrement;

                    if (_curFrame >= _currentAnimation.frames.Count || _curFrame<0)
                    {
                        if (_currentAnimation.loopType == "repeat") { _curFrame = 0; }
                        if (_currentAnimation.loopType == "reverse") { _animationIncrement = -_animationIncrement; _curFrame += _animationIncrement; }
                        if (_currentAnimation.loopType == "none") { _stopped = true; _curFrame -= _animationIncrement; }                         
                    }

                    _currentFrame = _currentAnimation.frames[_curFrame];
                    _time = 0;
                }
            }
        }
   
        /// <summary>
        /// Standard draw loop only draws if _visible
        /// </summary>
        /// <param name="batch">the batch drawer</param>
        /// <param name="where">where on the scren to draw this sprite (top-left corner) </param>
        public override void draw(SpriteBatch batch)
        {
            if (_visible && _currentFrame!=null)
            {     
                Vector2 cam = cCamera.Instance.Position;          
                Rectangle where = new Rectangle((int)(_pos.X), (int)(_pos.Y), _currentFrame.w, _currentFrame.h);
                if (!_screenFixed) { where.X -= (int)cam.X; where.Y -= (int)cam.Y; }
                Rectangle what = new Rectangle(_currentFrame.x, _currentFrame.y, _currentFrame.w, _currentFrame.h);
                SpriteEffects effects = SpriteEffects.None;
                if (_hozFlip) { effects = SpriteEffects.FlipHorizontally; }

                batch.Draw(_texture, where, what, _tint, _orientation, _origin, effects, 0f);
            }    
        }

        public cAnimatedSprite clone()
        {
            cAnimatedSprite s = new cAnimatedSprite(_texName);
            s.HorizontalFlip = HorizontalFlip;
            s.Orientation = Orientation;
            s.Position = Position;
            s.Visible = Visible;
            return s;
        }

    }   
}
