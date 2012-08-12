using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerGames.FarseerXNAPhysics;
using FarseerGames.FarseerXNAPhysics.Dynamics;

namespace Portal2D.GameEngine.Physics
{
    class cRigidBodyCircleObject : cRigidBodyGameObject
    {
        public cRigidBodyCircleObject(float radius, int edgecount, float mass)
        {
            RigidBody = new CircleRigidBody(radius, edgecount, mass);
            cPhysics.Instance.addObject(this);
        }

        public cRigidBodyCircleObject(float radius, int edgecount, float mass, float collisionPrecisionFactor)
        {
            RigidBody = new CircleRigidBody(radius, edgecount, mass, collisionPrecisionFactor);
            cPhysics.Instance.addObject(this);
        }       
    }
}



