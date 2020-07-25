using System;
using System.Collections.Generic;
using System.Linq;

namespace Dice_game.PlayerDomain
{
    public class Combination
    {
        public CombinationType CombinationType { get; set; }
        public int[] Dice { get; set; }     
        public int Score { get; set; }
        public bool Completed { get; set; }
        public decimal ProbabilityToComplete { get; set; }

        public Combination(CombinationType combinationType)
        {
            CombinationType = combinationType;
            Completed = false;
        }

        public Combination(CombinationType combinationType, int[] dice)
        {
            CombinationType = combinationType;
            Dice = dice;
            Score = dice.Sum();
            Completed = false;
        }

        public void AssignDice(int[] dice)
        {
            Dice = dice;
            Score = dice.Sum();
        }

        public int[] MissingDiceToCompleteCombination(int[] rolledDice)
        {
            // It is assumed that the combination dice are alredy sorted (I created them that way)
            // Also, it is assumed that rolledDice.Length >= Dice.Length
            var sortedDice = rolledDice.OrderBy(x => x).ToArray();

            var result = new List<int>(Dice.Length);

            int m = sortedDice.Length, m0 = 0, n = Dice.Length, n0 = 0;
            while (1 == 1)
            {
                // If we reached the end of combination Dice, then we can stop
                if (n0 == n)
                {
                    break;
                }

                // If we reached end of rolled dice or the rolled die value is greater than the Die,
                // then we can no longer match that Die
                if (m0 == m || sortedDice[m0] > Dice[n0])
                {
                    result.Add(Dice[n0]);
                    n0++;                 
                }
                // Move to the next rolled die if the value is lower than the one we need
                else if (sortedDice[m0] < Dice[n0])
                {
                    m0++;
                }
                // If we find a match then move both indexes
                else if (sortedDice[m0] == Dice[n0])
                {
                    n0++;
                    m0++;
                }
            }

            return result.ToArray();
        }

        
    }
}
