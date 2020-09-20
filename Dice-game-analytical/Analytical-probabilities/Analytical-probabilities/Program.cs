using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Analytical_probabilities
{
    class Program
    {
        static void Main(string[] args)
        {
            var charPattern = CreateGenericPatternFromDice(new int[] { 2, 3 });
            var charSubPattern = CreateGenericPatternFromDice(new int[] { 2, 3 });
            var newPattern = charPattern.Select(x => x - 96).ToArray();
            var newSubPattern = charPattern.Select(x => x - 96).ToArray();

            Console.WriteLine(string.Join(",", newPattern));
        }

        static decimal F(int[] pattern, int[] subPattern, int rollingDiceCount, int rollCount, Dictionary<string, decimal> p, Dictionary<string, decimal> f)
        {
            // Get pattern id
            var pid = GetPatternId(pattern, subPattern, rollingDiceCount, 1);
            var fid = GetPatternId(pattern, rollingDiceCount, rollCount);

            if (fid == "a-3-2")
            {
                var aaa = 0;
            }


            if (pattern.Length == 0)
            {
                f[fid] = 1;
                return f[fid];
            }

            if (f.ContainsKey(fid))
            {
                return f[fid];
            }

            if (rollCount == 1)
            {
                f[fid] = p[pid];
                return f[fid];
            }

            decimal probability = 0;
            foreach (var s in GetSubsets(pattern))
            {
                var c = GetComplement(pattern, s).ToArray();
                var pid_tmp = GetPatternId(pattern, s, rollingDiceCount, 1);
                var pr = p[pid_tmp];
                var fid_tmp = GetPatternId(c, rollingDiceCount - s.Length, rollCount - 1);
                var fr = (f.ContainsKey(fid_tmp) ? f[fid_tmp] : F(c, c, rollingDiceCount - s.Length, rollCount - 1, p, f));
                probability += p[pid_tmp] * fr;
            }
            f[fid] = probability;

            return f[fid];
        }

        static decimal GetPatternProbability(int[] pattern, int[] subPattern, List<int[]> possibleRolls)
        {
            decimal probability;
            if (pattern.Length == 0)
            {
                probability = 1;
            }
            else
            {
                decimal s = 0;
                foreach (var rolledDice in possibleRolls)
                {
                    if (IsProperSubset(pattern, rolledDice, subPattern))
                    {
                        s++;
                    }
                }
                probability = s / possibleRolls.Count;
            }

            return probability;
        }

        static void GetExactProbabilities()
        {
            var patterns = GetPatterns();

            var r = new Dictionary<int, List<int[]>>
            {
                { 1, GetRollsForDice(1) },
                { 2, GetRollsForDice(2) },
                { 3, GetRollsForDice(3) },
                { 4, GetRollsForDice(4) },
                { 5, GetRollsForDice(5) },
                { 6, GetRollsForDice(6) }
            };

            foreach (var p in patterns)
            {
                decimal probability;
                if (p.pattern.Length == 0)
                {
                    probability = 1;
                }
                else
                {
                    decimal s = 0;
                    foreach (var rolledDice in r[p.rollingDiceCount])
                    {
                        if (IsProperSubset(p.pattern, rolledDice, p.pattern))
                        {
                            s++;
                        }
                    }
                    probability = s / r[p.rollingDiceCount].Count;
                }

                Console.WriteLine($"Dice count: {p.rollingDiceCount} - {string.Join(",", p.pattern)}: {probability} ");
            }
        }

        static List<(int rollingDiceCount, int[] pattern)> GetPatterns()
        {
            return new List<(int rollingDiceCount, int[] pattern)>
            {
                ( 0, new int[] { } ),

                ( 1, new int[] { } ),
                ( 1, new int[] { 1 } ),

                ( 2, new int[] { } ),
                ( 2, new int[] { 1 } ),
                ( 2, new int[] { 1, 1 } ),
                ( 2, new int[] { 1, 2 } ),

                ( 3, new int[] { } ),
                ( 3, new int[] { 1 } ),
                ( 3, new int[] { 1, 1 } ),
                ( 3, new int[] { 1, 1, 1 } ),
                ( 3, new int[] { 1, 2, 3 } ),
                ( 3, new int[] { 1, 1, 2 } ),

                ( 4, new int[] { 1 } ),
                ( 4, new int[] { 1, 1 } ),
                ( 4, new int[] { 1, 1, 1 } ),
                ( 4, new int[] { 1, 2, 3, 4 } ),
                ( 4, new int[] { 1, 1, 1, 1 } ),
                ( 4, new int[] { 1, 1, 1, 2 } ),
                ( 4, new int[] { 1, 1, 2, 2 } ),
                ( 4, new int[] { 1, 1, 2, 3 } ),

                ( 5, new int[] { 1, 1 } ),
                ( 5, new int[] { 1, 1, 1 } ),
                ( 5, new int[] { 1, 1, 1, 1 } ),
                ( 5, new int[] { 1, 1, 1, 1, 1 } ),
                ( 5, new int[] { 1, 2, 3, 4, 5 } ),
                ( 5, new int[] { 1, 1, 2, 2, 3 } ),
                ( 5, new int[] { 1, 1, 1, 2, 2 } ),


                ( 6, new int[] { 1, 1, 1 } ),
                ( 6, new int[] { 1, 1, 1, 1 } ),
                ( 6, new int[] { 1, 1, 1, 1, 1 } ),
                ( 6, new int[] { 1, 1, 1, 1, 1, 1 } ),
                ( 6, new int[] { 1, 2, 3, 4, 5, 6 } ),
                ( 6, new int[] { 1, 1, 1, 2, 2, 2 } ),
                ( 6, new int[] { 1, 1, 2, 2, 3, 3 } )

                /*
                  ( 0, new int[] { } ),
                ( 1, new int[] { } ),
                ( 2, new int[] { } ),
                ( 3, new int[] { } ),

                ( 1, new int[] { 1 } ),
                ( 2, new int[] { 1 } ),
                ( 3, new int[] { 1 } ),
                ( 4, new int[] { 1 } ),

                ( 2, new int[] { 1, 1 } ),
                ( 3, new int[] { 1, 1 } ),
                ( 4, new int[] { 1, 1 } ),
                ( 5, new int[] { 1, 1 } ),

                ( 3, new int[] { 1, 1, 1 } ),
                ( 4, new int[] { 1, 1, 1 } ),
                ( 5, new int[] { 1, 1, 1 } ),
                ( 6, new int[] { 1, 1, 1 } ),

                ( 4, new int[] { 1, 1, 1, 1 } ),
                ( 5, new int[] { 1, 1, 1, 1 } ),
                ( 6, new int[] { 1, 1, 1, 1 } ),

                ( 5, new int[] { 1, 1, 1, 1, 1 } ),
                ( 6, new int[] { 1, 1, 1, 1, 1 } ),

                ( 6, new int[] { 1, 1, 1, 1, 1, 1 } ),

                // Any of the following ones can only complete a combination which consists of 6 dice
                ( 2, new int[] { 1, 2 } ),
                ( 3, new int[] { 1, 2, 3 } ),
                ( 4, new int[] { 1, 2, 3, 4 } ),
                ( 5, new int[] { 1, 2, 3, 4, 5 } ),
                ( 6, new int[] { 1, 2, 3, 4, 5, 6 } ),
                ( 3, new int[] { 1, 1, 2 } ),
                ( 4, new int[] { 1, 1, 1, 2 } ),
                ( 4, new int[] { 1, 1, 2, 2 } ),
                ( 5, new int[] { 1, 1, 2, 2, 3 } ),
                ( 5, new int[] { 1, 1, 1, 2, 2 } ),
                ( 6, new int[] { 1, 1, 1, 2, 2, 2 } ),
                ( 4, new int[] { 1, 1, 2, 3 } ),
                ( 6, new int[] { 1, 1, 2, 2, 3, 3 } )
                 */
            };
        }

        static bool IsProperSubset(int[] pattern, int[] rolledDice, int[] subPattern)
        {
            var patternTemp = (int[])pattern.Clone();
            var subPatternTemp = (int[])subPattern.Clone();
            // We need to check the following. Let's say the 'pattern' to get is [1,1,2,3] and we want to compute probability to get exactly subPattern [1,2],
            // meaning that [1,2,3] should not be counted because that would be part of the probability to get [1,2,3], etc.
            // In other words, rolled dice cannot contain any other dice from 'pattern' other than what's in 'subPattern'.

            // This could probably be done in more efficient way but doesn't matter for now
            foreach (var d in rolledDice)
            {
                // check if the rolled die is in combination and diceToget and mark it
                for (var i = 0; i < patternTemp.Length; i++)
                {
                    if (d == patternTemp[i])
                    {
                        patternTemp[i] = 0;
                        break;
                    }
                }

                for (var i = 0; i < subPatternTemp.Length; i++)
                {
                    if (d == subPatternTemp[i])
                    {
                        subPatternTemp[i] = 0;
                        break;
                    }
                }
            }

            if (subPatternTemp.Sum() == 0 && patternTemp.Count(d => d == 0) == subPatternTemp.Length)
            {
                return true;
            }

            return false;
        }

        static List<int[]> GetSubsets(int[] set)
        {
            return Subsets(set).Select(x => x.ToArray()).ToHashSet(new ArrayEqualityComparer()).ToList();
        }

        static IEnumerable<IEnumerable<T>> Subsets<T>(IEnumerable<T> set)
        {
            List<T> list = set.ToList();
            int length = list.Count;
            int max = (int)Math.Pow(2, list.Count);

            for (int count = 0; count < max; count++)
            {
                List<T> subset = new List<T>();
                uint rs = 0;
                while (rs < length)
                {
                    if ((count & (1u << (int)rs)) > 0)
                    {
                        subset.Add(list[(int)rs]);
                    }
                    rs++;
                }
                yield return subset;
            }
        }

        static List<int[]> GetRollsForDice(int diceCount)
        {
            if (diceCount == 0)
            {
                return new List<int[]>();
            }

            var v = new int[diceCount];
            var r = new List<int[]>
            {
                v.Select(e => e + 1).ToArray()
            };
            while (v.Sum() != v.Length * 5)
            {
                v[^1]++;
                for (var i = v.Length - 1; i >= 0; i--)
                {
                    if (v[i] >= 6 && i - 1 >= 0)
                    {
                        v[i - 1]++;
                    }
                    v[i] = v[i] % 6;
                }

                r.Add(v.Select(e => e + 1).ToArray());
            }

            return r;
        }

        static IEnumerable<int> GetComplement(int[] pattern, int[] subPattern)
        {
            var matches = new int[pattern.Length];

            for (int j = 0; j < subPattern.Length; j++)
            {
                for (int i = 0; i < pattern.Length; i++)
                {
                    if (subPattern[j] == pattern[i] && matches[i] != 1)
                    {
                        matches[i] = 1;
                        break;
                    }
                }
            }

            for (int i = 0; i < matches.Length; i++)
            {
                if (matches[i] == 0)
                {
                    yield return pattern[i];
                }
            }
            
        }

        static string GetPatternId(int[] pattern, int[] subPattern, int rollingDiceCount, int rollsCount)
        {
            return $"{string.Join("", CreateGenericPatternFromDice(pattern))}-{string.Join("", CreateGenericPatternFromDice(subPattern))}-{rollingDiceCount}-{rollsCount}";
        }

        static string GetPatternId(int[] pattern, int rollingDiceCount, int rollsCount)
        {
            return $"{string.Join("", CreateGenericPatternFromDice(pattern))}-{rollingDiceCount}-{rollsCount}";
        }

        public static char[] CreateGenericPatternFromDice(int[] dice)
        {
            // Dice combinations are converted into a generic patterns regardless of their actual value, i.e.
            // [4,5,5,5,1,3] => [a,a,a,b,c,d]
            // This makes it easy to find out what is a probability to complete each possible combination
            var pattern = new List<char>(dice.Length);
            var patternCharacters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f' };

            // Sort the dice by the frequencies of each value and substitute with an alphabet letter
            // This could probably be done in a more elegant way but I like the simplicity and the idea
            // of a 'math' substitution :-)
            var diceCounts = dice.GroupBy(x => x).OrderByDescending(x => x.Count()).ToArray();

            for (var i = 0; i < diceCounts.Count(); i++)
            {
                pattern.AddRange(Enumerable.Repeat(patternCharacters[i], diceCounts[i].Count()));
            }

            return pattern.ToArray();
        }



    }



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


}
