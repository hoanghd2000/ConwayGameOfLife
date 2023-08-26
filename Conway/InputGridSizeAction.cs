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
        var numTries = 3;
        while (numTries > 0)
        {
            Console.WriteLine("Please enter grid size in w h format (example: 10 15):");

            var line = Console.ReadLine();
            var dimensions = line.Split();
            
            if (dimensions.Length != 2)
            {
                Console.WriteLine("Wrong format for grid size!!!");
                numTries -= 1;
                continue;
            }
            
            try
            {
                var width = int.Parse(dimensions[0]);
                var height = int.Parse(dimensions[1]);
                if (width < 1 || width > 25 || height < 1 || height > 25)
                {
                    Console.WriteLine("Width and Height must be between 1 and 25 inclusive!!!");
                    numTries -= 1;
                    continue;
                }
                return new ActionResult(new GameState(width, height, 0), new DisplayMenuAction(Console));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Width and Height must be numerical!!!");
                numTries -= 1;
            }
        }
        
        return new ActionResult(new GameState(0, 0, 0), new DisplayMenuAction(Console));
    }
}

public record GameState(int Width, int Height, int NumGen);

public record ActionResult(GameState GameState, IAction NextAction);