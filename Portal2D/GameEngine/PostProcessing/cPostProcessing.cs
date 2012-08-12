using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Portal2D.GameEngine.Input;

using Portal2D.GameEngine.Sprites;


namespace Portal2D.GameEngine.PostProcessing
{
    public class cPostProcessor
    {
        private Effect _brightPassShader;
        private Effect _downsamplePassShader;
        private Effect _hozbloomPassShader;
        private Effect _vertbloomPassShader;
        private Effect _finalPassShader;
        private static cPostProcessor _instance;
        private float _brightPassValue;

        public float BrightPassValue { get { return _brightPassValue; } set { _brightPassValue = value; } }
        public static cPostProcessor Instance { get { return _instance; } }

        //private cSpriteBatch _batch;
        private cPostProcessBatch _ppBatch;
        
        public Effect Shader
        {
            get { return _brightPassShader; }
            set { _brightPassShader = value; }
        }

        private RenderTarget2D _sceneRender;
        private RenderTarget2D _brightPassRender;
        private RenderTarget2D _downsamplePassRender;
        private RenderTarget2D _hozbloomPassRender;
        private RenderTarget2D _vertbloomPassRender;

        private float _initBrightPassValue;  
        private float _finalBrightPassValue;
        private int _brightDarkCount;
        private int _brightDarkMaxTimer;

        public cPostProcessor(GraphicsDevice gd, ContentManager cm)
        {
            _instance = this;

            _initBrightPassValue = 0.8f; 
            _finalBrightPassValue = 0f;
            _brightDarkCount = 0;
            _brightDarkMaxTimer=-1;
            _brightPassValue = _initBrightPassValue;

            _brightPassShader = cm.Load<Effect>(@"Resources/Shaders/Bloom/brightPass");
            _downsamplePassShader = cm.Load<Effect>(@"Resources/Shaders/Bloom/downsamplePass");
            _hozbloomPassShader = cm.Load<Effect>(@"Resources/Shaders/Bloom/hozbloom");
            _vertbloomPassShader = cm.Load<Effect>(@"Resources/Shaders/Bloom/vertbloom");
            _finalPassShader = cm.Load<Effect>(@"Resources/Shaders/Bloom/finalPass");

            //_batch = new cSpriteBatch(gd);
            _ppBatch = new cPostProcessBatch(gd);

            _sceneRender = new RenderTarget2D(gd,
                gd.Viewport.Width,
                gd.Viewport.Height,
                1,
                gd.DisplayMode.Format);

            _brightPassRender = new RenderTarget2D(gd,
                gd.Viewport.Width / 2,
                gd.Viewport.Height / 2,
                1,
                gd.DisplayMode.Format);

            _downsamplePassRender = new RenderTarget2D(gd,
                gd.Viewport.Width / 6,
                gd.Viewport.Height / 6,
                1,
                gd.DisplayMode.Format);

            _hozbloomPassRender = new RenderTarget2D(gd,
                gd.Viewport.Width / 6,
                gd.Viewport.Height / 6,
                1,
                gd.DisplayMode.Format);

            _vertbloomPassRender = new RenderTarget2D(gd,
                gd.Viewport.Width / 6,
                gd.Viewport.Height / 6,
                1,
                gd.DisplayMode.Format);
        }

        public void goBrightThenDark(int time)
        { 
            _brightDarkCount = 0;
            _brightDarkMaxTimer = time;
        }
        
        public void startMainRender(GraphicsDevice gd)
        {
            gd.SetRenderTarget(0, _sceneRender);
            gd.Clear(Color.CornflowerBlue);
        }

        public void endMainRender(GraphicsDevice gd)
        {
            gd.ResolveRenderTarget(0);
            //gd.SetRenderTarget(0, null);
        }

        public void update(GameTime gameTime)
        {
            if (_brightDarkMaxTimer != -1)
            {

                float diff = _initBrightPassValue - _finalBrightPassValue;
                float ratio = diff/(_brightDarkMaxTimer / 2);

                if (_brightDarkCount < _brightDarkMaxTimer / 2)
                {
                    _brightPassValue = _initBrightPassValue - (ratio * _brightDarkCount);
                }
                else
                {
                    _brightPassValue = (ratio * _brightDarkCount) - _initBrightPassValue;
                }

                //Console.WriteLine(_brightPassValue);

                _brightDarkCount += gameTime.ElapsedGameTime.Milliseconds;
                if (_brightDarkCount > _brightDarkMaxTimer) { _brightDarkMaxTimer = -1; _brightPassValue = _initBrightPassValue; }
            }
        }
        
