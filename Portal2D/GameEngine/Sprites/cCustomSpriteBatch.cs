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

    public class cCustomSpriteBatch : cSpriteBatch
    {

        public cCustomSpriteBatch(GraphicsDevice device) : base (device)
        {           
        }
 
        public void Draw(Rectangle dstRectangle, Color color)
        {           
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
            vertices[vertexCount++] = new VertexPositionColorTexture(
                new Vector3(dstRectangle.Left, dstRectangle.Top, 0)
                , color, new Vector2(1,1));
            vertices[vertexCount++] = new VertexPositionColorTexture(
                new Vector3(dstRectangle.Right, dstRectangle.Top, 0)
                , color, new Vector2(0, 1));
            vertices[vertexCount++] = new VertexPositionColorTexture(
                new Vector3(dstRectangle.Right, dstRectangle.Bottom, 0)
                , color, new Vector2(0, 0));
            vertices[vertexCount++] = new VertexPositionColorTexture(
                new Vector3(dstRectangle.Left, dstRectangle.Bottom, 0)
                , color, new Vector2(1, 0));

            //  we premultiply all vertices times the world matrix.
            //  the world matrix changes alot and we don't want to have to flush
            //  every time it changes.
            Matrix world = this.World;
            for (int i = vertexCount - 4; i < vertexCount; i++)
                Vector3.Transform(ref vertices[i].Position, ref world, out vertices[i].Position);
        }    

        public void Flush()
        {
            if (this.vertexCount > 0)
            {
                if (this.declaration == null || this.declaration.IsDisposed)
                    this.declaration = new VertexDeclaration(device, VertexPositionColorTexture.VertexElements);

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

                    device.DrawUserIndexedPrimitives<VertexPositionColorTexture>(
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