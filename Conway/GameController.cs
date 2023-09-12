namespace Conway;

public class GameController
{
    private IConsoleFacade Console { get; }
    public GameState CurrentGameState { get; set; }
    public IAction CurrentAction { get; set; }

    public GameController(IFreshGameStateFactory freshGameStateFactory)
    {
        Console = new ConsoleFacade();
        CurrentGameState = freshGameStateFactory.CreateFreshGameState();
        CurrentAction = new DisplayMenuAction(Console);
    }

    public void Play()
    {
        while (CurrentAction is not TerminateAction)
        {
            var actionResult = CurrentAction.Execute(CurrentGameState);
            CurrentGameState = actionResult.GameState;
            CurrentAction = actionResult.NextAction;
        }
    }
}