namespace Conway;

public class GameController
{
    private IConsoleFacade Console { get; }
    public GameState CurrentGameState { get; set; }
    private IAction CurrentAction { get; set; }

    private Dictionary<int, IAction> ActionMap { get; }

    public GameController()
    {
        Console = new ConsoleFacade();
        ActionMap = new Dictionary<int, IAction>
        {
            {1, new InputGridSizeAction(Console)},
        };
        CurrentAction = new DisplayMenuAction(Console);
    }

    public void Play()
    {
        try
        {
            while (CurrentAction is not TerminateAction)
            {
                var actionResult = CurrentAction.Execute();
                CurrentGameState = actionResult.GameState;
                CurrentAction = actionResult.NextAction;

                if (CurrentAction is TerminateAction)
                {
                    var input = Console.ReadLine();
                    var inputNum = int.Parse(input);
                    if (ActionMap.TryGetValue(inputNum, out var action))
                    {
                        CurrentAction = action;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}