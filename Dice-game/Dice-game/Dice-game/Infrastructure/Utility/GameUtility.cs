using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dice_game.Infrastructure.Utility
{
    public static class GameUtility
    {
        public static char[] CreateGenericPatternFromDice(int[] dice)
        {
            var pattern = new List<char>(dice.Length);
            var patternCharacters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f' };

            var diceCounts = dice.GroupBy(x => x).OrderByDescending(x => x.Count()).ToArray();

            for (var i = 0; i < diceCounts.Count(); i++)
            {
                pattern.AddRange(Enumerable.Repeat(patternCharacters[i], diceCounts[i].Count()));
            }

            return pattern.ToArray();
        }

        public static HashSet<char[]> GetPatterns()
        {
            // Load list of all possible generic patterns
            var patterns = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), @"Infrastructure\Database\Patterns.txt"));

            var set = patterns.Select(x => x.ToCharArray()).ToHashSet(new ArrayEqualityComparerChar());
            set.Add(new char[] { });
            return set;
        }
    }




}
