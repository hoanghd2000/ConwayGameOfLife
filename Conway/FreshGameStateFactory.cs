namespace Conway;

public interface IFreshGameStateFactory
{
    public GameState CreateFreshGameState();
}

public class FreshGameStateFactory : IFreshGameStateFactory
{
    public GameState CreateFreshGameState()
    {
        return new GameState(LiveCells: new List<Cell>());
    }
}