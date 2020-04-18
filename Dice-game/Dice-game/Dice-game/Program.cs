using Dice_game.Infrastructure;
using Dice_game.PlayerDomain;
using System;
using System.Collections.Generic;

namespace Dice_game
{
    class Program
    {
        static void Main(string[] args)
        {

            var player1 = new Player(PlayerType.Human);



            while (1 == 1)
            {
                player1.NextAction();
            }
        }
    }
}
