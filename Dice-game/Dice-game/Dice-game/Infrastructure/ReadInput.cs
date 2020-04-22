using System;

namespace Dice_game.Infrastructure
{
    public class ReadInput
    {
        public virtual string GetNextInput()
        {
            return Console.ReadLine();
        }
    }
}
