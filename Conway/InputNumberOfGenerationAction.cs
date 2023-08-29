﻿namespace Conway;

public class InputNumberOfGenerationAction : IAction
{
    private IConsoleFacade Console { get; }
    private GameState CurrentGameState { get; }
    public InputNumberOfGenerationAction(IConsoleFacade console, GameState currentGameState)
    {
        Console = console;
        CurrentGameState = currentGameState;
    }

    public ActionResult Execute()
    {
        int numGen;
        while (!TryGetNumGen(out numGen))
        {
        }
        
        return new ActionResult(CurrentGameState with {NumGen = numGen}, new DisplayMenuAction(Console, CurrentGameState with {NumGen = numGen}));
    }

    private bool TryGetNumGen(out int numGen)
    {
        Console.WriteLine("Please enter the number of generation (3-20):");
        
        numGen = CurrentGameState.NumGen;
        var input = Console.ReadLine();
        var inputTokens = input.Split();

        if (inputTokens.Length != 1)
        {
            Console.WriteLine("Wrong format for number of generation!");
            return false;
        }

        if (!int.TryParse(inputTokens[0], out numGen))
        {
            Console.WriteLine("Number of generation must be numerical!");
            return false;
        }

        if (numGen is < 3 or > 20)
        {
            Console.WriteLine("Number of generation must be between 3 and 20 inclusive!");
            return false;
        }

        return true;
    }
}