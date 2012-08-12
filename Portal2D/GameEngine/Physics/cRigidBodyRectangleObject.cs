using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerGames.FarseerXNAPhysics;
using FarseerGames.FarseerXNAPhysics.Dynamics;

namespace Portal2D.GameEngine.Physics
{
    class cRigidBodyRectangleObject : cRigidBodyGameObject
    {
        public cRigidBodyRectangleObject(float width, float height, float mass) {
            RigidBody = new RectangleRigidBody(width, height, mass);
            cPhysics.Instance.addObject(this);
        }

        public cRigidBodyRectangleObject(float width, float height, float mass, float collisionPrecisionFactor)
        {
            RigidBody = new RectangleRigidBody(width, height, mass, collisionPrecisionFactor);
            cPhysics.Instance.addObject(this);
        }        
    }
}
