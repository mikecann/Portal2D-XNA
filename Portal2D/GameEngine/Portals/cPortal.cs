using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

using Portal2D.GameEngine.Physics;
using Portal2D.GameEngine.Sprites;
using Portal2D.GameEngine.Lights;

namespace Portal2D.GameEngine.Portals
{
    /// <summary>
    /// A portal
    /// </summary>
    class cPortal : cRigidBodyRectangleObject, iLightMapRenderable
    {
        private cRigidBodyGameObject _attachedTo;
        private Vector2 _normal;
        private cSprite _lightMap;
        private Color _portalColor;
        private bool _openingAnim;
        private static float OPEN_ANIM_LENGTH = 100f;
        private float _openAnimTime;

        public cRigidBodyGameObject AttachedToObject
        {
            get { return _attachedTo; }
        }

        public new Vector2 Normal
        {
            get { return _normal; }
        }

        public Color PortalColor
        {
            get { return _portalColor; }
            set { _portalColor = value; }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public cPortal() : base(10, 50, 1)
        {
            //RigidBody.CanCollide = false;
            RigidBody.IsStatic = true;
            //this.CanAttachPortalTo = false;
            //this.IsPortalTransparrent = false;
            _attachedTo = null;
            _openingAnim = false;

            cLightMapManager.Instance.add(this);

            // Make sure the textures are available to use
            cSpriteManager.Instance.addTexture(@"Resources/Sprites/portal", "portal");
            cSpriteManager.Instance.addTexture(@"Resources/LightMaps/portalLightMap", "portalLightMap");
            base.setStaticSprite("portal");
            _sprite.Visible = false;
            _lightMap = new cSprite("portalLightMap");
        }

        /// <summary>
        /// Attach to a point with a given normal.
        /// TODO: This will need to be more generic to allow for values other than 1
        ///         but for now it only needs 1.
        /// </summary>
        /// <param name="where">the point to attach at</param>
        /// <param name="normal">the normal of the point its attached at</param>
        public void attachTo(Vector2 where, Vector2 normal, cRigidBodyGameObject what)
        {
            float angle = (float)(Math.Atan2(-normal.Y, -normal.X));
            Orientation = angle;
            Position = where;
            _attachedTo = what;
            _normal = normal;

            _openingAnim = true;
            _openAnimTime = 0;  
        }

        public override void update(GameTime gameTime)
        {          
            base.update(gameTime);

            if (_openingAnim)
            {

                float ratio = _lightMap.Texture.Height / OPEN_ANIM_LENGTH;
                float ratio2 = 255 / OPEN_ANIM_LENGTH;
                float ratio3 = _sprite.Texture.Height / OPEN_ANIM_LENGTH;


                _lightMap.Height = (int)(ratio * _openAnimTime);
                _lightMap.Tint = new Color(255, (byte)(255 - (ratio2 * _openAnimTime)), (byte)(255 - (ratio2 * _openAnimTime)), 255);
                _sprite.Height = (int)(ratio3 * _openAnimTime);

                _openAnimTime += gameTime.ElapsedGameTime.Milliseconds;                
                if (_openAnimTime >= OPEN_ANIM_LENGTH)
                {
                    _lightMap.Tint = _portalColor;
                    _lightMap.Height = _lightMap.Texture.Height;
                    _sprite.Height = _sprite.Texture.Height;
                    _openingAnim = false;
                }
            }

            _lightMap.Orientation = this.Orientation;
            _lightMap.Visible = _sprite.Visible;       
        }       

        public void drawLightMap(SpriteBatch batch)
        {
            Vector2 pos = Normal * 50;
            _lightMap.Position = _sprite.Position+pos;
            _lightMap.draw(batch);       
        }
    }

}
