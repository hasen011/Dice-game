namespace Dice_game.PlayerDomain
{
    public enum PlayerAction
    {
        InvalidAction = 0,
        RollDice,
        FixDice,
        AssignCombination,
        ShowBoard,
        Yield,
        AssignDice, // This is used to manually override dice values
        EndGame
    }
}
