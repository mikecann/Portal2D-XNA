using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

using Portal2D.GameEngine.Physics;
using Portal2D.GameEngine.Input;
using Portal2D.GameEngine.Sprites;
using Portal2D.GameEngine.Scene;
using Portal2D.GameEngine.Portals;
using Portal2D.GameEngine.Audio;
using Portal2D.GameEngine.PostProcessing;
using Portal2D.GameEngine.Utils;

namespace Portal2D.GameEngine
{
    /// <summary>
    /// This class repersents the player object 
    /// </summary>
    class cPlayer : cRigidBodyRectangleObject
    {

        /// <summary>
        /// Variables
        /// </summary>
        private static cPlayer _instance;
        private int _state;
        private Vector2 _vel;
        //private cSprite _portalGunSprite;
        private cSprite _targeter;
        private bool _firing;
        private int _livesRemain;
        private Vector2 _spawnLocation;
        private const int MILLISECONDS_TO_DIE = 2000;
        private int _dieTimer;

        public int LivesRemain { get { return _livesRemain; } set { _livesRemain = value; } }
        public int State { get { return _state; } }

        /// <summary>
        /// The different states the player can be in
        /// </summary>
        public enum STATES
        {
            STANDING,
            WALKING,
            JUMPING,
            DIEING
        }

        // get the instance for this singleton class
        public static cPlayer Instance
        {
            get
            {
                // if not instancated yet, do so now
                if (_instance == null) { new cPlayer(); }
                return _instance;
            }
        }

        /// <summary>
        /// Constructor, player initially in standing state
        /// </summary>
        public cPlayer() : base(26,46,1)
        {
            _instance = this;
            Position = Vector2.Zero;             
            _state = (int)STATES.STANDING;
            _vel = Vector2.Zero;
     
            //RigidBody.IsPlayer = true;    
            //this.CanAttachPortalTo = false;
            //this.IsPortalTransparrent = true;

            // Make sure all the correct graphics get loaded
            cSpriteManager.Instance.addAnimation(@"Resources/Sprites/egon_anim.xml", "egon_anim");
            cEffectManager.Instance.defineEffect(@"Resources/Sprites/player_spawn_anim.xml", "player_spawn");
            //cSpriteManager.Instance.addTexture(@"Resources/Sprites/bazooka", "portal_gun");
            cSpriteManager.Instance.addTexture(@"Resources/Sprites/portalblob", "portal_target");
            base.setAnimatedSprite("egon_anim");
            ((cAnimatedSprite)_sprite).play("stand");
            _sprite.Origin = new Vector2(15, 23);
            //_portalGunSprite = new cSprite("portal_gun");
            _targeter = new cSprite("portal_target");    
        }      

        public void spawnIn(Vector2 where)
        {
            cEffectManager.Instance.spawnEffect("player_spawn", where);
            cAudio.Instance.play("Teleport");
            where.X += 10;
            Position = where;
            this.Sprite.Visible = true; 
            _spawnLocation = where;
        }

        /// <summary>
        /// main update loop
        /// </summary>
        /// <param name="gameTime">game time object</param>
        public override void update(GameTime gameTime)
        {
            // Stop the physics from trying to move us 
            // if we are on the ground
            if (RigidBody.OnGround)
            {
                RigidBody.LinearVelocity = new Vector2(0, 0);
            }
            else
            {
                cCamera.Instance.updateToFocusObject();
            }

            // Stop the physics from trying to rotate us
            this.RigidBody.AngularVelocity = 0;
            this.RigidBody.Orientation = 0;

            if (_state == (int)STATES.DIEING)
            {
                _dieTimer += gameTime.ElapsedGameTime.Milliseconds;      
                if (_dieTimer > MILLISECONDS_TO_DIE) { _state = (int)STATES.STANDING; spawnIn(_spawnLocation); }
            }
            else
            {
                // Any keys been pressed?
                updateInput();

                base.update(gameTime);
                // Update our guns
                updateWeapon();
            }
                                
        }

        /// <summary>
        /// Updates the currently selected wepon
        /// </summary>
        private void updateWeapon()
        {
            MouseState ms = cInput.Instance.Mouse;
            
            // Position the gun correctly
            Vector2 v = Position;
            v.Y += 6;
             

            // Rotate the gun to point at the targeter
            Vector2 mouseVec = new Vector2(ms.X, ms.Y);
            Vector2 fireVec = mouseVec - (this.Position - cCamera.Instance.Position);
            double angle = Math.Atan2(fireVec.Y, fireVec.X) + Math.PI / 2; ;
            angle = Math.Abs(angle);

            if (_state == (int)STATES.STANDING)
            {
                if (angle < Math.PI / 4) { ((cAnimatedSprite)_sprite).play("aim90"); }
                else if (angle < Math.PI / 2) { ((cAnimatedSprite)_sprite).play("aim45"); }
            }

            // Flip the worm if we are targeting in the opposite direction
            if (fireVec.X < 0) { _sprite.HorizontalFlip = true; } else { _sprite.HorizontalFlip = false; }

            // Make the targeter the mouse pointer
            _targeter.Position = mouseVec + cCamera.Instance.Position;
        }
                           

