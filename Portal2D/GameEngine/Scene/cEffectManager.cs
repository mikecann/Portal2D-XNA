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
using Portal2D.GameEngine.Audio;

namespace Portal2D.GameEngine.Scene
{
    class cEffectManager
    {
        private static cEffectManager _instance;
        private List<sEffect> _effectBlueprints;
        private List<sEffect> _activeEffects; 

        // get the instance for this singleton class
        public static cEffectManager Instance
        {
            get
            {
                // if not instancated yet, do so now
                if (_instance == null) { new cEffectManager(); }
                return _instance;
            }
        }

        private struct sEffect
        {         
            public cAnimatedSprite sprite;
            public string name;    
            
            public sEffect clone() 
            { 
                sEffect e = new sEffect(); 
                e.sprite = sprite.clone();
                e.name = name;                
                return e;
            }
        }


        public cEffectManager()
        {
            _instance = this;
            _effectBlueprints = new List<sEffect>();
            _activeEffects = new List<sEffect>();
        }

        public void defineEffect(String aminationXMLURL, String name)
        {
            cSpriteManager.Instance.addAnimation(aminationXMLURL, name);
            sEffect e = new sEffect();
            
            e.sprite = new cAnimatedSprite(name);
            e.name = name;
            _effectBlueprints.Add(e);
        }

        public void spawnEffect(String what, Vector2 where)
        {       

            for (int i = 0; i < _effectBlueprints.Count; i++)
            {
                if (_effectBlueprints[i].name == what)
                {
                    sEffect e = _effectBlueprints[i].clone();
                    e.sprite.Position = where;
                    e.sprite.play("main");                    
                    _activeEffects.Add(e);
                }
            }
        }

        public void update(GameTime gameTime)
        {
            for (int i = 0; i < _activeEffects.Count; i++)
            {
                _activeEffects[i].sprite.update(gameTime);
                if (_activeEffects[i].sprite.IsStopped) { _activeEffects.Remove(_activeEffects[i]); }
            }
        }

        public void draw(SpriteBatch batch)
        {
            for (int i = 0; i < _activeEffects.Count; i++)
            {
                _activeEffects[i].sprite.draw(batch);
            }
        }
    }
}
