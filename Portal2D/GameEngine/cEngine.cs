
#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

using Portal2D.GameEngine.Scene;
using Portal2D.GameEngine.Input;
using Portal2D.GameEngine.Sprites;
using Portal2D.GameEngine.Physics;
using Portal2D.GameEngine.Portals;
using Portal2D.GameEngine.Audio;
using Portal2D.GameEngine.Particles;
using Portal2D.GameEngine.PostProcessing;
using Portal2D.GameEngine.Lights;
using Portal2D.GameEngine.HUD;
#endregion


namespace Portal2D.GameEngine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class cEngine
    {
        private GraphicsDeviceManager _graphics;
        private ContentManager _contentManager;
        private cScene _scene;
        private cPhysics _physics;
        private cPlayer _player;
        private cEffectManager _effectManager;
        private cPortalManager _portalManager;        
        private cParticleEngine _particleEngine;
        private cPostProcessor _postProcessor;
        private cLightMapManager _lightMapManager;
        private static cEngine _instance;
        private cHUD _HUD;
        private int _gameTimer;
        private cCustomSpriteBatch _batch;

        private GAME_STATE _gameState;

        public enum GAME_STATE
        {
            NOTHING,
            INTRO,
            RUNNING,
            OUTRO,
            PAUSED
        }
                
        public static int GameTimer { get { return _instance._gameTimer; } set { _instance._gameTimer = value; } }
        public GAME_STATE GameState { get { return _gameState; } set { _gameState = value; } }
        public static cEngine Instance { get { return _instance; } }
        
        public cEngine(GraphicsDeviceManager gd, ContentManager cm)
        {
            _instance = this;
            _graphics = gd;
            _contentManager = cm;     
                                            
            _postProcessor = new cPostProcessor(gd.GraphicsDevice, cm);           
            _gameState = GAME_STATE.NOTHING;
            _batch = new cCustomSpriteBatch(gd.GraphicsDevice);

            _HUD = new cHUD();
        }
       
        public void loadLevel(String levelName)
        {
            // Very quick and dirty way of removing all objects!
            _scene = new cScene();
            _physics = new cPhysics();
            _player = new cPlayer();
            _particleEngine = new cParticleEngine();
            _effectManager = cEffectManager.Instance;
            _lightMapManager = new cLightMapManager();
            _portalManager = new cPortalManager();

            _gameTimer = 0;
            _scene.loadLevel(levelName);
            _scene.initLevel();
            cCamera.Instance.FocusObject = _player;
            _player.LivesRemain = 3;
            _gameState = GAME_STATE.INTRO;
        }

        public void continueGame()
        {
            if (_gameState == GAME_STATE.PAUSED) { _gameState = GAME_STATE.RUNNING; }
            else { loadLevel("Training"); }
        }

        public void startLevel()
        {    
            _gameState = GAME_STATE.RUNNING;
        }

        public void endLevel()
        {       
            _gameState = GAME_STATE.OUTRO;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {                       
            _scene.update(gameTime);

            if (_gameState == GAME_STATE.RUNNING)
            {
                _gameTimer += gameTime.ElapsedGameTime.Milliseconds;
                _physics.update(gameTime);
                _player.update(gameTime);
                _effectManager.update(gameTime);
                _portalManager.update(gameTime);
                _particleEngine.update(gameTime);
                _postProcessor.update(gameTime);
            }

            if (cInput.Instance.Keyboard.IsKeyDown(Keys.Escape)) { _gameState = GAME_STATE.PAUSED; Portal2D.Instance.GameRunning = false; }
            
            // TEMP TEMP TEMP TEMP TEMP
            if (cInput.Instance.Keyboard.IsKeyDown(Keys.Left)) { cCamera.Instance.impulse(-2, 0); }
            if (cInput.Instance.Keyboard.IsKeyDown(Keys.Right)) { cCamera.Instance.impulse(2, 0); }
            if (cInput.Instance.Keyboard.IsKeyDown(Keys.Up)) { cCamera.Instance.impulse(0, -2); }
            if (cInput.Instance.Keyboard.IsKeyDown(Keys.Down)) { cCamera.Instance.impulse(0, 2); }              
        
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(SpriteBatch batch)
        {
           
            GraphicsDevice gd = _graphics.GraphicsDevice;      

            _postProcessor.startMainRender(gd);
            batch.Begin(SpriteBlendMode.AlphaBlend);
            _scene.drawBackground(batch);           
            _lightMapManager.draw(batch);
            _portalManager.draw(batch);   
            _scene.draw(batch);            
            _player.draw(batch);
            _effectManager.draw(batch);
            _particleEngine.draw(batch);

            if (_gameState == GAME_STATE.INTRO) { _scene.drawLevelIntro(batch); }
            if (_gameState == GAME_STATE.OUTRO) { _scene.drawLevelOutro(batch); } 
  
            batch.End();
            _scene.drawShaderEffects(_batch);   
            batch.Begin(SpriteBlendMode.AlphaBlend);
            _HUD.draw(batch);
            batch.End();
            _postProcessor.endMainRender(gd);   
            _postProcessor.Render(gd);

            
        }
    }
}