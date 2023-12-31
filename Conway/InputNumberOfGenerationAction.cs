﻿namespace Conway;

public class InputNumberOfGenerationAction : IAction
{
    public string Message { get; }
    private IConsoleFacade Console { get; }

    public InputNumberOfGenerationAction(IConsoleFacade console)
    {
        Message = "Specify number of generation";
        Console = console;
    }
    
    public ActionResult Execute(GameState currentGameState)
    {
        var numGen = currentGameState.NumGen;
        var validInput = false;
        while (!validInput)
        {
            validInput = TryGetNumGen(currentGameState, out numGen);
        }
        
        return new ActionResult(currentGameState with {NumGen = numGen}, new BackToMenuAction());
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