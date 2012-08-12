using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Portal2D.GameEngine.Physics;
using Portal2D.GameEngine.Sprites;
using System.Xml;
using System.Collections;
using FarseerGames.FarseerXNAPhysics.Collisions;
using Microsoft.Xna.Framework.Content;
using Portal2D.Font;

using Portal2D.GameEngine.Lights;

namespace Portal2D.Menus
{
    public struct keyboardControlData
    {
        public String textName;
        public String key;
        public int xpos;
        public int ypos;
        public int nWidth;
    };


    class cTextParser
    {

        public static BitmapFont _mainFont;
        string whatever;

        public cTextParser()
        {

            _mainFont = new BitmapFont(@"Resources/Fonts/Treb11.xml");

        }



        public List<keyboardControlData> getText(String XMLURL)
        {
            // TODO: We need to dump the graphics from a previously loaded
            // level before we load new ones, or instead when we load the
            // graphics below we check that they dont already exist.
            List<keyboardControlData> data = new List<keyboardControlData>();
            #region XMLParsing
            // If the XML document isnt in the correct format that we expect then
            // an XmlException will be thrown to alert the user to the fact.
            try
            {
                // Load the XML document into the reader
                XmlTextReader reader = new XmlTextReader(XMLURL);
                //Dictionary<String, objDef> spriteVerts = new Dictionary<String, objDef>();
                

                // Loop through all the elements
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        String elementName = reader.Name;
                        // If its our top-level elemennt
                        if (elementName == "controls")
                        {
                            if (reader.HasAttributes)
                            {
                                keyboardControlData temp = new keyboardControlData();

                                for (int i = 0; i < reader.AttributeCount; i++)
                                {                                    
                                    reader.MoveToAttribute(i);
                                    
                                    // Getting the properties of the sprite
                                    if (reader.Name == "x") { temp.xpos = int.Parse(reader.Value); }
                                    if (reader.Name == "y") { temp.ypos = int.Parse(reader.Value); }
                                    //if (reader.Name == "textName") { textName = reader.Value; }
                                    if (reader.Name == "key") { temp.key = reader.Value; }
                                    if (reader.Name == "format") { temp.nWidth = int.Parse(reader.Value); }
                                }
                                data.Add(temp);
                            }

                        }
                    }
                }
             
            }
            catch (XmlException e)
            {
                Console.WriteLine("error occured: " + e.Message);
            }
            #endregion

            return data;

        }

        
        public void update(GameTime gameTime)
        {

        }


        

        public void draw(SpriteBatch batch)
        {

        }
    }
}
