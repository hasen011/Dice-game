using Dice_game.Infrastructure.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Dice_game.Infrastructure
{
    public class Combination
    {
        public CombinationType CombinationType { get; set; }
        public int[] Dice { get; set; }     
        public int Score { get; set; }
        public bool Completed { get; set; }
        public int[] DiceToCompleteCombination { get; set; }
        public decimal[] ProbabilitiesToCompleteCombinationWithinAllAvailableRolls { get; set; }
        public decimal[] ExpectedValuesForCombinationWithinAllAvailableRolls { get; set; }

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
            // If nothing was rolled then just return the combination's dice
            if (rolledDice.Length == 0)
            {
                DiceToCompleteCombination = Dice;
                return DiceToCompleteCombination;
            }

            // It is assumed that the combination dice are already sorted (I created them that way)
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

            // Set 'DiceToCompleteCombination' when this method is called
            DiceToCompleteCombination = result.ToArray();
            return DiceToCompleteCombination;
        }

        public void SetProbabilitiesAndEVToCompleteCombinationWithinAllAvailableRolls(int maximumNumberOfRolls, int[] rolledDice, Dictionary<string, decimal[]> patternProbabilities)
        {
            var patternOfMissingDice = GameUtility.CreateGenericPatternFromDice(MissingDiceToCompleteCombination(rolledDice));

            // Number of dice the player have left to roll to complete a combination
            var rollingDiceCount = 6 - (Dice.Length - DiceToCompleteCombination.Length);

            // Find the corresponding pattern probability for how many dice a player has left to roll
            var key = new string(patternOfMissingDice) + rollingDiceCount;
            var probabilitiesForAvailableRolls = patternProbabilities[key].Take(maximumNumberOfRolls).ToArray();
            ProbabilitiesToCompleteCombinationWithinAllAvailableRolls = probabilitiesForAvailableRolls;
            ExpectedValuesForCombinationWithinAllAvailableRolls = probabilitiesForAvailableRolls.Select(p => p * Score).ToArray();
        }


        
    }
}
