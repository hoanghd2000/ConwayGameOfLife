namespace Conway;

public class PrintGameStateAction : IAction
{
    private IConsoleFacade Console { get; set; }
    private GameState CurrentGameState { get; set; }

    public PrintGameStateAction(IConsoleFacade console, GameState currentGameState)
    {
        Console = console;
        CurrentGameState = currentGameState;
    }
    
    public ActionResult Execute()
    {
        Console.WriteLine("Current GameState:");
        Console.WriteLine($"Width: {CurrentGameState.Width}, Height: {CurrentGameState.Height}");
        Console.WriteLine($"Number of Generations: {CurrentGameState.NumGen}");
        Console.WriteLine("Live Cells: ");
        foreach (var liveCell in CurrentGameState.LiveCells)
        {
            Console.WriteLine($"({liveCell.X}, {liveCell.Y})");
        }
        Console.WriteLine("");

        return new ActionResult(CurrentGameState, new DisplayMenuAction(Console, CurrentGameState));
    }
}