using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Portal2D.GameEngine.Physics;
using Portal2D.GameEngine.Sprites;
using Portal2D.GameEngine.Scene;
using Portal2D.GameEngine.Portals;

namespace Portal2D.GameEngine.Particles
{
    class cParticleEngine
    {
        private static cParticleEngine _instance;        // singleton class
        private List<cEmitter> _emitters;
        private List<cParticle> _particles;
        private List<cParticleBlueprint> _particleBlueprints;

        // get the instance for this singleton class
        public static cParticleEngine Instance
        {
            get
            {
                // if not instancated yet, do so now
                if (_instance == null) { new cParticleEngine(); }
                return _instance;
            }
        }

        public cParticleEngine()
        {
            _instance = this;
            _emitters = new List<cEmitter>();
            _particles = new List<cParticle>();
            _particleBlueprints = new List<cParticleBlueprint>();

            // Ensure the correct graphics are loaded
            cSpriteManager.Instance.addTexture(@"Resources/Sprites/particle-blue", "particle_blue");
            cSpriteManager.Instance.addTexture(@"Resources/LightMaps/PointLight-30-30", "PointLight-30-30");
            cSpriteManager.Instance.addTexture(@"Resources/Sprites/part02", "particle_flare");
          

            init();
        }

        private void init()
        {     
            // Create a basic blueprint
            cParticleBlueprint pBp = new cParticleBlueprint();
            pBp._textureName = "particle_blue";
            pBp._name = "portal_open_blue";
            pBp._lightMapName = "PointLight-30-30";
            pBp._maxAge = 1000;
            pBp._initAlpha = 255;
            pBp._finalAlpha = 0;
            pBp._gravity = false;
            pBp._type = "fairy";
            _particleBlueprints.Add(pBp);

            pBp = new cParticleBlueprint();
            pBp._textureName = "particle_flare";
            pBp._name = "part_flare";
            pBp._lightMapName = "PointLight-30-30";
            pBp._maxAge = 1000;
            pBp._initAlpha = 255;
            pBp._finalAlpha = 0;
            pBp._gravity = false;
            pBp._type = "standard";
            _particleBlueprints.Add(pBp);  
        }

        public void spawnParticleEffect(string effectName, Vector2 pos, Vector2 vel)
        {
            if (effectName == "portal_open")
            {
                cEmitter e = new cEmitter();
                e.Position = pos;
                e.EmitParticle = "portal_open_blue";
                e.EmitRate = new Vector2(10, 20);
                e.MaxAge = 200;
                e.EmitVelocityMin = vel;
                e.EmitVelocityMax = vel;
                _emitters.Add(e);
            }

            if (effectName == "ObjectDeath")
            {
                Random r = new Random();
                Vector2 v = Vector2.Zero;
                for (int i = 0; i < 100; i++)
                {
                    v.X = r.Next(-1000,1000)/1000f;
                    v.Y = r.Next(-1000,1000)/1000f;
                    spawnParticle("part_flare", pos, v);
                }
            }


        }

        public void spawnParticle(string particleName, Vector2 where, Vector2 vel)
        {
            for (int i = 0; i < _particleBlueprints.Count; i++)
            {                
                if (_particleBlueprints[i]._name == particleName)
                {
                    cParticle p = _particleBlueprints[i].spawnInstance();
                    p.Position = where;
                    p.Velocity = vel;
                    _particles.Add(p);
                }
            }
        }

        public void kill(cParticle p)
        {   
            _particles.Remove(p);
            p.Dispose();
        }

        public void kill(cEmitter e)
        {
            _emitters.Remove(e);
            e.Dispose();
        }

        public void update(GameTime gameTime)
        {
            for (int i = 0; i < _emitters.Count; i++)
            {
                _emitters[i].update(gameTime);
            }

            for (int i = 0; i < _particles.Count; i++)
            {
                _particles[i].update(gameTime);
            }          
        }

        public void draw(SpriteBatch batch)
        {
            for (int i = 0; i < _particles.Count; i++)
            {
                _particles[i].draw(batch);
            } 
        }
    }
}
