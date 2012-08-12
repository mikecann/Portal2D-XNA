using System;
using System.Collections.Generic;
using System.Text;
using Portal2D.GameEngine.Physics;
using Portal2D.GameEngine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Portal2D.GameEngine.Scene
{
    interface iShaderObject 
    {  
        void update(GameTime gameTime);
        void draw(cCustomSpriteBatch batch);
    }
}
