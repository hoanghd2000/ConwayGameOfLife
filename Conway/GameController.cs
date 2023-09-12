namespace Conway;

public class GameController : IGameController
{
    public GameState CurrentGameState { get; set; }
    public IAction CurrentAction { get; set; }

    public GameController(IFreshGameStateFactory freshGameStateFactory, 
        IDisplayMenuActionFactory displayMenuActionFactory)
    {
        CurrentGameState = freshGameStateFactory.Create();
        CurrentAction = displayMenuActionFactory.Get();
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

public interface IGameController
{
    void Play();
}