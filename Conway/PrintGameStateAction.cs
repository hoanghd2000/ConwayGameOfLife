namespace Conway;

public class PrintGameStateAction : IAction
{
    public string Message { get; }
    private IConsoleFacade Console { get; }

    public PrintGameStateAction(IConsoleFacade console)
    {
        Message = "Print current configuration";
        Console = console;
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

        return new ActionResult(currentGameState, new BackToMenuAction());
    }
}