namespace Conway;

public class InputGridSizeAction : IAction
{
    public IConsoleFacade Console { get; set; }

    public InputGridSizeAction(IConsoleFacade console)
    {
        Console = console;
    }

    public ActionResult Execute()
    {
        Console.WriteLine("Please enter grid size in w h format (example: 10 15):");

        var line = Console.ReadLine();
        var dimensions = line.Split();
        
        if (dimensions.Length != 2)
        {
            Console.WriteLine("Wrong format for grid size!!! Please enter 2 integers in w h format (example: 10 15).");
            return new ActionResult(new GameState(0, 0, 0), new DisplayMenuAction(Console));
        }

        try
        {
            var width = int.Parse(dimensions[0]);
            var height = int.Parse(dimensions[1]);
            return new ActionResult(new GameState(width, height, 0), new DisplayMenuAction(Console));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Width and Height must be numerical!!! Please enter 2 integers in w h format (example: 10 15).");
            return new ActionResult(new GameState(0, 0, 0), new DisplayMenuAction(Console));
        }
    }
}

public record GameState(int Width, int Height, int NumGen);

public record ActionResult(GameState GameState, IAction NextAction);