        public void Render(GraphicsDevice gd)
        {
            /////////////////////////////////
            // BRIGHT PASS
            /////////////////////////////////

            gd.SetRenderTarget(0, _brightPassRender);
            //gd.SetRenderTarget(0, null);
            gd.Clear(Color.CornflowerBlue);

            // We need to compute the sampling offsets used for this pass.
            // A 2x2 sampling pattern is used, so we need to generate 4 offsets
            Vector4[] offsets = new Vector4[4];                    

            // Because the source and destination are NOT the same sizes, we
            // need to provide offsets to correctly map between them.
            float sU = (1.0f / _sceneRender.Width);
            float sV = (1.0f / _sceneRender.Height);
            
            // The last two components (z,w) are unused. This makes for simpler code, but if
            // constant-storage is limited then it is possible to pack 4 offsets into 2 float4's
            offsets[0] = new Vector4(-0.5f * sU, 0.5f * sV, 0.0f, 0.0f);
            offsets[1] = new Vector4(0.5f * sU, 0.5f * sV, 0.0f, 0.0f);
            offsets[2] = new Vector4(-0.5f * sU, -0.5f * sV, 0.0f, 0.0f);
            offsets[3] = new Vector4(0.5f * sU, -0.5f * sV, 0.0f, 0.0f);

            gd.Clear(Color.Black);
            _ppBatch.Effect = _brightPassShader;
            _ppBatch.ResetMatrices(gd.Viewport.Width, gd.Viewport.Height);
            _ppBatch.Effect.Parameters["tcDownSampleOffsets"].SetValue(offsets);
            _ppBatch.Effect.Parameters["fBrightPassThreshold"].SetValue(_brightPassValue);
            _ppBatch.Effect.Parameters["diffuseTexture"].SetValue(_sceneRender.GetTexture());
            _ppBatch.RenderToTexture(_brightPassRender);
            //_ppBatch.Draw(_sceneRender.GetTexture(), new Rectangle(0, 0, 800, 600), new Rectangle(0, 0, (800 / 2), (600 / 2)), Color.White);
            _ppBatch.Flush();

            /////////////////////////////////
            // DOWNSAMPLE PASS
            /////////////////////////////////

            gd.ResolveRenderTarget(0);
            //gd.SetRenderTarget(0, null);
            gd.SetRenderTarget(0, _downsamplePassRender);

            // We need to compute the sampling offsets used for this pass.
            // A 2x2 sampling pattern is used, so we need to generate 4 offsets
            Vector4[] offsets2 = new Vector4[16];

            int idx = 0;
            for (int i = -2; i < 2; i++)
            {
                for (int j = -2; j < 2; j++)
                {
                    offsets2[idx++] = new Vector4(
                                                ((i) + 0.5f) * (1.0f / (_downsamplePassRender.Width)),
                                                ((j) + 0.5f) * (1.0f / (_downsamplePassRender.Height)),
                                                0.0f, // unused 
                                                0.0f  // unused
                                            );
                }
            }

            gd.Clear(Color.Black);
            _ppBatch.Effect = _downsamplePassShader;
            _ppBatch.ResetMatrices(gd.Viewport.Width, gd.Viewport.Height);
            _ppBatch.Effect.Parameters["tcDownSampleOffsets"].SetValue(offsets2);
            _ppBatch.Effect.Parameters["diffuseTexture"].SetValue(_brightPassRender.GetTexture());
            _ppBatch.RenderToTexture(_downsamplePassRender);
            //_ppBatch.Draw(_brightPassRender.GetTexture(), new Rectangle(0, 0, 800 / 2, 600 / 2), new Rectangle(0, 0, 800 / 8, 600 / 8), Color.White);
            _ppBatch.Flush();

            ///////////////////////////////////
            //// HORIZONTAL BLOOM PASS
            ///////////////////////////////////

            gd.ResolveRenderTarget(0);
            //gd.SetRenderTarget(0, null);
            gd.SetRenderTarget(0, _hozbloomPassRender);

            // Configure the sampling offsets and their weights
            float[] HBloomWeights = new float[9];
            float[] HBloomOffsets = new float[9];

            for (int i = 0; i < 9; i++)
            {
                // Compute the offsets. We take 9 samples - 4 either side and one in the middle:
                //     i =  0,  1,  2,  3, 4,  5,  6,  7,  8
                //Offset = -4, -3, -2, -1, 0, +1, +2, +3, +4
                HBloomOffsets[i] = (i - 4.0f) * (1.0f / _downsamplePassRender.Width);

                // 'x' is just a simple alias to map the [0,8] range down to a [-1,+1]
                float x = (i - 4.0f) / 4.0f;

                // Use a gaussian distribution. Changing the standard-deviation
                // (second parameter) as well as the amplitude (multiplier) gives
                // distinctly different results.
                HBloomWeights[i] = 0.4f * ComputeGaussianValue(x, 0.0f, 0.8f);
            }

            gd.Clear(Color.Black);
            _ppBatch.Effect = _hozbloomPassShader;
            _ppBatch.ResetMatrices(gd.Viewport.Width, gd.Viewport.Height);
            _ppBatch.Effect.Parameters["HBloomWeights"].SetValue(HBloomWeights);
            _ppBatch.Effect.Parameters["HBloomOffsets"].SetValue(HBloomOffsets);
            _ppBatch.Effect.Parameters["diffuseTexture"].SetValue(_downsamplePassRender.GetTexture());
            _ppBatch.RenderToTexture(_hozbloomPassRender);
            //_ppBatch.Draw(_downsamplePassRender.GetTexture(), new Rectangle(0, 0, 800 / 8, 600 / 8), new Rectangle(0, 0, 800 / 8, 600 / 8), Color.White);
            _ppBatch.Flush();
            
            /////////////////////////////////////
            ////// VERTICAL BLOOM PASS
            /////////////////////////////////////

            gd.ResolveRenderTarget(0);
            //gd.SetRenderTarget(0, null);
            gd.SetRenderTarget(0, _vertbloomPassRender);

            // Configure the sampling offsets and their weights
            float[] VBloomWeights = new float[9];
            float[] VBloomOffsets = new float[9];

            for (int i = 0; i < 9; i++)
            {
                // Compute the offsets. We take 9 samples - 4 either side and one in the middle:
                //     i =  0,  1,  2,  3, 4,  5,  6,  7,  8
                //Offset = -4, -3, -2, -1, 0, +1, +2, +3, +4
                VBloomOffsets[i] = (i - 4.0f) * (1.0f / _downsamplePassRender.Height);

                // 'x' is just a simple alias to map the [0,8] range down to a [-1,+1]
                float x = (i - 4.0f) / 4.0f;

                // Use a gaussian distribution. Changing the standard-deviation
                // (second parameter) as well as the amplitude (multiplier) gives
                // distinctly different results.
                VBloomWeights[i] = 0.4f * ComputeGaussianValue(x, 0.0f, 0.8f);
            }

            gd.Clear(Color.Black);
            _ppBatch.Effect = _vertbloomPassShader;
            _ppBatch.ResetMatrices(gd.Viewport.Width, gd.Viewport.Height);
            _ppBatch.Effect.Parameters["VBloomWeights"].SetValue(VBloomWeights);
            _ppBatch.Effect.Parameters["VBloomOffsets"].SetValue(VBloomOffsets);
            _ppBatch.Effect.Parameters["diffuseTexture"].SetValue(_hozbloomPassRender.GetTexture());
            _ppBatch.RenderToTexture(_vertbloomPassRender);
            //_ppBatch.Draw(_hozbloomPassRender.GetTexture(), new Rectangle(0, 0, 800 / 8, 600 / 8), new Rectangle(0, 0, 800 / 8, 600 / 8), Color.White);
            _ppBatch.Flush();

            /////////////////////////////////////
            ////// FINAL PASS
            /////////////////////////////////////

            gd.ResolveRenderTarget(0);
            gd.SetRenderTarget(0, null);

            gd.Clear(Color.Black);
            _ppBatch.Effect = _finalPassShader;
            _ppBatch.ResetMatrices(gd.Viewport.Width, gd.Viewport.Height);
            _ppBatch.Effect.Parameters["g_rcp_bloom_tex_w"].SetValue(1.0f / _vertbloomPassRender.Width);
            _ppBatch.Effect.Parameters["g_rcp_bloom_tex_h"].SetValue(1.0f / _vertbloomPassRender.Height);
            _ppBatch.Effect.Parameters["fExposure"].SetValue(0.5f);
            _ppBatch.Effect.Parameters["fGaussianScalar"].SetValue(0.4f);
            _ppBatch.Effect.Parameters["BloomRender"].SetValue(_vertbloomPassRender.GetTexture());
            _ppBatch.Effect.Parameters["diffuseTexture"].SetValue(_sceneRender.GetTexture());
            _ppBatch.RenderToTexture(_sceneRender);
            //_ppBatch.Draw(_sceneRender.GetTexture(), new Rectangle(0, 0, 800, 600), new Rectangle(0, 0, 800, 600), Color.White);
            _ppBatch.Flush();

 
          
             
        }

        float ComputeGaussianValue(float x, float mean, float std_deviation)
        {
            // The gaussian equation is defined as such:
            /*    
                                                             -(x - mean)^2
                                                             -------------
                                            1.0               2*std_dev^2
                f(x,mean,std_dev) = -------------------- * e^
                                    sqrt(2*pi*std_dev^2)

            */

            return (float)((1.0f / Math.Sqrt(2.0f * Math.PI * std_deviation * std_deviation)) * Math.Exp(((-((x - mean) * (x - mean))) / (2.0f * std_deviation * std_deviation)) ));
        }

    }
}
