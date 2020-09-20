using Dice_game.Infrastructure;
using Dice_game.Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Probability_calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            /*using StreamWriter patternProbabilitiesFile = new StreamWriter(
                @"C:\Coding\Dice-game\Dice-game\Dice-game\Dice-game\Database\PatternProbabilitiesAnalytical.txt");

            var patterns = GetPatterns();
            var p = new Dictionary<string, decimal>();
            var f = new Dictionary<string, decimal>();

            var possibleRolls = new Dictionary<int, List<int[]>>
            {
                { 0, GetRollsForDice(0) },
                { 1, GetRollsForDice(1) },
                { 2, GetRollsForDice(2) },
                { 3, GetRollsForDice(3) },
                { 4, GetRollsForDice(4) },
                { 5, GetRollsForDice(5) },
                { 6, GetRollsForDice(6) }
            };

            // This generates probabilities to get all possible subsets of all possible patterns for 1 roll
            foreach (var pt in patterns)
            {
                var hashSet = new HashSet<int[]>(new ArrayEqualityComparer());
                var subsets = GetSubsets(pt.pattern);

                foreach (var s in subsets)
                {
                    if (s.Length >= 1 && s[0] == 2)
                    {
                        var br = true;
                    }

                    var id = GetPatternId(pt.pattern, s, pt.rollingDiceCount, 1);
                    var prob = GetPatternProbability(pt.pattern, s, possibleRolls[pt.rollingDiceCount]);
                    p[id] = prob;

                    foreach (var ss in GetSubsets(s))
                    {
                        id = GetPatternId(s, ss, s.Length, 1);
                        prob = GetPatternProbability(s, ss, possibleRolls[s.Length]);
                        p[id] = prob;
                    }
                    
                    
                }


            }

            foreach (var pt in patterns)
            {
                Console.WriteLine(GetPatternId(pt.pattern, pt.rollingDiceCount, 2));
                F(pt.pattern, pt.pattern, pt.rollingDiceCount, 100, p, f);
            }

            var ppp = new Dictionary<string, decimal[]>(patterns.Count);
            foreach (var pt in GetPatterns(true))
            {
                ppp[$"{string.Join("", GameUtility.CreateGenericPatternFromDice(pt.pattern))} {pt.rollingDiceCount}"] = new decimal[100];
            }

            foreach (var pair in f)
            {
                var key = pair.Key.Split("-");
                var ptr = new List<int>();
                var ptrKey = key[0].ToCharArray().Select(x => x - '0').ToArray();

                ppp[$"{string.Join("", GameUtility.CreateGenericPatternFromDice(ptrKey))} {key[1]}"][int.Parse(key[2]) - 1] = Math.Round(pair.Value, 6);
            }

            foreach (var pair in ppp)
            {
                patternProbabilitiesFile.WriteLine($"{pair.Key} {string.Join(" ", pair.Value)}");
                patternProbabilitiesFile.Flush();
            }*/

            var y = 0;

            using StreamWriter patternProbabilitiesFileDiff = new StreamWriter(
                @"C:\Coding\Dice-game\Dice-game\Dice-game\Dice-game\Database\PatternProbabilitiesDiff.txt");

            var probNumerical = File.ReadAllLines(@"C:\Coding\Dice-game\Dice-game\Dice-game\Dice-game\Database\PatternProbabilities.txt");
            var probAnalytical = File.ReadAllLines(@"C:\Coding\Dice-game\Dice-game\Dice-game\Dice-game\Database\PatternProbabilitiesAnalytical.txt");

            for (int i = 0; i < probNumerical.Length; i++)
            {
                var diffs = new List<string>();
                var probNumericalValues = probNumerical[i].Split(" ");
                var probAnalyticalValues = probAnalytical[i].Split(" ");
                diffs.Add(probNumericalValues[0]);
                diffs.Add(probNumericalValues[1]);
                for (int j = 2; j < probNumericalValues.Length; j++)
                {
                    diffs.Add((decimal.Parse(probNumericalValues[j]) -
                        decimal.Parse(probAnalyticalValues[j])).ToString());
                }

                patternProbabilitiesFileDiff.WriteLine(string.Join(" ", diffs));
            }

            /*var r = new List<int[]>();
            GetRollsForDice(3, r);

            var s = 0;
            foreach (var e in r)
            {
                var m = 0;
                foreach (var f in e)
                {
                    if (f == 1)
                    {
                        m++;
                    }
                }

                if (m == 0)
                {
                    s++;
                }
            }

            var x = (decimal)s / r.Count;
            var y = 0;*/


            /*var charA = new char[] { '1', '2', '3', '4', '5', '6' };
            int d = 6;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (var n = 0; n < 50000000; n++)
            {
                var key = new string(charA) + d.ToString();
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs);

            watch = System.Diagnostics.Stopwatch.StartNew();
            for (var n = 0; n < 50000000; n++)
            {
                //var key = new string(charA) + d.ToString();
                var key = new StringBuilder(7).Append(charA).Append(d.ToString()).ToString();
            }
            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs);*/

            //var key = new StringBuilder(7).Append(patternOfMissingDice).Append(rollingDiceCount.ToString()).ToString();

            /*var pattern = new[] { 1, 2, 3, 4, 5, 6 };
            var currentDice = new[] { 4, 2, 2, 4, 2, 4 };
            var N = 1000000;
            var random = new Random();

            var combinationList = new CombinationList();
            combinationList.CreateFullListOfAllPossibleRollsWithPossibleCombinations();

            var combination = new Combination(CombinationType.Threes)
            {
                Dice = new int[] { 3, 3, 3 }
            };

            var x = combinationList.LookupMatchingCombinations(new int[] { 3, 3, 3, 3, 4, 4 });

            var ppp = new PatternList();
            ppp.CreatePatternProbabilities(1);

            var p = new Player(PlayerType.Human)
            {
                TotalNumberOfRolls = 10000
            };

            var patterns = GameUtility.GetPatterns();
            while (p.TotalNumberOfRolls > 0)
            {
                p.RollDice();
                
                foreach (var c in combinationList.Combinations)
                {
                    var m = c.MissingDiceToCompleteCombination(p.CurrentDice);

                    var pat = GameUtility.CreateGenericPatternFromDice(m);

                    if (!patterns.Contains(pat))
                    {
                        Console.WriteLine(string.Join(',', pat));
                    }                   
                }
            }

            var xx = GameUtility.CreateGenericPatternFromDice(new int[] { 4, 1, 4, 4, 1 });
            var yy = combination.MissingDiceToCompleteCombination(new int[] { 2, 1, 3, 3, 5, 1 });

            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (var n = 0; n < N; n++)
            {
                for (var i = 0; i < currentDice.Length; i++)
                {
                    currentDice[i] = random.Next(1, 7);
                }
                combinationList.LookupMatchingCombinations(currentDice);
            }
            foreach (var x in combinationList.LookupCombination(currentDice))
            {
                Console.WriteLine(x.CombinationType);
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs);


            elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs);*/
        }

        static decimal F(int[] pattern, int[] subPattern, int rollingDiceCount, int rollCount, Dictionary<string, decimal> p, Dictionary<string, decimal> f)
        {
            // Get pattern id
            var pid = GetPatternId(pattern, subPattern, rollingDiceCount, 1);
            var fid = GetPatternId(pattern, rollingDiceCount, rollCount);

            /*var charPattern = GameUtility.CreateGenericPatternFromDice(pattern);
            var charSubPattern = GameUtility.CreateGenericPatternFromDice(subPattern);
            var newPattern = charPattern.Select(x => x - 96).ToArray();
            var newSubPattern = charPattern.Select(x => x - 96).ToArray();*/

            if (fid == "aab-3-2")
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
            var br = false;
            if (pattern.Sum() == 4 && subPattern.Sum() == 1 && pattern.Length == 3)
            {
                br = true;
            }
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
                        if (br) Console.WriteLine(string.Join(" ", rolledDice));
                        s++;
                    }
                }
                probability = s / possibleRolls.Count;
            }

            return probability;
        }

        static List<(int rollingDiceCount, int[] pattern)> GetPatterns(bool ordered = false)
        {
            if (ordered)
            {
                return new List<(int rollingDiceCount, int[] pattern)>
                {
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
                };
            }
            else
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
                };
            }        
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
            var charPattern = GameUtility.CreateGenericPatternFromDice(pattern);
            var charSubPattern = GameUtility.CreateGenericPatternFromDice(subPattern);
            var newPattern = charPattern.Select(x => x - 96).ToArray();
            var newSubPattern = charSubPattern.Select(x => x - 96).ToArray();

            return $"{string.Join("", pattern)}-{string.Join("", subPattern)}-{rollingDiceCount}-{rollsCount}";
        }

        static string GetPatternId(int[] pattern, int rollingDiceCount, int rollsCount)
        {
            var charPattern =  GameUtility.CreateGenericPatternFromDice(pattern);
            var newPattern = charPattern.Select(x => x - 96).ToArray();
            return $"{string.Join("", pattern)}-{rollingDiceCount}-{rollsCount}";
        }

        static void GetProbabilitiesForAllPatterns()
        {
            using StreamWriter patternProbabilitiesFile = new StreamWriter(
                @"C:\Coding\Dice-game\Dice-game\Dice-game\Dice-game\Database\PatternProbabilities.txt");

            for (int i = 0; i < 35; i++)
            {
                var results = ComputePatternProbabilities(i, 100, 10000, 10000);
                patternProbabilitiesFile.WriteLine(results);
                patternProbabilitiesFile.Flush();
            }
        }

        /// <summary>
        /// Manual method to compute pattern probablity.
        /// </summary>
        /// <param name="index"></param>
        static string ComputePatternProbabilities(int index, int numberOfRolls, int numberOfRounds, int numberOfIterations)
        {
            // TODO: Add comments

            // These represent all possible patterns of missing to dice to finish any combination. Note that it doesn't matter
            // what dice values we choose to represent the patterns.
            // Also, there is a special case for combinations ones through sixes where any of these combinations can be completed
            // using three to all six dice. This means we need to compute probabilities, i.e. get value of 6 with 4 remaining dice
            // Format of the dictionary: <remainingDice, pattern>
            var patterns = new List<(int rollingDiceCount, int[] pattern)>(36)
            {
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
            };

            var probabilities = new List<decimal[]>(numberOfRolls);
            var random = new Random();

            var rollingDiceCount = patterns[index].rollingDiceCount;
            var diceToGet = patterns[index].pattern;
            var c = new Combination(CombinationType.Ones) // Doesn't matter what combination type we use
            {
                Dice = diceToGet
            };

            // How many rolls a player has (I will probably cap it ~100)
            for (int i = 0; i < numberOfRolls; i++)
            {
                // For each # of available rolls
                probabilities.Add(new decimal[numberOfRounds]);

                // We run the expiriment 'numberOfIterations' times for 'numberOfRounds' times (average per round)
                // Basically - how many times we succeed in 'numberOfIterations' attempts
                // averageg over 'numberOfRounds'
                for (int r = 0; r < numberOfRounds; r++)
                {
                    probabilities[i][r] = 0;
                    for (int j = 0; j < numberOfIterations; j++)
                    {
                        // total number of rolls available
                        var roll = i + 1;
                        var matchedDiceNumber = 0;
                        c.Dice = diceToGet;

                        while (roll > 0)
                        {
                            roll--;
                            int[] currentDice = Enumerable.Repeat(0, rollingDiceCount - matchedDiceNumber)
                                                    .Select(i => random.Next(1, 7))
                                                    .ToArray();
                            // Check what dice is missing to complete the combination
                            c.Dice = c.MissingDiceToCompleteCombination(currentDice);

                            // If no dice is missing then add one to the list of completed runs
                            if (c.Dice.Length == 0)
                            {
                                probabilities[i][r]++;
                                break;
                            }

                            matchedDiceNumber = diceToGet.Length - c.Dice.Length;
                        }
                    }

                    // Compute how many we succeeded withing 'numberOfIterations' attempts
                    probabilities[i][r] = probabilities[i][r] / numberOfIterations;
                }
                Console.WriteLine($"Pattern: {index + 1} / {patterns.Count}, Roll: {i + 1} / {numberOfRolls} completed");
            }

            var result = new StringBuilder();
            result.Append(GameUtility.CreateGenericPatternFromDice(diceToGet));
            result.Append($" {rollingDiceCount} ");

            // Average the success rates
            result.Append(string.Join(' ', probabilities.Select(x => Math.Round(x.Average(), 6))));

            return result.ToString();
        }

        static void CreatePatternProbabilities(int numberOfRolls)
        {
            using StreamWriter file = new StreamWriter(
                @"C:\Coding\Dice-game\Dice-game\Dice-game\Dice-game\Database\Patterns.txt");

            for (var r = 0; r < numberOfRolls; r++)
            {


                Console.WriteLine(string.Join(",", GenerateDice(2)));
            }
        }


        /// <summary>
        /// Creates a txt file with all possible rolls we can get and all possible matching combinations.
        /// </summary>
        static void CreateFullListOfAllPossibleRollsWithPossibleCombinations()
        {
            var dice = new int[] { 0, 0, 0, 0, 0, 0 }.ToList();
            var rollWithPossibleCombinations = new StringBuilder(56);
            Console.WriteLine(Directory.GetCurrentDirectory());
            using StreamWriter file = new StreamWriter(
                @"C:\Coding\Dice-game\Dice-game\Dice-game\Dice-game\Database\FullRollListWithCombinations.txt");

            // Load list of all possible combinations (dice patterns)
            var combinations = File.ReadAllLines(@"C:\Coding\Dice-game\Dice-game\Dice-game\Dice-game\Database\Combinations.txt");

            var combinationList = combinations.Select(c =>
            {
                var temp = c.Split(" ");
                var dicePattern = temp[0].ToCharArray().Select(x => x - '0').ToArray();
                Enum.TryParse(temp[1], true, out CombinationType combinationType);

                return new Combination(combinationType, dicePattern);
            }).ToArray();

            // Maps dice (combination) pattern to a combination object
            var combinationsLookup = combinationList.ToLookup(x => x.Dice, x => x, new ArrayEqualityComparer());

            for (var a = 1; a <= 6; a++)
            {
                dice[0] = a;
                for (var b = 1; b <= 6; b++)
                {
                    dice[1] = b;
                    for (var c = 1; c <= 6; c++)
                    {
                        dice[2] = c;
                        for (var d = 1; d <= 6; d++)
                        {
                            dice[3] = d;
                            for (var e = 1; e <= 6; e++)
                            {
                                dice[4] = e;
                                for (var f = 1; f <= 6; f++)
                                {
                                    dice[5] = f;

                                    rollWithPossibleCombinations.Append(string.Join("", dice));
                                    var uniqueCombinations = new HashSet<int[]>(new ArrayEqualityComparer());

                                    // Create all possible dice subsets of length 3 or greater
                                    for (var i = 3; i <= 6; i++)
                                    {
                                        for (var j = 0; j <= 6 - i; j++)
                                        {
                                            uniqueCombinations.Add(dice.OrderBy(x => x).ToList().GetRange(j, i).ToArray());
                                        }
                                    }

                                    // Check if a subset is a combination
                                    foreach (var z in uniqueCombinations)
                                    {
                                        if (combinationsLookup.Contains(z))
                                        {
                                            rollWithPossibleCombinations.Append($" {string.Join("", z)}");
                                        }
                                    }

                                    file.WriteLine(rollWithPossibleCombinations);
                                    rollWithPossibleCombinations.Clear();
                                }
                            }
                        }
                    }
                }
            }
        }




        static bool IsUniqueSubset(int[] set, int[] subset)
        {
            var counts = new Dictionary<int, int>(set.Length);
            // Assign counts
            foreach (var e in set)
            {
                if (counts.ContainsKey(e))
                {
                    counts[e]++;
                }
                else
                {
                    counts[e] = 1;
                }
            }

            foreach (var e in subset)
            {
                if (counts.ContainsKey(e))
                {
                    counts[e]--;
                    if (counts[e] < 0)
                    {
                        return false;
                    }
                }   
            }

            foreach (var e in counts)
            {
                if (counts[e.Key] != 0)
                {
                    return false;
                }
            }

            return true;
        }


        static char[] GenerateDice(int len)
        {
            var random = new Random();

            var rolledDice = new char[len];

            for (var i = 0; i < rolledDice.Length; i++)
            {
                rolledDice[i] = (char)(random.Next(0, 6) + 'a');
            }

            return rolledDice;
        }


    }
}
