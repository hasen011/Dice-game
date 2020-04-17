﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Dice_game.PlayerDomain
{
    public class Board
    {
        public Dictionary<CombinationType, Combination> CurrentBoard { get; set; }
        public Board()
        {
            CurrentBoard = ((CombinationType[])Enum.GetValues(typeof(CombinationType)))
                .Where(x => x != CombinationType.Unknown)
                .ToDictionary(
                    x => x,
                    x => new Combination(x)
                );
        }
    }
}