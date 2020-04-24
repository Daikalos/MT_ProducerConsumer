using System;

namespace Multithreading_03
{
    class StaticRandom
    {
        private static readonly Random myRandom = new Random();
        private static readonly object mySyncLock = new object();

        public static int RandomNumber(int min, int max)
        {
            lock (mySyncLock)
            { // synchronize
                return myRandom.Next(min, max);
            }
        }

        public static double RandomDouble()
        {
            lock (mySyncLock)
            { // synchronize
                return myRandom.NextDouble();
            }
        }
    }
}
