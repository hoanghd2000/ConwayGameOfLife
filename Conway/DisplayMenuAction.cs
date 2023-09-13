namespace Conway;

public class DisplayMenuAction: IAction
{
    public string Message { get; }
    public IConsoleFacade Console { get; }
    public List<IAction> Actions { get; }

    public DisplayMenuAction(IConsoleFacade console, IEnumerable<IAction> actions)
    {
        Message = "Display Menu";
        Console = console;
        Actions = actions.ToList();
    }

    public ActionResult Execute(GameState currentGameState)
    {
        var nextAction = GetNextAction();
        return new ActionResult(currentGameState, nextAction);
    }
    
    private IAction GetNextAction()
    {
        IAction nextAction = new TerminateAction();
        var validInput = false;
        while (!validInput)
        {
            validInput = TryGetNextAction(out nextAction);
        }

        return nextAction;
    }

    private bool TryGetNextAction(out IAction nextAction)
    {
        Console.WriteLine("Welcome to Conway's Game of Life");
        for (var i = 0; i < Actions.Count; i++)
        {
            Console.WriteLine($"[{i + 1}] {Actions[i].Message}");
        }
        Console.WriteLine("Please enter your selection");
        
        nextAction = new TerminateAction();
        var input = Console.ReadLine();
        if (!int.TryParse(input.Trim(), out var inputNum) || inputNum < 1 || inputNum > 6)
        {
            Console.WriteLine("Invalid input! Please enter your option between 1-5.");
            return false;
        }

        nextAction = Actions[inputNum - 1];
        return true;
    }
}