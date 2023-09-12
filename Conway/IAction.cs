namespace Conway;

public interface IAction
{
    public ActionResult Execute(GameState currentGameState);
}