using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

using FarseerGames.FarseerXNAPhysics;
using FarseerGames.FarseerXNAPhysics.Mathematics;
using FarseerGames.FarseerXNAPhysics.Dynamics;
using FarseerGames.FarseerXNAPhysics.Collisions;

namespace Portal2D.GameEngine.Physics
{
    class cRayCollision
    {
        public bool collision;
        public Vector2 where;
        public cRigidBodyGameObject what;
        public Vector2 normal;
        public Vector2 c;
        public Vector2 d;
  
        public cRayCollision()
        {
            collision = false;
            where = new Vector2(0,0);
            normal = new Vector2(0, 0);
            c = new Vector2(0, 0);
            d = new Vector2(0, 0);
            what = null;
        }      
    }
}
