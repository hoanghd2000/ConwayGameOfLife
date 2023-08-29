namespace Conway;

public class InputGridSizeAction : IAction
{
    private IConsoleFacade Console { get; }
    private GameState CurrentGameState { get; }

    public InputGridSizeAction(IConsoleFacade console, GameState currentGameState)
    {
        Console = console;
        CurrentGameState = currentGameState;
    }

    public ActionResult Execute()
    {
        int width, height;
        while (!TryGetGridSize(out width, out height))
        {
        }
        
        return new ActionResult(CurrentGameState with {Width = width, Height = height}, new DisplayMenuAction(Console, CurrentGameState with {Width = width, Height = height}));
    }

    private bool TryGetGridSize(out int width, out int height)
    {
        Console.WriteLine("Please enter grid size in w h format (example: 10 15):");
        width = CurrentGameState.Width;
        height = CurrentGameState.Height;
        
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