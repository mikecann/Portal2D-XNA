#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using System.Xml;
#endregion

namespace Portal2D.GameEngine.Sprites
{

    public class cTexture
    {
        public String name;
        public Texture2D texture;
        public String URL;
    }

    public class cAnimatedTexture
    {
        public string name;
        public List<cAnimation> animations;
        public Texture2D texture;
        public String URL;
    }

    /// <summary>
    /// Structure holding an animation which can be composed of many frames
    /// </summary>
    public class cAnimation
    {
        public String loopType;
        public String name;
        public List<cFrame> frames;
    }

    /// <summary>
    /// Structure holding and individual frame
    /// </summary>
    public class cFrame
    {
        public int num;
        public int x;
        public int y;
        public int w;
        public int h;
        public int l;
    }

    class cSpriteManager
    {
        private List<cTexture> _textures;
        private List<cAnimatedTexture> _animatedTextures;
        private static cSpriteManager _instance;
        private Boolean _graphicsLoaded;

        // get the instance for this singleton class
        public static cSpriteManager Instance
        {
            get
            {

                // if not instancated yet, do so now
                if (_instance == null) { new cSpriteManager(); }
                return _instance;
            }
        }

        public cSpriteManager()
        {
            _instance = this;     
            _animatedTextures = new List<cAnimatedTexture>();
            _textures = new List<cTexture>();
            _graphicsLoaded = false;
        }
                     
        public cTexture getTexture(String name)
        {
            for (int i = 0; i < _textures.Count; i++)
            {
                if (_textures[i].name == name) { return _textures[i]; }
            }

            // TODO: Make this return an exception instead
            return new cTexture();
        }

        public cAnimatedTexture getAnimatedTexture(String name)
        {
            for (int i = 0; i < _animatedTextures.Count; i++)
            {
                if (_animatedTextures[i].name == name) { return _animatedTextures[i]; }
            }

            // TODO: Make this return an exception instead
            return new cAnimatedTexture();
        }

        public void addTexture(String URL, String name)
        {
            // Dont add a texture we already have
            for (int i = 0; i < _textures.Count; i++)
            {
                if (_textures[i].name == name) { return; }
            }

            cTexture tex = new cTexture();
            tex.name = name;
            tex.URL = URL;  
            tex.texture = Portal2D.CM.Load<Texture2D>(URL);
            _textures.Add(tex);
        }               

        public void addAnimation(String XMLURL, String name)
        {
            cAnimatedTexture tex = new cAnimatedTexture();
            tex.name = name;
            tex.URL = XMLURL;
            parseAnimation(tex, Portal2D.GD, Portal2D.CM);
            _animatedTextures.Add(tex);
        }

        private void parseAnimation(cAnimatedTexture anim, GraphicsDevice gd, ContentManager cm)
        {
            String textureURL = "";
            anim.animations = new List<cAnimation>();

            #region XMLParsing
            // If the XML document isnt in the correct format that we expect then
            // an XmlException will be thrown to alert the user to the fact.
            try
            {
                // Load the XML document into the reader
                XmlTextReader reader = new XmlTextReader(anim.URL);
                int currentAnim = -1;
                int frameLength = 10;

                // Loop through all the elements
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        String elementName = reader.Name;

                        // If its our top-level elemennt
                        if (elementName == "sprite")
                        {
                            // Get the URL for where to load the sprite
                            reader.MoveToAttribute(0);
                            if (reader.Name == "url") { anim.texture = cm.Load<Texture2D>(reader.Value); }
                        }

                        // If its an animation of the sprite
                        if (elementName == "animation")
                        {
                            cAnimation a = new cAnimation();
                            a.frames = new List<cFrame>();

                            // Load all the attributes for this animation
                            if (reader.HasAttributes)
                            {

                                // For all the attributed for this animation
                                for (int i = 0; i < reader.AttributeCount; i++)
                                {
                                    reader.MoveToAttribute(i);

                                    // Getting the name of the animation
                                    if (reader.Name == "name")
                                    {
                                        a.name = reader.Value;
                                    }

                                    // Getting its loop type
                                    if (reader.Name == "loop")
                                    {
                                        a.loopType = reader.Value;
                                    }

                                    // Getting the generic length of each frame
                                    if (reader.Name == "frameLength") { frameLength = Int32.Parse(reader.Value); }
                                }
                            }

                            currentAnim = anim.animations.Count;
                            anim.animations.Add(a);
                        }

                        // If its a frame of the animation
                        if (elementName == "frame")
                        {
                            cAnimation a = anim.animations[currentAnim];


                            // Creating a new frame object
                            cFrame curFrame = new cFrame();

                            // Setting the default attributed for the frame
                            curFrame.l = frameLength;

                            if (reader.HasAttributes)
                            {
                                for (int i = 0; i < reader.AttributeCount; i++)
                                {
                                    reader.MoveToAttribute(i);

                                    // Getting the protperties of the frame
                                    if (reader.Name == "number") { curFrame.num = Int32.Parse(reader.Value); }
                                    if (reader.Name == "x") { curFrame.x = Int32.Parse(reader.Value); }
                                    if (reader.Name == "y") { curFrame.y = Int32.Parse(reader.Value); }
                                    if (reader.Name == "w") { curFrame.w = Int32.Parse(reader.Value); }
                                    if (reader.Name == "h") { curFrame.h = Int32.Parse(reader.Value); }
                                    if (reader.Name == "l") { curFrame.l = Int32.Parse(reader.Value); }
                                }
                            }
                            a.frames.Add(curFrame);
                        }
                    }
                }
            }
            catch (XmlException e)
            {
                Console.WriteLine("error occured: " + e.Message);
            }
            #endregion

        }

    }
}
