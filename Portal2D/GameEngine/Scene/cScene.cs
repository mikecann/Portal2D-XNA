using System;
using System.IO;
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
using FarseerGames.FarseerXNAPhysics;

using Portal2D.GameEngine.Lights;
using Portal2D.Font;
using Portal2D.GameEngine.Input;
using Microsoft.Xna.Framework.Input;
using Portal2D.Menus;

using Portal2D.GameEngine.Particles;
using FarseerGames.FarseerXNAPhysics.Dynamics;
using Portal2D.GameEngine.Triggers;




namespace Portal2D.GameEngine.Scene
{
    class cScene : iButtonClickListener
    {
        public cSprite _paralaxBack;
        //public cSprite _paralaxFront; 
        private List<cGameObject> _sceneObjects;
        private List<iShaderObject> _shaderObjects;

        // Variables used to draw the level description panel thing
        private string _currentLevelName;
        private string[] _currentLevelDescription;
        private BitmapFont _bitmapFont;
        private cSprite _blankWindow;
        private cSprite _mousePointer;
        private cMenuTextButton _startButton;
        private cMenuTextButton _continueButton;
        private String _nextLevel;
        private static cScene _instance;
        private int _levelFastestTime;

        public String NextLevel { get { return _nextLevel; } set { _nextLevel = value; } }
        public static cScene Instance { get { return _instance; } }
       
        public cScene()
        {   
            _instance = this;
            _sceneObjects = new List<cGameObject>();
            _shaderObjects = new List<iShaderObject>();
            _bitmapFont = cFontManager.Instance.getFont("Treb11");
            cSpriteManager.Instance.addTexture(@"Resources/HUD/descripwindow", "descripwindow");
            _blankWindow = new cSprite("descripwindow");
            _blankWindow.ScreenFixed = true;
            cSpriteManager.Instance.addTexture("Resources/Menu/half-life2_pointer", "mouse_pointer");
            _mousePointer = new cSprite("mouse_pointer");
            _mousePointer.ScreenFixed = true;
            cSpriteManager.Instance.addTexture("Resources/Menu/button1", "button1");
            _startButton = new cMenuTextButton("button1", "Start");
            _startButton.setHoverOverColor(new Color(17, 194, 253));
            _startButton.setOnClickListener(this);
            _startButton.ScreenFixed = true;      
      
            _continueButton = new cMenuTextButton("button1", "Continue");
            _continueButton.setHoverOverColor(new Color(17, 194, 253));
            _continueButton.setOnClickListener(this);
            _continueButton.ScreenFixed = true;

            
        }

        private struct objDef
        {
            public Vertices v;
            public float mass;            
            public float friction;
            public float restitution;
        };

        public void removeSceneObject(cGameObject obj)
        {
            _sceneObjects.Remove(obj);
        }

