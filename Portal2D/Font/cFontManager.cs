using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;

namespace Portal2D.Font
{


    class cFnt
    {
        public string name;
        public string URL;
        public BitmapFont font;
    }

    class cFontManager
    {
        private List<cFnt> _fonts;
        private static cFontManager _instance;

        public static cFontManager Instance { get { return _instance; } }

        public cFontManager()
        {
            _instance = this;
            _fonts = new List<cFnt>();            
        }

        public void LoadGraphicsContent(GraphicsDevice gd)
        {
            for (int i = 0; i < _fonts.Count; i++)
            {
                _fonts[i].font = new BitmapFont(_fonts[i].URL);
                _fonts[i].font.Reset(gd);
            }
        }

        public void addFont(string URL, string name)
        {
            cFnt f = new cFnt();
            f.URL = URL;
            f.name = name;
            _fonts.Add(f);
        }

        public BitmapFont getFont(string name)
        {
            for (int i = 0; i < _fonts.Count; i++)
            {
                if (_fonts[i].name == name) { return _fonts[i].font; }
            }

            return null;
        }
    }
}
