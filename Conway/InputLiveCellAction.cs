namespace Conway;

public class InputLiveCellAction : IAction
{
    public IConsoleFacade Console { get; set; }
    public GameState CurrentGameState { get; set; }
    public InputLiveCellAction(IConsoleFacade console, GameState currentGameState)
    {
        Console = console;
        CurrentGameState = currentGameState;
    }

    public ActionResult Execute()
    {
        var numTries = 3;
        while (numTries > 0)
        {
            Console.WriteLine("Please enter live cell coordinate in x y format, ~ to clear all the previously entered cells or # to go back to main menu:");

            var input = Console.ReadLine();
            var inputTokens = input.Split();
            if (inputTokens.Length is > 2 or < 1)
            {
                Console.WriteLine("Wrong format for live cell!");
                numTries -= 1;
                continue;
            }
            
            if (inputTokens.Length == 1 && inputTokens[0] != "#" && inputTokens[0] != "~")
            {
                Console.WriteLine("Wrong format for live cell!");
                numTries -= 1;
                continue;
            }
            
            if (inputTokens.Length == 1 && inputTokens[0] == "#")
            {
                return new ActionResult(CurrentGameState, new DisplayMenuAction(Console));
            }
            
            if (inputTokens.Length == 1 && inputTokens[0] == "~")
            {
                return new ActionResult(CurrentGameState with {LiveCells = new List<Cell>()}, new DisplayMenuAction(Console));
            }

            if (inputTokens.Length == 2)
            {
                try
                {
                    var x = int.Parse(inputTokens[0]);
                    var y = int.Parse(inputTokens[1]);
                    CurrentGameState.LiveCells.Add(new Cell(x, y));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Live cell coordinates must be numerical!");
                    System.Console.WriteLine(e);
                    numTries -= 1;
                }
            }
        }

        return new ActionResult(CurrentGameState, new DisplayMenuAction(Console));
    }
}