using System.Collections.Generic;

namespace Dice_game.Infrastructure.Utility
{
    // Custom comparer to allow arrays as keys in Dictionaries/Lookups
    public class ArrayEqualityComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[] x, int[] y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(int[] obj)
        {
            // Hash code obtained here: https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-overriding-gethashcode
            int result = 17;
            for (int i = 0; i < obj.Length; i++)
            {
                unchecked // Overflow is fine, just wrap
                {
                    result = result * 23 + obj[i];
                }
            }
            return result;
        }
    }

    public class ArrayEqualityComparerChar : IEqualityComparer<char[]>
    {
        public bool Equals(char[] x, char[] y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(char[] obj)
        {
            // Hash code obtained here: https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-overriding-gethashcode
            int result = 17;
            for (int i = 0; i < obj.Length; i++)
            {
                unchecked // Overflow is fine, just wrap
                {
                    result = result * 23 + obj[i];
                }
            }
            return result;
        }
    }
}
