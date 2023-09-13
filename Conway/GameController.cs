namespace Conway;

public class GameController : IGameController
{
    private readonly IDisplayMenuActionFactory _displayMenuActionFactory;
    public GameState CurrentGameState { get; private set; }
    public IAction CurrentAction { get; set; }

    public GameController(IFreshGameStateFactory freshGameStateFactory, 
        IDisplayMenuActionFactory displayMenuActionFactory)
    {
        CurrentGameState = freshGameStateFactory.Create();
        _displayMenuActionFactory = displayMenuActionFactory;
        CurrentAction = _displayMenuActionFactory.Get();
    }

    public void Play()
    {
        while (CurrentAction is not TerminateAction)
        {
            var actionResult = CurrentAction.Execute(CurrentGameState);
            CurrentGameState = actionResult.GameState;
            CurrentAction = actionResult.NextAction;
            if (CurrentAction is BackToMenuAction)
            {
                CurrentAction = _displayMenuActionFactory.Get();
            }
        }
    }
}

public interface IGameController
{
    void Play();
}