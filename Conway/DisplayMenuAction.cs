namespace Conway;

public class DisplayMenuAction: IAction
{
    public IConsoleFacade Console { get; }

    public DisplayMenuAction(IConsoleFacade console)
    {
        Console = console;
    }

    public ActionResult Execute(GameState currentGameState)
    {
        var nextAction = GetNextAction();
        return new ActionResult(currentGameState, nextAction);
    }
    
    private IAction GetNextAction()
    {
        IAction nextAction;
        while (!TryGetNextAction(out nextAction))
        {
        }

        return nextAction;
    }

    private bool TryGetNextAction(out IAction nextAction)
    {
        Console.WriteLine(@"Welcome to Conway's Game of Life
[1] Specify grid size
[2] Specify number of generation
[3] Specify initial live cells
[4] Print current configuration
[5] Run
[6] Exit
Please enter your selection");
        
        nextAction = new TerminateAction();
        var input = Console.ReadLine();
        if (!int.TryParse(input.Trim(), out var inputNum) || inputNum < 1 || inputNum > 6)
        {
            Console.WriteLine("Invalid input! Please enter your option between 1-5.");
            return false;
        }

        nextAction = inputNum switch
        {
            1 => new InputGridSizeAction(Console),
            2 => new InputNumberOfGenerationAction(Console),
            3 => new InputLiveCellAction(Console),
            4 => new PrintGameStateAction(Console),
            5 => new RunAction(Console),
            6 => new TerminateAction(),
            _ => new TerminateAction()
        };

        return true;
    }
}