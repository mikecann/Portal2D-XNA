using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Portal2D.GameEngine.Lights
{
    interface iLightMapRenderable
    {
        void drawLightMap(SpriteBatch batch);
    }
}
