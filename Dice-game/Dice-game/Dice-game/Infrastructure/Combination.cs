using System.Linq;

namespace Dice_game.PlayerDomain
{
    public class Combination
    {
        public CombinationType CombinationType { get; set; }
        public int[] Dice { get; set; }     
        public int Score { get; set; }
        public bool Completed { get; set; }

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
    }
}
