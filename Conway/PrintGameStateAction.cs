namespace Conway;

public class PrintGameStateAction : IAction
{
    public string Message { get; }
    private IConsoleFacade Console { get; }
    private IDisplayMenuActionFactory DisplayMenuActionFactory { get; }

    public PrintGameStateAction(IConsoleFacade console, IDisplayMenuActionFactory displayMenuActionFactory)
    {
        Message = "Print current configuration";
        Console = console;
        DisplayMenuActionFactory = displayMenuActionFactory;
    }
    
    public ActionResult Execute(GameState currentGameState)
    {
        Console.WriteLine("Current GameState:");
        Console.WriteLine($"Width: {currentGameState.Width}, Height: {currentGameState.Height}");
        Console.WriteLine($"Number of Generations: {currentGameState.NumGen}");
        Console.WriteLine("Live Cells: ");
        foreach (var liveCell in currentGameState.LiveCells)
        {
            Console.WriteLine($"({liveCell.X}, {liveCell.Y})");
        }
        Console.WriteLine("");

        return new ActionResult(currentGameState, DisplayMenuActionFactory.Get());
    }
}