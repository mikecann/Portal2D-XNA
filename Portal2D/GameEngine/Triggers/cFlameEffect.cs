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
using Portal2D.GameEngine.PostProcessing;

namespace Portal2D.GameEngine.Triggers
{
    class cFlameEffect : cRigidBodyRectangleObject, iShaderObject, iObjectCollisionListner
    {

        protected Texture2D _flameTex;
        protected Texture3D _noiseTex;
        protected Effect _effect;
        protected float _width;
        protected float _height;
        protected float _timeSeconds;

        public new float Width { get { return _width; } set { _width = value; } }
        public new float Height { get { return _height; } set { _height = value; } }

        public cFlameEffect(GraphicsDevice gd, ContentManager cm, int w, int h)
            : base(w, h, 1)
        {
            this.RigidBody.IsStatic = true;
            Width = w;
            Height = h;
            _effect = cm.Load<Effect>(@"Resources/Shaders/Fire/FlameEffect");
            _flameTex = cm.Load<Texture2D>(@"Resources/Textures/flame2");
            _noiseTex = Texture3D.FromFile(gd, @"Resources/Textures/NoiseVolume.dds");
            this.RigidBody.setCollisionListner(this, "fire");
        }
        
        public override void update(GameTime gameTime)
        {
            _timeSeconds += gameTime.ElapsedGameTime.Milliseconds/1000f;         
        }

        public void draw(cCustomSpriteBatch batch)
        {
             Vector2 cam = cCamera.Instance.Position;
             batch.Effect = _effect;
             batch.ResetMatrices(800, 600);
             batch.Effect.Parameters["Noise_Tex"].SetValue(_noiseTex);
             batch.Effect.Parameters["Flame_Tex"].SetValue(_flameTex);
             batch.Effect.Parameters["time"].SetValue(_timeSeconds);
             batch.Draw(new Rectangle((int)(Position.X - cam.X - Width / 2), (int)(Position.Y - cam.Y - Height / 2), (int)Width, (int)Height), Color.White);
             batch.Flush();
        }

        public void objectCollisionOccured(RigidBody rb, string command)
        {
            cRigidBodyGameObject obj = cPhysics.Instance.getOwnerOf(rb);
            if (obj == null)
            {
                Console.WriteLine("gettin null");
                return;
            }
            if (command == "fire")
            {

                if (obj.Equals(cPlayer.Instance))
                {
                    if (cPlayer.Instance.State == (int)cPlayer.STATES.DIEING) { return; }
                    cParticleEngine.Instance.spawnParticleEffect("ObjectDeath", obj.Position, Vector2.Zero);
                    cPlayer.Instance.respawn();
                    cPlayer.Instance.LivesRemain = cPlayer.Instance.LivesRemain - 1;
                    cPostProcessor.Instance.goBrightThenDark(3000);
                }
                else
                {
                    cParticleEngine.Instance.spawnParticleEffect("ObjectDeath", obj.Position, Vector2.Zero);
                    cParticleEngine.Instance.spawnParticleEffect("ObjectDeath", obj.Position, Vector2.Zero);
                    cPhysics.Instance.removeObject(obj);
                    cScene.Instance.removeSceneObject(obj);                
                    obj = null;
                }
            }          
        }
    }
}
