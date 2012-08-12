using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

using FarseerGames.FarseerXNAPhysics.Mathematics;
using FarseerGames.FarseerXNAPhysics.Dynamics;
using FarseerGames.FarseerXNAPhysics.Collisions;
using Portal2D.GameEngine.Audio;

using Portal2D.GameEngine.Physics;
using Portal2D.GameEngine.Particles;

namespace Portal2D.GameEngine.Portals
{
    /// <summary>
    /// portal manager handles all portal creation and interaction.
    /// this class is SINGLETON!
    /// </summary>
    class cPortalManager
    {

        /// <summary>
        /// variables
        /// </summary>
        private static cPortalManager _instance;        // singleton class
        private cPortal[] _portals;

        public cPortal[] Portals
        {
            get { return _portals; }
        }

        // get the instance for this singleton class
        public static cPortalManager Instance
        {        
            get {  

                // if not instancated yet, do so now
                if (_instance == null) { new cPortalManager(); }
                return _instance;
            }           
        }

        /// <summary>
        /// constructor
        /// </summary>
        public cPortalManager()
        {
            // Singleton class
            _instance = this;

            // Setup the basic 
            _portals = new cPortal[2];
            _portals[0] = new cPortal();
            _portals[1] = new cPortal();
            _portals[0].PortalColor = new Color(230, 30, 30, 255);
            _portals[1].PortalColor = new Color(30, 230, 30, 255);
        }
             
    
        /// <summary>
        /// draw the portals and blob
        /// </summary>
        /// <param name="batch">the drawing object</param>
        public void draw(SpriteBatch batch)
        {
            // Draw the two portals      
            _portals[0].draw(batch);
            _portals[1].draw(batch);                
        }

        /// <summary>
        /// update the portals and the blob
        /// </summary>
        /// <param name="elapsed">the time elapsed since the previous frame</param>
        public void update(GameTime gameTime)
        {
            _portals[0].update(gameTime);
            _portals[1].update(gameTime);       
        }
      
        public int checkPortalCollisions(cRigidBodyGameObject obj)
        {
            AABB aabb = obj.RigidBody.Geometry.AABB;
            ContactList newContactList = new ContactList(10);
            newContactList.Clear();

            if (AABB.Intersect(aabb, _portals[0].RigidBody.Geometry.AABB))
            {
                //Console.WriteLine("COLLISION 0");  
                obj.RigidBody.Collide(_portals[0].RigidBody, newContactList);
                if (newContactList.Count != 0) { return 0; }
            }
            else if (AABB.Intersect(aabb, _portals[1].RigidBody.Geometry.AABB))
            {
                //Console.WriteLine("COLLISION 1");            
                obj.RigidBody.Collide(_portals[1].RigidBody, newContactList);
                if (newContactList.Count != 0) { return 1; }
            }
            else
            {
                //Console.WriteLine("-------");               
            }
            return -1;
        }           

        /// <summary>
        /// opens a portal at the given location. alternates 
        /// between the two portal, which one is to be opened next
        /// </summary>
        /// <param name="where">where to open the portal</param>
        /// <param name="normal">the normal of the slope to open it on</param>        
        public void openPortal(int number, Vector2 where, Vector2 normal, cRigidBodyGameObject what)
        {
            normal.Y = -normal.Y;
            _portals[number].attachTo(where, normal, what);
            _portals[number].Sprite.Visible = true;
            cAudio.Instance.play("portal");
            cParticleEngine.Instance.spawnParticleEffect("portal_open", where, normal*2);         
         
        }
        
    }
}
