namespace Conway;

public class InputNumberOfGenerationAction : IAction
{
    private IConsoleFacade Console { get; set; }
    private GameState CurrentGameState { get; set; }
    public InputNumberOfGenerationAction(IConsoleFacade console, GameState currentGameState)
    {
        Console = console;
        CurrentGameState = currentGameState;
    }

    public ActionResult Execute()
    {
        var numTries = 3;
        while (numTries > 0)
        {
            Console.WriteLine("Please enter the number of generation (3-20):");
            
            var input = Console.ReadLine();
            var inputTokens = input.Split();
            
            if (inputTokens.Length != 1)
            {
                Console.WriteLine("Wrong format for number of generation!");
                numTries -= 1;
                continue;
            }

            try
            {
                var numGen = int.Parse(inputTokens[0]);
                if (numGen is < 3 or > 20)
                {
                    Console.WriteLine("Number of generation must be between 3 and 20 inclusive!");
                    numTries -= 1;
                    continue;
                }
                return new ActionResult(CurrentGameState with {NumGen = numGen}, new DisplayMenuAction(Console));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Number of generation must be numerical!");
                Console.WriteLine(ex.Message);
                numTries -= 1;
            }
        }
        
        return new ActionResult(CurrentGameState, new DisplayMenuAction(Console));
    }
}