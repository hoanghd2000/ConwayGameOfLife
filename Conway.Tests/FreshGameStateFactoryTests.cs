namespace Conway.Tests;

public class FreshGameStateFactoryTests
{
    [Fact]
    public void Should_Create_Fresh_GameState()
    {
        var factory = new FreshGameStateFactory();

        var freshGameState = factory.Create();
        
        Assert.Equal(0, freshGameState.Width);
        Assert.Equal(0, freshGameState.Height);
        Assert.Equal(0, freshGameState.NumGen);
        Assert.Empty(freshGameState.LiveCells);
    }
}