using System;
using System.Collections.Generic;
using System.Text;

namespace Portal2D.GameEngine.Utils
{
    public class cMath
    {
        private static Random _random;

        public static float RandRange(float from, float to)
        {
            if (_random == null) { _random = new Random(); }
            return _random.Next((int)(from * 1000), (int)(to * 1000)) / 1000f;
        }

        public static int RandRange(int from, int to)
        {
            if (_random == null) { _random = new Random(); }
            return _random.Next(from, to);
        }

    }
}
