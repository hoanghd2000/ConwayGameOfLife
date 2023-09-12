namespace Conway;

public class InputLiveCellAction : IAction
{
    private IConsoleFacade Console { get; }

    public InputLiveCellAction(IConsoleFacade console)
    {
        Console = console;
    }

    public ActionResult Execute(GameState currentGameState)
    {
        GameState resultingGameState;
        while (!ProcessLiveCellActionInput(currentGameState, out resultingGameState))
        {
            currentGameState = resultingGameState;
        }

        return new ActionResult(resultingGameState, new DisplayMenuAction(Console));
    }

    private bool ProcessLiveCellActionInput(GameState currentGameState, out GameState resultingGameState)
    {
        Console.WriteLine("Please enter live cell coordinate in x y format, ~ to clear all the previously entered cells or # to go back to main menu:");
        
        var input = Console.ReadLine();
        var inputTokens = input.Split();
        resultingGameState = currentGameState;

        if (TryParseTerminatingChar(inputTokens))
        {
            return true;
        }

        if (TryParseEmptyLiveCellList(inputTokens))
        {
            resultingGameState = currentGameState with { LiveCells = new List<Cell>() };
            return false;
        }

        if (TryParseLiveCell(inputTokens, out var x, out var y))
        {
            resultingGameState.LiveCells.Add(new Cell(x, y));
            return false;
        }

        Console.WriteLine("Invalid input for input live cell action!");
        return false;
    }

    private bool TryParseTerminatingChar(IReadOnlyList<string> inputTokens)
    {
        if (inputTokens.Count != 1)
        {
            return false;
        }

        return inputTokens[0] == "#";
    }
    
    private bool TryParseEmptyLiveCellList(IReadOnlyList<string> inputTokens)
    {
        if (inputTokens.Count != 1 || inputTokens[0] != "~")
        {
            return false;
        }
        
        return true;
    }

    private bool TryParseLiveCell(string[] inputTokens, out int x, out int y)
    {
        x = 0;
        y = 0;
        if (inputTokens.Length != 2)
        {
            return false;
        }
        
        var isValidX = int.TryParse(inputTokens[0], out var validX);
        var isValidY = int.TryParse(inputTokens[1], out var validY);
        if (isValidX && isValidY)
        {
            x = validX;
            y = validY;
            return true;
        }
        
        Console.WriteLine("Live cell coordinates must be numerical!");
        return false;
    }
}