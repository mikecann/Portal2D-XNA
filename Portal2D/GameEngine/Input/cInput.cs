using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Portal2D.GameEngine.Input
{
    class cInput
    {
        private static cInput _instance;        // singleton class
        private KeyboardState _keyboardState;
        private MouseState _mouseState;

        // get the instance for this singleton class
        public static cInput Instance
        {
            get
            {
                // if not instancated yet, do so now
                if (_instance == null) { new cInput(); }
                return _instance;
            }
        }

        public KeyboardState Keyboard
        {
            get { return _keyboardState; }
        }

        public MouseState Mouse
        {
            get { return _mouseState; }
        }

        public cInput()
        {
            _instance = this;        
        }

        public void update(GameTime gameTime)
        {
            _keyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            _mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
        }

    }
}
