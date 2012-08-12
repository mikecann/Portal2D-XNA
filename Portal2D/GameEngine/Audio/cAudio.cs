using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Portal2D.GameEngine.Audio
{
    class cAudio
    {
        private static AudioEngine _engine;
        private static WaveBank _waveBank;
        private static SoundBank _soundBank;
        private static cAudio _instance;

        // get the instance for this singleton class
        public static cAudio Instance
        {
            get
            {
                // if not instancated yet, do so now
                if (_instance == null) { new cAudio(); }
                return _instance;
            }
        }

        public cAudio()
        {
            _instance = this;
            _engine = new AudioEngine("Resources/Audio/Portal2DSounds.xgs");
            _waveBank = new WaveBank(_engine, "Resources/Audio/Wave Bank.xwb");
            _soundBank = new SoundBank(_engine, "Resources/Audio/Sound Bank.xsb");  
        }

        public void play(string name)
        {
            try
            {
                _soundBank.PlayCue(name);           
            }
            catch (Exception e) { }
                      
        }
       
        public void stop(Cue cue)
        {
            cue.Stop(AudioStopOptions.Immediate);
        }
     
        public void update(GameTime gametime)  
        {
            _engine.Update();
        }

        /// <summary>
        /// Shuts down the sound code tidily
        /// </summary>
        public void shutdown()
        {
            _soundBank.Dispose();
            _waveBank.Dispose();
            _engine.Dispose();
        }
    }
}
