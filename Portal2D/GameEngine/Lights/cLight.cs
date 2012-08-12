using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Portal2D.GameEngine.Scene;
using Portal2D.GameEngine.Sprites;

namespace Portal2D.GameEngine.Lights
{
    class cLight : cGameObject, iLightMapRenderable
    {
        protected cSprite _lightMap;
        private Random _random;

        public cLight()
        {
            _random = new Random();
        }

        public void setLightMap(string map)
        {
            _lightMap = new cSprite(map);     
        }
   
        public void drawLightMap(SpriteBatch batch)
        {
            _lightMap.Position = _sprite.Position;
            _lightMap.Orientation = _sprite.Orientation;
            _lightMap.Visible = _sprite.Visible;
            _lightMap.draw(batch);
        }
    }
}
