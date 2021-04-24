using System;

namespace aDevLib
{
    public static class SRandom
    {
        static readonly Random RandomInstance = new Random();

        public static int Next(int inclusiveMinValue, int exclusiveMaxValue)
        {
            lock (RandomInstance)
                return RandomInstance.Next(inclusiveMinValue, exclusiveMaxValue);
        }

        public static int Next(int exclusiveMaxValue)
        {
            lock (RandomInstance)
                return RandomInstance.Next(exclusiveMaxValue);
        }

        public static int Next()
        {
            lock (RandomInstance)
                return RandomInstance.Next();
        }

        /// <summary>
        /// Returns a random floating-point number between 0.0 and 1.0.
        /// </summary>
        /// <returns>A double-precision floating point number greater than or equal to 0.0, and less than 1.0.</returns>
        public static double NextDouble()
        {
            lock (RandomInstance)
                return RandomInstance.NextDouble();
        }

        /// <summary>
        /// Returns a random floating-point number between rangeMin and rangeMax.
        /// </summary>
        /// <param name="rangeMin">Minimum possible random value</param>
        /// <param name="rangeMax">Maximum possible random value</param>
        /// <returns>A double-precision floating point number greater than or equal to rangeMin, and less than rangeMax.</returns>
        public static double NextDouble(double rangeMin, double rangeMax)
        {
            lock (RandomInstance)
                return rangeMin + (rangeMax - rangeMin) * NextDouble();
        }

        /// <inheritdoc cref="Random.NextBytes"/>
        public static void NextBytes(byte[] array)
        {
            lock (RandomInstance)
                RandomInstance.NextBytes(array);
        }
    }
}