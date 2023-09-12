namespace Conway;

public interface IFreshGameStateFactory
{
    public GameState Create();
}

public class FreshGameStateFactory : IFreshGameStateFactory
{
    public GameState Create()
    {
        return new GameState(LiveCells: new List<Cell>());
    }
}