        public void loadLevel(String levelName)
        {
            // TODO: We need to dump the graphics from a previously loaded
            // level before we load new ones, or instead when we load the
            // graphics below we check that they dont already exist.

            #region XMLParsing
            // If the XML document isnt in the correct format that we expect then
            // an XmlException will be thrown to alert the user to the fact.
            try
            {
                // Load the XML document into the reader
                XmlTextReader reader = new XmlTextReader(@"Resources/Levels/"+levelName+".xml");
                Dictionary<String, objDef> spriteVerts = new Dictionary<String, objDef>();                                
                
                // Loop through all the elements
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        String elementName = reader.Name;
                        // If its our top-level elemennt
                        if (elementName == "level")
                        {
                            for (int i = 0; i < reader.AttributeCount; i++)
                            {
                                reader.MoveToAttribute(i);

                                if (reader.Name == "type")
                                {
                                    if (reader.Value == "lab")
                                    {
                                        // Load the parallax graphics
                                        cSpriteManager.Instance.addTexture("Resources/Textures/wall01-800-600", "parallax_lab_back");
                                        //cSpriteManager.Instance.addTexture(gd, "Resources/floor.bmp", "parallax_lab_front");                                  

                                        // Create new sprites
                                        _paralaxBack = new cSprite("parallax_lab_back");
                                        //_paralaxFront = new cSprite("parallax_lab_front");        
                                    }
                                }

                                if (reader.Name == "name")
                                {
                                    _currentLevelName = reader.Value;
                                }                                
                            }
                        }

                        if (elementName == "description")
                        {
                            string str = reader.ReadInnerXml();
                            str = str.Replace("\r","");
                            str = str.Replace("\t","");                            
                            string[] s = { "\n" };
                            _currentLevelDescription = str.Split(s, StringSplitOptions.RemoveEmptyEntries);
                        }
                       
                        // If its a frame of the animation
                        if (elementName == "sprite")
                        {
                            String spriteURL = "";
                            String type = "";
                            String collisionType = "";
                            String spriteName = "";
                            bool spritecollision = false;
                            int width = 0;
                            int height = 0;
                            float mass = 1;
                            float friction = 1;
                            float restitution = 0;
                           
                            if (reader.HasAttributes)
                            {                      

                                for (int i = 0; i < reader.AttributeCount; i++)
                                {
                                    reader.MoveToAttribute(i);

                                    // Getting the properties of the sprite
                                    if (reader.Name == "url") { spriteURL += reader.Value; }
                                    if (reader.Name == "type") { type = reader.Value; }
                                    if (reader.Name == "collision") { collisionType = reader.Value; }
                                    if (reader.Name == "w") { width = Int32.Parse(reader.Value); }
                                    if (reader.Name == "h") { height = Int32.Parse(reader.Value); }
                                    if (reader.Name == "mass") { mass = float.Parse(reader.Value); }
                                    if (reader.Name == "friction") { mass = float.Parse(reader.Value); }
                                    if (reader.Name == "restitution") { mass = float.Parse(reader.Value); }
                                    if (reader.Name == "name") { spriteName = reader.Value; }                                                 
                                }
                            }

                            if (type == "static") { cSpriteManager.Instance.addTexture(spriteURL, spriteName); }
                            if (collisionType == "rectangle")
                            {                                
                                Vertices v = Vertices.CreateRectangle(width,height);
                                objDef od = new objDef();
                                od.v = v;
                                od.mass = mass;
                                od.friction = friction;
                                od.restitution = restitution;                              
                                spriteVerts.Add(spriteName, od);
                            }
                        }

                        // If its a frame of the animation
                        if (elementName == "obj")
                        {       
                            String type = "";                            
                            String resource = "";            
                            int x = 0;
                            int y = 0;                
                            float orientation = 0;
                            bool canAttachPortalTo = false;

                            if (reader.HasAttributes)
                            {
                                for (int i = 0; i < reader.AttributeCount; i++)
                                {
                                    reader.MoveToAttribute(i);

                                    // Getting the properties of the object                          
                                    if (reader.Name == "type") { type = reader.Value; }       
                                    if (reader.Name == "x") { x = Int32.Parse(reader.Value); }
                                    if (reader.Name == "y") { y = Int32.Parse(reader.Value); }    
                                    if (reader.Name == "orientation") { orientation = float.Parse(reader.Value); }
                                    if (reader.Name == "resource") { resource = reader.Value; }
                                    if (reader.Name == "canAttachPortalTo") { canAttachPortalTo = bool.Parse(reader.Value); }
                                }
                            }


                            objDef od = spriteVerts[resource];            
                            
                            cRigidBodyGameObject obj = new cRigidBodyGameObject(od.mass,od.v);
                            if (type == "static") { obj.RigidBody.IsStatic = true; }
                            obj.setStaticSprite(resource);
                            obj.Position = new Vector2(x, y);
                            obj.Orientation = orientation*(float)(Math.PI*2/360);
                            obj.RigidBody.RestitutionCoefficient = od.restitution;
                            obj.RigidBody.FrictionCoefficient = od.friction;
                            obj.RigidBody.RotationalDragCoefficient = 10;
                            obj.RigidBody.LinearDragCoefficient = .00001f;
                            obj.CanAttachPortalTo = canAttachPortalTo;                        
                            _sceneObjects.Add(obj);                            
                                                                                        
                        }


                        // If its a frame of the animation
                        if (elementName == "trigger")
                        {       
                            String type = "";     
                            int x = 0;
                            int y = 0;
                            int w = 0;
                            int h = 0;
                            String nextLevel = "";
                            String audio = "";
                          
                            if (reader.HasAttributes)
                            {
                                for (int i = 0; i < reader.AttributeCount; i++)
                                {
                                    reader.MoveToAttribute(i);

                                    // Getting the properties of the object                          
                                    if (reader.Name == "type") { type = reader.Value; }       
                                    if (reader.Name == "x") { x = Int32.Parse(reader.Value); }
                                    if (reader.Name == "y") { y = Int32.Parse(reader.Value); }
                                    if (reader.Name == "w") { w = Int32.Parse(reader.Value); }
                                    if (reader.Name == "h") { h = Int32.Parse(reader.Value); }
                                    if (reader.Name == "audio") { audio = reader.Value; }
                                    if (reader.Name == "nextLevel") { nextLevel = reader.Value; } 
                                }
                            }

                            if (type == "player_spawn")
                            {
                                cPlayer.Instance.spawnIn(new Vector2(x,y));
                            }

                            if (type == "flame")
                            {
                                cFlameEffect fe = new cFlameEffect(Portal2D.GD, Portal2D.CM, w, h);
                                fe.Position = new Vector2(x, y);                                
                                _shaderObjects.Add(fe);
                            }

                            if (type == "exit")
                            {                               
                                cExit e = new cExit(nextLevel,audio);                               
                                e.Position = new Vector2(x,y);                                                        
                                _sceneObjects.Add(e);
                            }
                                                            
                        }

                        // If its a frame of the animation
                        if (elementName == "light")
                        {
                            String type = "";
                            String resource = "";
                            String lightMap = "";
                            int x = 0;
                            int y = 0;

                            if (reader.HasAttributes)
                            {
                                for (int i = 0; i < reader.AttributeCount; i++)
                                {
                                    reader.MoveToAttribute(i);

                                    // Getting the properties of the object                          
                                    if (reader.Name == "type") { type = reader.Value; }
                                    if (reader.Name == "resource") { resource = reader.Value; }
                                    if (reader.Name == "lightMap") { lightMap = reader.Value; }
                                    if (reader.Name == "x") { x = Int32.Parse(reader.Value); }
                                    if (reader.Name == "y") { y = Int32.Parse(reader.Value); }
                                }
                            }

                            if (type == "point")
                            {
                                cLight l = new cLight();
                                l.setStaticSprite(resource);                                
                                l.setLightMap(lightMap);
                                l.AffectedByGravity = false;
                                l.Position = new Vector2(x, y);
                                cLightMapManager.Instance.add(l);
                                _sceneObjects.Add(l);
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

            _levelFastestTime = this.getLevelFastestTime(_currentLevelName);            
        }

        public int getLevelFastestTime(String levelName)
        {
            // create reader & open file
            StreamReader tr = new StreamReader("playerRecords.ini");

            String line = tr.ReadLine();
            while (line!=null)
            {
                if (line != "")
                {
                    int indx = line.IndexOf(" ");
                    String level = line.Substring(0, indx);
                    int time = Int32.Parse(line.Substring(indx + 1));
                    if (level == _currentLevelName) { tr.Close(); return time; }
                    line = tr.ReadLine();
                }
            }

            tr.Close();
            return -1;
        }


        public void initLevel()
        {
            // Once a level has been loaded we need to work out the bounds for the scene and tell the camera
            // about these bounds 
            Vector4 camBounds = new Vector4();
            camBounds.X = _sceneObjects[0].Sprite.Position.X - _sceneObjects[0].Sprite.Width / 2; // Min x
            camBounds.Y = _sceneObjects[0].Sprite.Position.Y - _sceneObjects[0].Sprite.Height / 2; // Min y
            camBounds.W = _sceneObjects[0].Sprite.Position.X + _sceneObjects[0].Sprite.Width / 2; // Max x
            camBounds.Z = _sceneObjects[0].Sprite.Position.Y + _sceneObjects[0].Sprite.Height / 2; // Max y

            for (int i = 1; i < _sceneObjects.Count; i++)
            {
                cSprite s = _sceneObjects[i].Sprite;
                if ((s.Position.X - s.Width / 2) < camBounds.X) { camBounds.X = s.Position.X - s.Width / 2; }
                if ((s.Position.Y - s.Height / 2) < camBounds.Y) { camBounds.Y = s.Position.Y - s.Height / 2; }
                if ((s.Position.X + s.Width / 2) > camBounds.W) { camBounds.W = s.Position.X + s.Width / 2; }
                if ((s.Position.Y + s.Height / 2) > camBounds.Z) { camBounds.Z = s.Position.Y + s.Height / 2; }
            }

            cCamera.Instance.Bounds = camBounds;
        }

        public void update(GameTime gameTime)
        {
            Vector2 camPos = cCamera.Instance.Position;
            int x = (int)(camPos.X / _paralaxBack.Width);
            int y = (int)(camPos.Y / _paralaxBack.Height);
            x *= _paralaxBack.Width;
            y *= _paralaxBack.Height;

            _paralaxBack.Position = new Vector2(x + _paralaxBack.Width / 2, y + _paralaxBack.Height / 2);

            // Fix the paralax background
            //_paralaxBack.Position = new Vector2(_paralaxBack.Width / 2 + camPos.X, _paralaxBack.Height / 2 + camPos.Y);

            for (int i = 0; i < _sceneObjects.Count; i++)
            {
                _sceneObjects[i].update(gameTime);
            }

            for (int i = 0; i < _shaderObjects.Count; i++)
            {
                _shaderObjects[i].update(gameTime);
            }      

            if (cEngine.Instance.GameState==cEngine.GAME_STATE.INTRO)
            {
                _startButton.update(gameTime);
            }

            if (cEngine.Instance.GameState == cEngine.GAME_STATE.OUTRO)
            {
                _continueButton.update(gameTime);
            }
        }

        public void drawLevelIntro(SpriteBatch batch)
        {
            // Draw the blank window
            _blankWindow.Position = new Vector2(cCamera.Instance.WidthHeight.X / 2,cCamera.Instance.WidthHeight.Y / 2);
            _blankWindow.draw(batch);

            // Draw the name of the level
            _bitmapFont.Batch = batch;
            int nWidth = _bitmapFont.MeasureString(_currentLevelName);
            _bitmapFont.DrawString(360, 330 - (_blankWindow.Height / 2), Color.White, _currentLevelName, nWidth);

            // Draw the description of the level
            for (int i = 0; i < _currentLevelDescription.Length; i++)
            {
                string s = _currentLevelDescription[i];
                nWidth = _bitmapFont.MeasureString(s);
                _bitmapFont.KernEnable = true;
                _bitmapFont.DrawString(240, 360 - (_blankWindow.Height / 2) + (i * 18), s, nWidth);
            }

            // Draw the start button
            _startButton.Position = new Vector2(400, 300 + (_blankWindow.Height / 2) - _startButton.Height - 8);
            _startButton.draw(batch);
            
            // Draw Mouse pointer
            MouseState ms = cInput.Instance.Mouse;
            _mousePointer.Position = new Vector2(ms.X + _mousePointer.Width/2, ms.Y + _mousePointer.Height/2);
            _mousePointer.draw(batch);
        }

        public void drawLevelOutro(SpriteBatch batch)
        {
            // Draw the blank window
            _blankWindow.Position = new Vector2(cCamera.Instance.WidthHeight.X / 2, cCamera.Instance.WidthHeight.Y / 2);
            _blankWindow.draw(batch);

            // Draw the name of the level
            _bitmapFont.Batch = batch;
            int nWidth = _bitmapFont.MeasureString(_currentLevelName);
            _bitmapFont.DrawString(360, 330 - (_blankWindow.Height / 2), Color.White, _currentLevelName, nWidth);


            string s = "Congratulations you have compleated the level!";
            nWidth = _bitmapFont.MeasureString(s);
            _bitmapFont.DrawString(240, 360 - (_blankWindow.Height / 2) + (0 * 18), s, nWidth);

            s = "Level Time: " + cEngine.GameTimer/1000 + " seconds";
            nWidth = _bitmapFont.MeasureString(s);
            _bitmapFont.DrawString(240, 360 - (_blankWindow.Height / 2) + (2 * 18), s, nWidth);

            if (_levelFastestTime == -1) { s = "Record Time: NA";  }
            else { s = "Record Time: " + _levelFastestTime + " seconds"; }            
            nWidth = _bitmapFont.MeasureString(s);
            _bitmapFont.DrawString(240, 360 - (_blankWindow.Height / 2) + (3 * 18), s, nWidth);

            if (cEngine.GameTimer / 1000 < _levelFastestTime || _levelFastestTime==-1)
            {
                s = "NEW RECORD SET!";          
                nWidth = _bitmapFont.MeasureString(s);
                _bitmapFont.DrawString(240, 360 - (_blankWindow.Height / 2) + (5 * 18), s, nWidth);
            }

            // Draw the start button
            _continueButton.Position = new Vector2(400, 300 + (_blankWindow.Height / 2) - _startButton.Height - 8);
            _continueButton.draw(batch);

            // Draw Mouse pointer
            MouseState ms = cInput.Instance.Mouse;
            _mousePointer.Position = new Vector2(ms.X + _mousePointer.Width / 2, ms.Y + _mousePointer.Height / 2);
            _mousePointer.draw(batch);
        }
    
        public void drawBackground(SpriteBatch batch)
        {
            Vector2 newPos = Vector2.Zero;

            float w = _paralaxBack.Width;
            float h = _paralaxBack.Height;

            // For this paralax the bacgroung needs to be
            // rendered 9 times in the following seqence:
            // 1 | 2 | 3
            // 4 | 5 | 6
            // 7 | 8 | 9

            // Draw 5 (initial)
            _paralaxBack.draw(batch);

            // Draw 1
            newPos = _paralaxBack.Position;
            newPos.X -= w;
            newPos.Y -= h;
            _paralaxBack.Position = newPos;
            _paralaxBack.draw(batch);

            // Draw 2
            newPos.X += w;
            _paralaxBack.Position = newPos;
            _paralaxBack.draw(batch);

            // Draw 3
            newPos.X += w;
            _paralaxBack.Position = newPos;
            _paralaxBack.draw(batch);

            // Draw 4
            newPos.X -= w * 2;
            newPos.Y += h;
            _paralaxBack.Position = newPos;
            _paralaxBack.draw(batch);

            // Draw 6
            newPos.X += w * 2;
            _paralaxBack.Position = newPos;
            _paralaxBack.draw(batch);

            // Draw 7 
            newPos.X -= w * 2;
            newPos.Y += h;
            _paralaxBack.Position = newPos;
            _paralaxBack.draw(batch);

            // Draw 8
            newPos.X += w;
            _paralaxBack.Position = newPos;
            _paralaxBack.draw(batch);

            // Draw 9
            newPos.X += w;
            _paralaxBack.Position = newPos;
            _paralaxBack.draw(batch);

            // Return to original pos
            newPos.X -= w;
            newPos.Y -= h;
            _paralaxBack.Position = newPos;
        }

        public void draw(SpriteBatch batch)
        {      
            for (int i = 0; i < _sceneObjects.Count; i++)
            {                
                _sceneObjects[i].draw(batch);
            }            
        }

        public void onButtonClicked(cMenuButton button)
        {
            // If the start menu button was clicked
            if (button==_startButton)
            {
                cEngine.Instance.startLevel();
            }
            else if (button==_continueButton)
            {
                if (cEngine.GameTimer / 1000 < _levelFastestTime || _levelFastestTime == -1)
                {
                    saveRecord();
                }
                Console.WriteLine(_nextLevel);
                cEngine.Instance.loadLevel(_nextLevel);
            }      
        }

        private void saveRecord()
        {
            // create reader & open file
            StreamReader tr = new StreamReader("playerRecords.ini");

            // create a writer and open the file
            TextWriter tw;

            String wholeThing = tr.ReadToEnd();
            tr.Close();

            if (wholeThing.Contains(_currentLevelName))
            {     
                wholeThing = wholeThing.Replace(_currentLevelName + " " + _levelFastestTime, _currentLevelName + " " + (cEngine.GameTimer / 1000));

                tw = new StreamWriter("playerRecords.ini", false);
                tw.Write(wholeThing);
            }
            else
            {
                String s = _currentLevelName + " " + (cEngine.GameTimer / 1000);
                tw = new StreamWriter("playerRecords.ini", true);
                tw.WriteLine();
                tw.Write(s);
            }

            // close the write stream
            tw.Close();
        }

        public void drawShaderEffects(cCustomSpriteBatch batch)
        {
            for (int i = 0; i < _shaderObjects.Count; i++)
            {
                _shaderObjects[i].draw(batch);
            }
        }


       

    }
}
