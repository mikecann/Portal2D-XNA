using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerGames.FarseerXNAPhysics;
using FarseerGames.FarseerXNAPhysics.Dynamics;
using Portal2D.GameEngine.Scene;
using FarseerGames.FarseerXNAPhysics.Collisions;
using Portal2D.GameEngine.Portals;
using Portal2D.GameEngine.Audio;
using Portal2D.GameEngine.Particles;

namespace Portal2D.GameEngine.Physics
{
    class cRigidBodyGameObject : cGameObject
    {
        protected RigidBody _rigidBody;
        protected bool _portaling;
        

        public RigidBody RigidBody
        {
            set { _rigidBody = value; }
            get { return _rigidBody; }
        }

        public new Vector2 Position
        {
            get { return _rigidBody.Position; }
            set
            {
                _rigidBody.Position = value;
                if (_sprite != null) { _sprite.Position = value; }
            }
            
        }

        public new Vector2 Velocity
        {
            get { return _rigidBody.LinearVelocity; }
            set { _rigidBody.LinearVelocity = value; _vel = value; }
        }

        public new float Orientation
        {
            get { return _rigidBody.Orientation; }
            set 
            { 
                _rigidBody.Orientation = value;
                if (_sprite != null) { _sprite.Orientation = value; }
            }
        }

        public float Width
        {
            get { return _sprite.Width; }
        }

        public float Height
        {
            get { return _sprite.Height; }
        }
                      
        public cRigidBodyGameObject() { _canAttachPortalTo = false; _portaling = false; }

        public cRigidBodyGameObject(float mass, Vertices v)
        {
            _rigidBody = new PolygonRigidBody(mass, v);          
            cPhysics.Instance.addObject(this);           
            _canAttachPortalTo = false;
            _portaling = false; 
        }

        public override void update(GameTime gameTime)
        {            
            _vel = _rigidBody.LinearVelocity;
            base.update(gameTime);
            if (_sprite != null)
            {
                _sprite.Position = Position;
                _sprite.Orientation = Orientation;                
            }

            if (!RigidBody.IsStatic && !_canAttachPortalTo)
            {
                checkPortalCollisions();
            }
        }

        private void checkPortalCollisions()
        {
            cPortal[] portals = cPortalManager.Instance.Portals;

            // Only check for portal collision if both portals are open
            if (portals[0].Sprite.Visible && portals[1].Sprite.Visible)
            {
                // Check for portal collision
                int collisionResult = cPortalManager.Instance.checkPortalCollisions(this);

                // If no collision we arent portaling and dont need to do anythin else
                if (collisionResult == -1) { _portaling = false; return; }

                // If we are portaling already then lets not do anythin (stops infinate loops)
                if (_portaling) { return; }

                // Get the to and from portals
                int portalFrom = collisionResult;
                int portalTo = 1;
                if (portalFrom == 1) { portalTo = 0; }

                Vector2 where = Position;
                where.X -= 10;
                

                // Calculate exit position and velocity
                Position = getPortalExitPosition(portals[portalTo]);
                RigidBody.LinearVelocity = getPortalExitVelocity(portals[portalFrom], portals[portalTo]);
                _portaling = true;

                where = Position;
                where.X -= 10;

                // Spawn effect of us leaving portal
                cParticleEngine.Instance.spawnParticleEffect("portal_open", Position-Velocity, Velocity);
                cAudio.Instance.play("Portaling");
            }

        }

        public Vector2 getPortalExitPosition(cPortal to)
        {
            Vector2 v = to.Position;
            Vector2 vn = to.Normal * 1.5f;
            float w = RigidBody.Geometry.AABB.Width;
            float h = RigidBody.Geometry.AABB.Height;
            Vector2 r = new Vector2((vn.X * w) / 2, (vn.Y * h) / 2);
            return v + r;
        }

        public Vector2 getPortalExitVelocity(cPortal from, cPortal to)
        {
            Vector2 vel = RigidBody.LinearVelocity;
            float tmp;

            if (from.Normal.X == 0)
            {
                if (to.Normal.X == 0)
                {
                    vel.Y = Math.Abs(vel.Y) * to.Normal.Y;
                }
                else
                {
                    tmp = vel.X;
                    vel.X = Math.Abs(vel.Y) * to.Normal.X;
                    vel.Y = tmp;
                }
            }
            else
            {
                if (to.Normal.X == 0)
                {
                    tmp = vel.Y;
                    vel.Y = Math.Abs(vel.X) * to.Normal.Y;
                    vel.X = tmp;
                }
                else
                {
                    vel.X = Math.Abs(vel.X) * to.Normal.X;
                }
            }

            return vel;
        }
    }
}
