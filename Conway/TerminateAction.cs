namespace Conway;

public class TerminateAction : IAction
{
    public string Message { get; } = "Exit";
    public ActionResult Execute(GameState currentGameState)
    {
        throw new NotImplementedException();
    }
}