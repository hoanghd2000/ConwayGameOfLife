namespace Conway;

public class InputLiveCellAction : IAction
{
    private IConsoleFacade Console { get; }
    private GameState CurrentGameState { get; set; }

    public InputLiveCellAction(IConsoleFacade console, GameState currentGameState)
    {
        Console = console;
        CurrentGameState = currentGameState;
    }

    public ActionResult Execute()
    {
        while (!ProcessLiveCellActionInput())
        {
        }

        return new ActionResult(CurrentGameState, new DisplayMenuAction(Console, CurrentGameState));
    }

    private bool ProcessLiveCellActionInput()
    {
        Console.WriteLine("Please enter live cell coordinate in x y format, ~ to clear all the previously entered cells or # to go back to main menu:");
        
        var input = Console.ReadLine();
        var inputTokens = input.Split();

        if (TryParseTerminatingChar(inputTokens))
        {
            return true;
        }

        if (!TryEmptyLiveCellList(inputTokens) && !TryAddLiveCell(inputTokens))
        {
            Console.WriteLine("Invalid input for input live cell action!");
        }
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
    
    private bool TryEmptyLiveCellList(IReadOnlyList<string> inputTokens)
    {
        if (inputTokens.Count != 1 || inputTokens[0] != "~")
        {
            return false;
        }
        
        CurrentGameState = CurrentGameState with { LiveCells = new List<Cell>() };
        return true;
    }

    private bool TryAddLiveCell(string[] inputTokens)
    {
        if (inputTokens.Length != 2)
        {
            return false;
        }
        
        var isValidX = int.TryParse(inputTokens[0], out var x);
        var isValidY = int.TryParse(inputTokens[1], out var y);
        if (isValidX && isValidY)
        {
            CurrentGameState.LiveCells.Add(new Cell(x, y));
            return true;
        }
        
        Console.WriteLine("Live cell coordinates must be numerical!");
        return false;
    }
}