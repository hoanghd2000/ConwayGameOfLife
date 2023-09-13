namespace Conway;

public class BackToMenuAction : IAction
{
    public string Message { get; } = "Back to Menu";
    public ActionResult Execute(GameState currentGameState)
    {
        throw new NotImplementedException();
    }
}