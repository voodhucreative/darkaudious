using System;
namespace Darkaudious.Helpers
{
    public static class Numbers
    {
        public static double GetNextRandom(int num)
        {
            Random rand = new Random();

            return rand.Next(0, num);
        }

        public static int GetNextRandom(int min, int max)
        {
            Random rand = new Random();

            return (int)rand.Next(min, max);
        }
    }
}

