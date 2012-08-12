using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Portal2D.GameEngine.Particles
{
    class cParticleBlueprint
    {
        public string _textureName;
        public string _name;
        public string _lightMapName;
        public float _maxAge;
        public float _initAlpha;
        public float _finalAlpha;
        public bool _gravity;
        public string _type;

        public cParticleBlueprint()
        {
            _initAlpha = 1;
            _finalAlpha = 1;
            _maxAge = 1000;
            _textureName = _lightMapName = "";
        }

        public cParticle spawnInstance()
        {
            cParticle p;
            if (_type == "fairy")
            {
                p = new cParticleFairy();
            }
            else
            {
                p = new cParticle();
            }
             
            p.InitialAlpha = _initAlpha;
            p.FinalAlpha = _finalAlpha;
            p.setStaticSprite(_textureName);
            p.setLightMap(_lightMapName);
            p.MaxAge = _maxAge;
            p.AffectedByGravity = _gravity;
            return p;
        }
    }
}
