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
using Portal2D.GameEngine.Scene;

namespace Portal2D.GameEngine.Physics
{
    /// <summary>
    /// This is basically a wrapper class for the simulator allowing static referencing
    /// </summary>
    class cPhysics
    {
        private static cPhysics _instance;        // singleton class
        private PhysicsSimulator _physicsSimulator;
        private List<cRigidBodyGameObject> _rigidBodies; // This will need to be more generic later, when we need non-rectangle objects
        private Vector2 _gravity;

        // get the instance for this singleton class
        public static cPhysics Instance
        {
            get
            {
                // if not instancated yet, do so now
                if (_instance == null) { new cPhysics(); }
                return _instance;
            }
        }

        public static PhysicsSimulator Simulator
        {
            get
            {
                return Instance._physicsSimulator;
            }
        }

        public Vector2 Gravity
        {
            get { return _gravity; }
            set { _gravity = value; }
        }

        public cPhysics()
        {
            _instance = this;

            _gravity = new Vector2(0, 9.81f);
        
            _physicsSimulator = new PhysicsSimulator(_gravity*18);
            //_physicsSimulator.AllowedPenetrations = 1; 
            //_physicsSimulator.BiasFactor = 0.1f;
            _rigidBodies = new List<cRigidBodyGameObject>();
        }

        public void update(GameTime gameTime)
        {
            _physicsSimulator.Update((float)gameTime.ElapsedGameTime.TotalSeconds);            
        }

        public void addObject(cRigidBodyGameObject obj)
        {
            _rigidBodies.Add(obj);
            _physicsSimulator.Add(obj.RigidBody);
        }

        public cRigidBodyGameObject getOwnerOf(RigidBody r)
        {
            for (int i = 0; i < _rigidBodies.Count; i++)
            {
                if (_rigidBodies[i].RigidBody.Equals(r)) { return _rigidBodies[i]; }
            }

            return null;
        }

        public List<cRayCollision> rayCollision(Vector2 a, Vector2 b)
        {          
            List<cRayCollision> collisions = new List<cRayCollision>();

            for (int i = 0; i < _rigidBodies.Count; i++)
            {
                RigidBody rb = _rigidBodies[i].RigidBody;
                Geometry geom = rb.Geometry;
                Vertices verts = geom.WorldVertices;

                for (int j = 0; j < verts.Count; j++)
                {
                    Vector2 c = verts[j];
                    Vector2 d;
                    if (j == verts.Count - 1) { d = verts[0]; }
                    else { d = verts[j + 1]; }

                    cRayCollision col = cRayUtils.intersects(a, b, c, d);
                    if (col.collision) { col.what = _rigidBodies[i]; collisions.Add(col); };      
                }               
            }

            return collisions;
        }

        public void removeObject(cRigidBodyGameObject obj)
        {
            _rigidBodies.Remove(obj);
            _physicsSimulator.Remove(obj.RigidBody);      
        }
    }
}
