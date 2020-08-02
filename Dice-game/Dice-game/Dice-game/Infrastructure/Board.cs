using Dice_game.Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dice_game.Infrastructure
{
    public class Board
    {
        public Dictionary<CombinationType, Combination> CurrentBoard { get; set; }
        public int TotalScore { get; set; }
        public Board()
        {
            CurrentBoard = ((CombinationType[])Enum.GetValues(typeof(CombinationType)))
                .Where(x => x != CombinationType.Unknown)
                .ToDictionary(
                    x => x,
                    x => new Combination(x)
                );

            TotalScore = 0;
        }
    }
}
