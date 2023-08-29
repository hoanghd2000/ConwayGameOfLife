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
        Console.WriteLine("Please enter grid size in w h format (example: 10 15):");
        
        int width, height;
        while (!TryParseGridSize(out width, out height))
        {
            Console.WriteLine("Please enter grid size in w h format (example: 10 15):");
        }
        
        return new ActionResult(CurrentGameState with {Width = width, Height = height}, new DisplayMenuAction(Console));
    }

    private bool TryParseGridSize(out int width, out int height)
    {
        var line = Console.ReadLine();
        var dimensions = line.Split();

        if (dimensions.Length != 2)
        {
            Console.WriteLine("Wrong format for grid size!!!");
            AssignOldWidthHeight(out width, out height);
            return false;
        }

        var isValidWidth = int.TryParse(dimensions[0], out width);
        var isValidHeight = int.TryParse(dimensions[1], out height);
        if (!isValidWidth || !isValidHeight)
        {
            Console.WriteLine("Width and Height must be numerical!!!");
            AssignOldWidthHeight(out width, out height);
            return false;
        }

        if (width < 1 || width > 25 || height < 1 || height > 25)
        {
            Console.WriteLine("Width and Height must be between 1 and 25 inclusive!!!");
            AssignOldWidthHeight(out width, out height);
            return false;
        }

        return true;
    }

    private void AssignOldWidthHeight(out int width, out int height)
    {
        width = CurrentGameState.Width;
        height = CurrentGameState.Height;
    }
}