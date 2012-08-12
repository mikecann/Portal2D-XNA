/////////////////////////////////////////////////////////////////////////////////////////////////
//   BEGIN MySpriteBatch.cs
//////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Portal2D.GameEngine.Sprites
{

    public class cPostProcessBatch
    {

        protected VertexPositionTexture[] vertices;
        protected short[] indices;
        protected int vertexCount = 0;
        protected int indexCount = 0;
        protected Texture2D texture;
        protected VertexDeclaration declaration;
        protected GraphicsDevice device;

        //  these should really be properties
        public Matrix World;
        public Matrix View;
        public Matrix Projection;
        public Effect Effect;

        public cPostProcessBatch(GraphicsDevice device)
        {
            this.device = device;
            this.vertices = new VertexPositionTexture[256];
            this.indices = new short[vertices.Length * 3 / 2];
        }

        public void ResetMatrices(int width, int height)
        {
            this.World = Matrix.Identity;
            this.View = new Matrix(
                1.0f, 0.0f, 0.0f, 0.0f,
                0.0f, -1.0f, 0.0f, 0.0f,
                0.0f, 0.0f, -1.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
            this.Projection = Matrix.CreateOrthographicOffCenter(
                0, width, -height, 0, 0, 1);
        }

        public virtual void RenderToTexture(RenderTarget2D target)
        {
            float fWidth = target.Width - 0.5f;
            float fHeight = target.Height - 0.5f;

             //  ensure space for my vertices and indices.
            this.EnsureSpace(6, 4);

            //  add the new indices
            indices[indexCount++] = (short)(vertexCount + 0);
            indices[indexCount++] = (short)(vertexCount + 1);
            indices[indexCount++] = (short)(vertexCount + 3);
            indices[indexCount++] = (short)(vertexCount + 1);
            indices[indexCount++] = (short)(vertexCount + 2);
            indices[indexCount++] = (short)(vertexCount + 3);

             // add the new vertices
            vertices[vertexCount++] = new VertexPositionTexture(new Vector3(-0.5f, -0.5f, 0)
                , new Vector2(0, 0));

            vertices[vertexCount++] = new VertexPositionTexture(new Vector3(fWidth, -0.5f, 0)
                , new Vector2(1, 0));

            vertices[vertexCount++] = new VertexPositionTexture(new Vector3(fWidth, fHeight, 0)
                , new Vector2(1, 1));

            vertices[vertexCount++] = new VertexPositionTexture(new Vector3(-0.5f, fHeight, 0)
                , new Vector2(0, 1));

            //  we premultiply all vertices times the world matrix.
            //  the world matrix changes alot and we don't want to have to flush
            //  every time it changes.
            Matrix world = this.World;
            for (int i = vertexCount - 4; i < vertexCount; i++)
                Vector3.Transform(ref vertices[i].Position, ref world, out vertices[i].Position);

        }

        //public virtual void Draw(Texture2D texture, Rectangle srcRectangle, Rectangle dstRectangle, Color color)
        //{
        //    //  if the texture changes, we flush all queued sprites.
        //    if (this.texture != null && this.texture != texture)
        //        this.Flush();
        //    this.texture = texture;

        //    //  ensure space for my vertices and indices.
        //    this.EnsureSpace(6, 4);

        //    //  add the new indices
        //    indices[indexCount++] = (short)(vertexCount + 0);
        //    indices[indexCount++] = (short)(vertexCount + 1);
        //    indices[indexCount++] = (short)(vertexCount + 3);
        //    indices[indexCount++] = (short)(vertexCount + 1);
        //    indices[indexCount++] = (short)(vertexCount + 2);
        //    indices[indexCount++] = (short)(vertexCount + 3);

        //    // add the new vertices
        //    vertices[vertexCount++] = new VertexPositionColorTexture(
        //        new Vector3(dstRectangle.Left, dstRectangle.Top, 0)
        //        , color, GetUV(srcRectangle.Left, srcRectangle.Top));
        //    vertices[vertexCount++] = new VertexPositionColorTexture(
        //        new Vector3(dstRectangle.Right, dstRectangle.Top, 0)
        //        , color, GetUV(srcRectangle.Right, srcRectangle.Top));
        //    vertices[vertexCount++] = new VertexPositionColorTexture(
        //        new Vector3(dstRectangle.Right, dstRectangle.Bottom, 0)
        //        , color, GetUV(srcRectangle.Right, srcRectangle.Bottom));
        //    vertices[vertexCount++] = new VertexPositionColorTexture(
        //        new Vector3(dstRectangle.Left, dstRectangle.Bottom, 0)
        //        , color, GetUV(srcRectangle.Left, srcRectangle.Bottom));

        //    //  we premultiply all vertices times the world matrix.
        //    //  the world matrix changes alot and we don't want to have to flush
        //    //  every time it changes.
        //    Matrix world = this.World;
        //    for (int i = vertexCount - 4; i < vertexCount; i++)
        //        Vector3.Transform(ref vertices[i].Position, ref world, out vertices[i].Position);
        //}

        Vector2 GetUV(float x, float y)
        {
            return new Vector2(x / (float)texture.Width, y / (float)texture.Height);
        }

        protected void EnsureSpace(int indexSpace, int vertexSpace)
        {
            if (indexCount + indexSpace >= indices.Length)
                Array.Resize(ref indices, Math.Max(indexCount + indexSpace, indices.Length * 2));
            if (vertexCount + vertexSpace >= vertices.Length)
                Array.Resize(ref vertices, Math.Max(vertexCount + vertexSpace, vertices.Length * 2));
        }

        public virtual void Flush()
        {
            if (this.vertexCount > 0)
            {
                if (this.declaration == null || this.declaration.IsDisposed)
                    this.declaration = new VertexDeclaration(device, VertexPositionTexture.VertexElements);

                device.VertexDeclaration = this.declaration;

                Effect effect = this.Effect;
                //  set the only parameter this effect takes.
                effect.Parameters["viewProjection"].SetValue(this.View * this.Projection);
                
                EffectTechnique technique = effect.CurrentTechnique;
                effect.Begin();
                EffectPassCollection passes = technique.Passes;
                for (int i = 0; i < passes.Count; i++)
                {
                    EffectPass pass = passes[i];
                    pass.Begin();

                    device.DrawUserIndexedPrimitives<VertexPositionTexture>(
                        PrimitiveType.TriangleList, this.vertices, 0, this.vertexCount,
                        this.indices, 0, this.indexCount / 3);

                    pass.End();
                }
                effect.End();

                this.vertexCount = 0;
                this.indexCount = 0;
            }
        }

    }

}
//////////////////////////////////////////////////////////////////////////////////////////////////
//   END MySpriteBatch.cs
//////////////////////////////////////////////////////////////////////////////////////////////////