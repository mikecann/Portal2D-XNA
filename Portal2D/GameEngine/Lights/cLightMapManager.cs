using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Portal2D.GameEngine.Lights
{
    class cLightMapManager
    {
        private static cLightMapManager _instance;
        private List<iLightMapRenderable> _lightMapObjects;

        // get the instance for this singleton class
        public static cLightMapManager Instance
        {
            get
            {
                // if not instancated yet, do so now
                if (_instance == null) { new cLightMapManager(); }
                return _instance;
            }
        }

        public cLightMapManager()
        {
            _lightMapObjects = new List<iLightMapRenderable>();
            _instance = this;
        }

        public void add(iLightMapRenderable obj)
        {
            _lightMapObjects.Add(obj);
        }

        public void remove(iLightMapRenderable obj)
        {
            _lightMapObjects.Remove(obj);
        }

        public void draw(SpriteBatch batch)
        {
            for (int i = 0; i < _lightMapObjects.Count; i++)
            {
                _lightMapObjects[i].drawLightMap(batch);
            }
        }

    }
}
