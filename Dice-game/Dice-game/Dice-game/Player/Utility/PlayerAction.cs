namespace Dice_game.PlayerDomain.Utility
{
    public enum PlayerAction
    {
        InvalidAction = 0,
        RollDice,
        FixDice,
        AssignCombination,
        ShowBoard,
        EndTurn,
        EndGame,
        EvaluateAllCombinations,

        AssignDice = 99, // This is used to manually override dice values
    }
}
