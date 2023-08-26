namespace Conway;

public record ActionResult(GameState GameState, IAction NextAction);