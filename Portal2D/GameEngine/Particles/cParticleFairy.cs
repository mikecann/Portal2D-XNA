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
    class cParticleFairy : cParticle
    {
        private Random _random;
        private Vector2 _dir;

        public new Vector2 Velocity
        {
            get { return _vel; }
            set { _dir = value;  }
        }

        public cParticleFairy()
        {
            _random = new Random();
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);
            _vel.X += _random.Next(-500, 500)/1000f;
            _vel.Y += _random.Next(-500, 500) / 1000f;
            if (_vel.X > 2) { _vel.X = 2; }
            if (_vel.Y > 2) { _vel.Y = 2; }
            if (_vel.X < -2) { _vel.X = -2; }
            if (_vel.Y < -2) { _vel.Y = -2; }

            _sprite.Visible = true;
            if (_random.Next(0, 10) == 5) { _sprite.Visible = false; }
        }     

    }
}
