﻿namespace Conway;

public class InputGridSizeAction : IAction
{
    public string Message { get; }
    private IConsoleFacade Console { get; }

    public InputGridSizeAction(IConsoleFacade console)
    {
        Message = "Specify grid size";
        Console = console;
    }
    
    public ActionResult Execute(GameState currentGameState)
    {
        var width = currentGameState.Width;
        var height = currentGameState.Height;
        
        var validInput = false;
        while (!validInput)
        {
            validInput = TryGetGridSize(currentGameState, out width, out height);
        }
        
        return new ActionResult(currentGameState with {Width = width, Height = height}, new BackToMenuAction());
    }

    private bool TryGetGridSize(GameState currentGameState, out int width, out int height)
    {
        Console.WriteLine("Please enter grid size in w h format (example: 10 15):");
        width = currentGameState.Width;
        height = currentGameState.Height;
        
        var line = Console.ReadLine();
        var dimensions = line.Split();

        if (dimensions.Length != 2)
        {
            Console.WriteLine("Wrong format for grid size!!!");
            return false;
        }

        var isValidWidth = int.TryParse(dimensions[0], out width);
        var isValidHeight = int.TryParse(dimensions[1], out height);
        if (!isValidWidth || !isValidHeight)
        {
            Console.WriteLine("Width and Height must be numerical!!!");
            return false;
        }

        if (width < 1 || width > 25 || height < 1 || height > 25)
        {
            Console.WriteLine("Width and Height must be between 1 and 25 inclusive!!!");
            return false;
        }

        return true;
    }
}