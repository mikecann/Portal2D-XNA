#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

using Microsoft.DirectX.AudioVideoPlayback;

using Portal2D.GameEngine.Scene;
using Portal2D.GameEngine.Input;
using Portal2D.GameEngine.Sprites;
using Portal2D.GameEngine.Physics;
using Portal2D.GameEngine.Portals;
using Portal2D.GameEngine.Audio;
using Portal2D.GameEngine.Particles;
using Portal2D.GameEngine.PostProcessing;
using Portal2D.GameEngine.Lights;
using Portal2D.Videos;
#endregion

namespace Portal2D.Videos
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class cVideo 
    {

        private Video vid;
        private bool playstate;

        public cVideo(GameWindow gw)
        {

            vid = new Video(@"Resources/Video/helloworldintro.avi");
            vid.Owner = Form.FromHandle(gw.Handle);
            vid.Ending += new EventHandler(vid_Ending);
            playstate = true;
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected void Initialize()
        {
            // TODO: Add your initialization logic here
            

        }

        
        /// <summary>
        /// Load your graphics content.  If loadAllContent is true, you should
        /// load content from both ResourceManagementMode pools.  Otherwise, just
        /// load ResourceManagementMode.Manual content.
        /// </summary>
        /// <param name="loadAllContent">Which type of content to load.</param>
        protected void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {


            }

            // TODO: Load any ResourceManagementMode.Manual content
        }


        /// <summary>
        /// Unload your graphics content.  If unloadAllContent is true, you should
        /// unload content from both ResourceManagementMode pools.  Otherwise, just
        /// unload ResourceManagementMode.Manual content.  Manual content will get
        /// Disposed by the GraphicsDevice during a Reset.
        /// </summary>
        /// <param name="unloadAllContent">Which type of content to unload.</param>
        protected void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent == true)
            {
               
            }
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public bool playVideo(string filename)
        {
            if(playstate == true)  vid.Play();

            return playstate;
        }

        public void mydispose()
        {
            if (playstate == false) vid.Dispose();
        }

        void vid_Ending(object sender, EventArgs e)
        {
            vid.Stop();
            playstate = false;
        }
    }
}
