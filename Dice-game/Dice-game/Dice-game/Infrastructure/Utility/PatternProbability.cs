namespace Dice_game.Infrastructure.Utility
{
    public class PatternProbability
    {
        public PatternProbability(char[] pattern, int rollingDiceCount, decimal[] patternProbabilities)
        {
            Id = GameUtility.GetPatternProbabilityId(pattern, rollingDiceCount);
            Pattern = pattern;
            RollingDiceCount = rollingDiceCount;
            PatternProbabilities = patternProbabilities;
        }

        // Id is created as pattern + number of remaining dice to roll to finish a given combination
        public string Id { get; }
        public char[] Pattern { get; }
        // The nuimber of dice left to roll to complete a combination
        public int RollingDiceCount { get; }
        public decimal[] PatternProbabilities { get; }
    }
}