        /// <summary>
        /// Updates the keyboard and mouse input
        /// </summary>        
       
        private void updateInput()
        {
            MouseState ms = cInput.Instance.Mouse;
            KeyboardState ks = cInput.Instance.Keyboard;

            // Mouse states
            if (ms.LeftButton == ButtonState.Pressed) { fire(0); }
            else if (ms.RightButton == ButtonState.Pressed) { fire(1); }
            else { _firing = false; }

            // Checking for the key downs
            if (ks.IsKeyDown(Keys.A)) 
            {
                if (RigidBody.OnGround)
                {
                    ((cAnimatedSprite)_sprite).play("walk");
                    _state = (int)STATES.WALKING;
                }

                //_sprite.HorizontalFlip = true;                    
                move(new Vector2(-100, 0));
                cCamera.Instance.updateToFocusObject();
            }            
            
            
            if (ks.IsKeyDown(Keys.D)) 
            {
                if (RigidBody.OnGround)
                {
                    ((cAnimatedSprite)_sprite).play("walk");
                    _state = (int)STATES.WALKING; 
                }

                //_sprite.HorizontalFlip = false;                
                move(new Vector2(100, 0));
                cCamera.Instance.updateToFocusObject();                
            }

            if (!ks.IsKeyDown(Keys.A) && !ks.IsKeyDown(Keys.D))
            {
                if (RigidBody.OnGround)
                {
                    ((cAnimatedSprite)_sprite).play("stand");
                    _state = (int)STATES.STANDING;
                    _vel = Vector2.Zero;
                }
            }

            if (ks.IsKeyDown(Keys.W)) 
            { 
                if (RigidBody.OnGround) 
                { 
                    RigidBody.ApplyForce(new Vector2(0, -9000f));
                    _state = (int)STATES.JUMPING;
                    ((cAnimatedSprite)_sprite).play("jump");
                    cAudio.Instance.play("Backflip");
                }
                cCamera.Instance.updateToFocusObject();
            }            
        }

        /// <summary>
        /// Draws the player and crosshair
        /// </summary>
        /// <param name="batch">the drawing object</param>
        public override void draw(SpriteBatch batch)
        {
            base.draw(batch);
            //_crosshair.draw(batch);
            //_portalGunSprite.draw(batch);
            _targeter.draw(batch);
        }

        /// <summary>
        /// moves the player
        /// </summary>
        /// <param name="by">how much to move the player by</param>
        public void move(Vector2 by)
        {
            Vector2 v = RigidBody.LinearVelocity;
            if (RigidBody.OnGround)
            {                
                v.X = by.X;                
            }
            else
            {
                v.X += by.X / 100;
            }
            RigidBody.LinearVelocity = v;
            
        }
                   
        /// <summary>
        /// fires the blob-gun in the direction the target
        /// </summary>
        private void fire(int portalNumber)
        {    
            if (!_firing)
            {
                Vector2 fireVec = _targeter.Position - (this.Position);
                fireVec.Normalize();
                fireVec *= 1000;
                Vector2 va = this.Position;
                Vector2 vb = va + fireVec;

                List<cRayCollision> ojects = cPhysics.Instance.rayCollision(va, vb);
                cRayCollision closestCollision = null;
                float distance = float.MaxValue;

                for (int i = 0; i < ojects.Count; i++)
                {
                    cRayCollision c = ojects[i];

                    // TODO: Need to have surfaces that cant have portals opened on
                    if (c.what.CanAttachPortalTo)
                    {
                        Vector2 tmp = va - c.where;
                        float len = tmp.LengthSquared();
                        if (len < distance) { distance = len; closestCollision = c; }
                    }
                }

                if (closestCollision != null)
                {
                    cPortalManager.Instance.openPortal(portalNumber, closestCollision.where, closestCollision.normal, closestCollision.what);
                }

                _firing = true;
            }
        }

        public void respawn()
        {
            int play = cMath.RandRange(1, 5);
            Console.WriteLine(play);
            cAudio.Instance.play("Die"+play);
            cAudio.Instance.play("scream");
            _dieTimer = 0;
            this.Sprite.Visible = false;
            _state = (int)STATES.DIEING;
        }
    }
}
