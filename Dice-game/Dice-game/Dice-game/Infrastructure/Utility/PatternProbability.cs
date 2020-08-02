namespace Dice_game.Game.Infrastructure.Utility
{
    public class PatternProbability
    {
        PatternProbability()
        {
            Id = new string(Pattern) + RollingDiceCount.ToString();
        }

        // Id is created as pattern + number of remaining dice to roll to finish a given combination
        public string Id { get; set; }
        public char[] Pattern { get; set; }
        // The nuimber of dice left to roll to complete a combination
        public int RollingDiceCount { get; set; }
        public decimal[] PatternProbabilities { get; set; }
    }
}
