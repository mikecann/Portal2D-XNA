using System;
using System.Collections.Generic;
using System.Text;
using Portal2D.GameEngine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Portal2D.GameEngine.Physics;

namespace Portal2D.GameEngine.Scene
{
    public class cGameObject
    {
        protected cSprite _sprite;
        protected bool _canAttachPortalTo;
        protected Vector2 _vel;
        protected bool _affectByGravity;

        public Vector2 Position { get { return _sprite.Position; } set { _sprite.Position=value; } }
        public Vector2 Velocity { get { return _vel; } set { _vel = value; } }
        public float Orientation { get { return _sprite.Orientation; } set { _sprite.Orientation = value; } }
        public bool AffectedByGravity { get { return _affectByGravity; } set { _affectByGravity = value; } }

        public cSprite Sprite
        {
            get { return _sprite; }
        }

        public bool CanAttachPortalTo
        {
            get { return _canAttachPortalTo; }
            set { _canAttachPortalTo = value; }
        }
    
        public void setStaticSprite(String name)
        {
            _sprite = new cSprite(name);
        }

        public void setAnimatedSprite(String name)
        {
            _sprite = new cAnimatedSprite(name);
        }

        public cGameObject()
        {
            _vel = Vector2.Zero;
            _affectByGravity = true;
        }

        public virtual void update(GameTime gameTime)
        {
            if (_affectByGravity) { Velocity += cPhysics.Instance.Gravity / 100; }
            Position += Velocity;
            if (_sprite != null) { _sprite.update(gameTime); }
        }

        public virtual void draw(SpriteBatch batch)
        {
            if (_sprite != null) { _sprite.draw(batch); }
        }
    }
}
