namespace Conway;

public interface IAction
{
    public string Message { get; }
    public ActionResult Execute(GameState currentGameState);
}