namespace Conway;

public class GameController
{
    private IConsoleFacade Console { get; }
    private GameState CurrentGameState { get; set; }
    private IAction CurrentAction { get; set; }

    public GameController()
    {
        Console = new ConsoleFacade();
        CurrentGameState = new GameState(LiveCells: new List<Cell>());
        CurrentAction = new DisplayMenuAction(Console, CurrentGameState);
    }

    public void Play()
    {
        while (CurrentAction is not TerminateAction)
        {
            var actionResult = CurrentAction.Execute();
            CurrentGameState = actionResult.GameState;
            CurrentAction = actionResult.NextAction;
        }
    }
}