﻿namespace Conway;

public class InputNumberOfGenerationAction : IAction
{
    private IConsoleFacade Console { get; }

    public InputNumberOfGenerationAction(IConsoleFacade console)
    {
        Console = console;
    }
    
    public ActionResult Execute(GameState currentGameState)
    {
        int numGen;
        while (!TryGetNumGen(currentGameState, out numGen))
        {
        }
        
        return new ActionResult(currentGameState with {NumGen = numGen}, new DisplayMenuAction(Console));
    }

    private bool TryGetNumGen(GameState currentGameState, out int numGen)
    {
        Console.WriteLine("Please enter the number of generation (3-20):");
        
        numGen = currentGameState.NumGen;
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