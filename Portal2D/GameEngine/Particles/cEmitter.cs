using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Portal2D.GameEngine.Particles
{
    class cEmitter
    {
        private Vector2 _pos;
        private Vector2 _emitVelMin;
        private Vector2 _emitVelMax;
        private string _emitParticle;
        private Vector2 _emitRate;
        private long _nextEmitAt;
        private int _emitCount;
        private Random _random;
        private float _maxAge;
        private float _age;
        private bool _disposed;

        public Vector2 Position
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public Vector2 EmitVelocityMin
        {
            get { return _emitVelMin; }
            set { _emitVelMin = value; }
        }

        public Vector2 EmitVelocityMax
        {
            get { return _emitVelMax; }
            set { _emitVelMax = value; }
        }

        public string EmitParticle
        {
            get { return _emitParticle; }
            set { _emitParticle = value; }
        }

        public Vector2 EmitRate
        {
            get { return _emitRate; }
            set { _emitRate = value; }
        }

        public float MaxAge
        {
            get { return _maxAge; }
            set { _maxAge = value; }
        }
        
        public cEmitter()
        {
            _emitCount = 0;
            _nextEmitAt = 0;
            _random = new Random();
            _maxAge = 0;
            _age = 0;
        }

        public void update(GameTime gameTime)
        {
            _emitCount += gameTime.ElapsedGameTime.Milliseconds;
            _age += gameTime.ElapsedGameTime.Milliseconds;

            // If its time to emit 
            if (_emitCount >= _nextEmitAt)
            {
                float velx = _random.Next((int)_emitVelMin.X*1000, (int)_emitVelMax.X*1000)/1000f;
                float vely = _random.Next((int)_emitVelMin.Y*1000, (int)_emitVelMax.Y*1000)/1000f;

                Vector2 vel = new Vector2(velx, vely);
                cParticleEngine.Instance.spawnParticle(_emitParticle, _pos, vel);
                _nextEmitAt = _random.Next((int)_emitRate.X, (int)_emitRate.Y);
                _emitCount = 0;
            }

            if (_age > _maxAge)
            {
                cParticleEngine.Instance.kill(this);
            }
        }

        public void Dispose()
        {    
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
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
