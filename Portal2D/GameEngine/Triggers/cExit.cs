using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Portal2D.GameEngine.Physics;
using Portal2D.GameEngine.Sprites;
using Portal2D.GameEngine.Audio;
using Portal2D.GameEngine.Scene;
using FarseerGames.FarseerXNAPhysics;
using FarseerGames.FarseerXNAPhysics.Dynamics;
using Portal2D.GameEngine.Particles;

namespace Portal2D.GameEngine.Triggers
{
    class cExit : cRigidBodyRectangleObject, iObjectCollisionListner
    {
        private String _nextLevel;
        private String _audio;

        public cExit(String nextLevel, String audio) : base(10,60,1)     
        {        
            cSpriteManager.Instance.addTexture(@"Resources/Sprites/Exit", "Exit");
            this.setStaticSprite("Exit");
            this.RigidBody.IsStatic = true;
            this.RigidBody.setCollisionListner(this, "exit");
            _nextLevel = nextLevel;
            _audio = audio;
        }

        public void objectCollisionOccured(RigidBody rb, string command)
        {
            cRigidBodyGameObject obj = cPhysics.Instance.getOwnerOf(rb);

            if (command == "exit")
            {
                if (obj.Equals(cPlayer.Instance))
                {
                    cScene.Instance.NextLevel = _nextLevel;
                    cEngine.Instance.endLevel();
                    if (_audio != "") { cAudio.Instance.play(_audio); }

                    /*
                    if (cPlayer.Instance.State == (int)cPlayer.STATES.DIEING) { return; }
                    cParticleEngine.Instance.spawnParticleEffect("ObjectDeath", obj.Position, Vector2.Zero);
                    cPlayer.Instance.respawn();
                    cPlayer.Instance.LivesRemain = cPlayer.Instance.LivesRemain - 1;
                    */
                }
            }
        }
    }
}
