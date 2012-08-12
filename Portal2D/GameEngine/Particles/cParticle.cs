using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Portal2D.GameEngine.Physics;
using Portal2D.GameEngine.Sprites;
using Portal2D.GameEngine.Scene;
using Portal2D.GameEngine.Portals;
using Portal2D.GameEngine.Lights;

namespace Portal2D.GameEngine.Particles
{
    class cParticle : cGameObject, iLightMapRenderable
    {
        protected float _maxAge;
        protected float _age;
        protected float _initAlpha;
        protected float _finalAlpha;
        protected bool _disposed;
        protected cSprite _lightMap;
        protected Vector2 _velocity;

        public float MaxAge
        {
            get { return _maxAge; }
            set { _maxAge = value; }
        }

        public float InitialAlpha
        {
            get { return _initAlpha; }
            set { _initAlpha = value; }
        }

        public float FinalAlpha
        {
            get { return _finalAlpha; }
            set { _finalAlpha = value; }
        }

        public cParticle()
        {
            _disposed = false;
        }

        public void setLightMap(string map)
        {
            _lightMap = new cSprite(map);
            cLightMapManager.Instance.add(this);
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);
            _age += gameTime.ElapsedGameTime.Milliseconds;

            float aDiff = _initAlpha-_finalAlpha;
            float alpha = (aDiff / _maxAge) * _age;       
            _sprite.Tint = new Color(255,255,255,(byte)(1-alpha));

            // we need to die
            if (_age >= _maxAge)
            {
                cParticleEngine.Instance.kill(this);
            }
        }

        public void drawLightMap(SpriteBatch batch)
        {        
            _lightMap.Position = _sprite.Position;
            _lightMap.Tint = _sprite.Tint;
            _lightMap.Visible = _sprite.Visible;
            _lightMap.draw(batch);
        }

        public void Dispose() 
        {
            cLightMapManager.Instance.remove(this);
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            //subclasses can override incase they need to dispose of resources
            //otherwise do nothing.
            if (!_disposed)
            {
                if (disposing) { };
                _disposed = true;
            }            
        }

    }
}
