using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Portal2D.GameEngine.Physics;

namespace Portal2D.GameEngine.Scene
{
    class cCamera
    {
        private Vector2 _pos;
        private Vector2 _vel;
        private Vector2 _widthHeight;
        private static cCamera _instance;
        private const float MAX_VEL = 4.0f;
        private cRigidBodyGameObject _focusObject;
        private Vector4 _bounds;
        private bool _updateToFocus;
                
   
        /// <summary>
        /// get the instance for this class (singleton)
        /// </summary>
        public static cCamera Instance
        {
            get
            {
                // if not instanced yet, do so now
                if (_instance == null) { new cCamera(); }
                return _instance;
            }
        }

        /// <summary>
        /// get set the object for the camera to focus on
        /// </summary>
        public cRigidBodyGameObject FocusObject
        {
            get { return _focusObject; }
            set { _focusObject = value; _updateToFocus = true; }
        }

        /// <summary>
        /// get set width height of the camera
        /// </summary>
        public Vector2 WidthHeight
        {
            get { return _widthHeight; }
            set { _widthHeight = value; }
        }

        /// <summary>
        /// get set position
        /// </summary>
        public Vector2 Position
        {
            get { return _pos; }
            set { _pos = value; }
        }

        /// <summary>
        /// Limits the camera to a particular bound
        /// </summary>
        public Vector4 Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        public cCamera() { }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="x">where to start x</param>
        /// <param name="y">where to start x</param>
        /// <param name="w">width of camera</param>
        /// <param name="h">height of camera</param>
        public cCamera(int x, int y, int w, int h)
        {
            _instance = this;
            _pos = new Vector2(x,y);
            _widthHeight = new Vector2(w,h);
            _focusObject = null;
            _bounds = Vector4.Zero;
            _updateToFocus = false;
        }   

        /// <summary>
        /// translate the camera by a given amount
        /// </summary>
        /// <param name="x">move by how much in the x dir</param>
        /// <param name="y">move by how much in the y dir</param>
        public void translate(int x, int y)
        {
            _pos.X += x;
            _pos.Y += y;
        }

        /// <summary>
        /// Apply a velocity impulse to the camera 
        /// Only works when focus object is set to null
        /// </summary>
        /// <param name="x">impuse in the x dir</param>
        /// <param name="y">impulse in the y dir</param>
        public void impulse(int x, int y)
        {
            _vel.X += x;
            _vel.Y += y;

            // dont allow our velocity to get too big!
            if (_vel.X > MAX_VEL) { _vel.X = MAX_VEL; }
            if (_vel.Y > MAX_VEL) { _vel.Y = MAX_VEL; }
            if (_vel.X < -MAX_VEL) { _vel.X = -MAX_VEL; }
            if (_vel.Y < -MAX_VEL) { _vel.Y = -MAX_VEL; }
        }

        public void updateToFocusObject()
        {
            _updateToFocus = true;
        }

        /// <summary>
        /// Update the camera
        /// </summary>
        /// <param name="gameTime">game time object</param>
        public void update(GameTime gameTime)
        {
            if (_updateToFocus)
            {
                _updateToFocus = false;

                // If there is no focus object then use a velocity type of movement system
                if (_focusObject != null)              
                {
                    // Focus on the focus object and centre it to screen
                    _pos = _focusObject.Position;
                    _pos -= _widthHeight / 2;
                }
            }
            else
            {
                // Apply velocity to camera position
                _pos += _vel;
                _vel *= 0.95f;

                // If the velocity gets below a certain value then just stop the camera
                // from moving.
                if (_vel.X < 0.1 && _vel.X > -0.1) { _vel.X = 0; }
                if (_vel.Y < 0.1 && _vel.Y > -0.1) { _vel.Y = 0; }
            }

            // We are in menus exit
            if (_bounds.Y == 0 && _bounds.Z == 0) { return; }

            // Now constrict the camera to a bound           
            if (_pos.X < _bounds.X) { _pos.X = _bounds.X; }
            if (_pos.Y < _bounds.Y) { _pos.Y = _bounds.Y; }
            if (_pos.X + _widthHeight.X > _bounds.W) { _pos.X = _bounds.W - _widthHeight.X; }
            if (_pos.Y + _widthHeight.Y > _bounds.Z) { _pos.Y = _bounds.Z - _widthHeight.Y; }

           
        }
    }
}
