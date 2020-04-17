using Dice_game.Infrastructure;
using Dice_game.PlayerDomain;
using System;

namespace Dice_game
{
    class Program
    {
        static void Main(string[] args)
        {

            var player1 = new Player(PlayerType.Human);

            var com = new CombinationList();

            var x = com.LookupCombination(new int[] { 4, 4, 4, 4 });

            while (1 == 1)
            {
                player1.NextAction();
            }
        }
    }
}
