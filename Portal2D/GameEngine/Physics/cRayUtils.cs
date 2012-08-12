using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Portal2D.GameEngine.Physics
{
    class cRayUtils
    {

        /// <summary>
        /// Finds the intersection point of two lines
        /// Taken from Real time collision detection p152 (modified by mike.cann@gmail.com)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static cRayCollision intersects(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            float a1 = Signed2DTriArea(a, b, d);
            float a2 = Signed2DTriArea(a, b, c);

            float t=0;
            cRayCollision p = new cRayCollision();

            p.c = c;
            p.d = d;

            Vector2 v = c-d;
            v.Normalize();
            float tmp = v.X;
            v.X = v.Y;
            v.Y = tmp;

            p.normal = v;
       
            if (a1 * a2 < 0)
            {
                float a3 = Signed2DTriArea(c, d, a);
                float a4 = a3 + a2 - a1;

                if (a3 * a4 < 0)
                {          
                    t=a3/(a3-a4);
                    p.collision = true;
                    p.where.X = a.X + t * (b.X - a.X);
                    p.where.Y = a.Y + t * (b.Y - a.Y);
                }
                else
                {                
                    t=a3/(a3-a4);
                    p.collision = false;
                    p.where.X = a.X + t * (b.X - a.X);
                    p.where.Y = a.Y + t * (b.Y - a.Y);
                }
            }

            return p;         
        }
       
        /// <summary>
        /// Finds the winding direction of a triangle
        /// Taken from Real time collision detection p152
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private static float Signed2DTriArea(Vector2 a, Vector2 b, Vector2 c)
        {
            return ((a.X - c.X) * (b.Y - c.Y)) - ((a.Y - c.Y) * (b.X - c.X));
        }
    }
}
