using NSubstitute;

namespace Conway.Tests;

public class DisplayMenuActionTests
{
    [Fact]
    public void Should_Display_Menu()
    {
        var console = Substitute.For<IConsoleFacade>();
        var currentGameState = new GameState();
        var action = new DisplayMenuAction(console);
        console.ReadLine().Returns("1");

        action.Execute(currentGameState);

        console.Received(1).WriteLine(@"Welcome to Conway's Game of Life
[1] Specify grid size
[2] Specify number of generation
[3] Specify initial live cells
[4] Print current configuration
[5] Run
[6] Exit
Please enter your selection");
    }

    [Theory]
    [InlineData("")]
    [InlineData("asjdlkf")]
    [InlineData("1 1")]
    [InlineData("1 2 3")]
    [InlineData("10")]
    public void Should_Display_Error_And_Prompt_Again_When_User_Enters_Invalid_Input(string input)
    {
        var console = Substitute.For<IConsoleFacade>();
        var currentGameState = new GameState();
        var action = new DisplayMenuAction(console);
        console.ReadLine().Returns(input, "1");
        
        action.Execute(currentGameState);
        
        console.Received(1).WriteLine("Invalid input! Please enter your option between 1-5.");
        console.Received(2).WriteLine(@"Welcome to Conway's Game of Life
[1] Specify grid size
[2] Specify number of generation
[3] Specify initial live cells
[4] Print current configuration
[5] Run
[6] Exit
Please enter your selection");
    }
    
    [Theory]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("3")]
    [InlineData("4")]
    public void Should_Return_CurrentGameState(string input)
    {
        var currentGameState = new GameState();
        var console = Substitute.For<IConsoleFacade>();
        var action = new DisplayMenuAction(console);
        console.ReadLine().Returns(input);

        var actionResult = action.Execute(currentGameState);

        Assert.Equal(currentGameState, actionResult.GameState);
    }

    [Fact]
    public void Should_Record_And_Return_User_Selected_NextAction1()
    {
        var console = Substitute.For<IConsoleFacade>();
        var currentGameState = new GameState();
        var action = new DisplayMenuAction(console);
        console.ReadLine().Returns("1");

        var actionResult = action.Execute(currentGameState);
        
        Assert.IsType<InputGridSizeAction>(actionResult.NextAction);
    }
    
    [Fact]
    public void Should_Record_And_Return_User_Selected_NextAction2()
    {
        var console = Substitute.For<IConsoleFacade>();
        var currentGameState = new GameState();
        var action = new DisplayMenuAction(console);
        console.ReadLine().Returns("2");

        var actionResult = action.Execute(currentGameState);
        
        Assert.IsType<InputNumberOfGenerationAction>(actionResult.NextAction);
    }
    
    [Fact]
    public void Should_Record_And_Return_User_Selected_NextAction3()
    {
        var console = Substitute.For<IConsoleFacade>();
        var currentGameState = new GameState();
        var action = new DisplayMenuAction(console);
        console.ReadLine().Returns("3");

        var actionResult = action.Execute(currentGameState);
        
        Assert.IsType<InputLiveCellAction>(actionResult.NextAction);
    }
    
    [Fact]
    public void Should_Record_And_Return_User_Selected_NextAction4()
    {
        var console = Substitute.For<IConsoleFacade>();
        var currentGameState = new GameState();
        var action = new DisplayMenuAction(console);
        console.ReadLine().Returns("4");

        var actionResult = action.Execute(currentGameState);
        
        Assert.IsType<PrintGameStateAction>(actionResult.NextAction);
    }
    
    [Fact]
    public void Should_Record_And_Return_User_Selected_NextAction5()
    {
        var console = Substitute.For<IConsoleFacade>();
        var currentGameState = new GameState();
        var action = new DisplayMenuAction(console);
        console.ReadLine().Returns("5");

        var actionResult = action.Execute(currentGameState);
        
        Assert.IsType<RunAction>(actionResult.NextAction);
    }
    
    [Fact]
    public void Should_Record_And_Return_User_Selected_NextAction6()
    {
        var console = Substitute.For<IConsoleFacade>();
        var currentGameState = new GameState();
        var action = new DisplayMenuAction(console);
        console.ReadLine().Returns("6");

        var actionResult = action.Execute(currentGameState);
        
        Assert.IsType<TerminateAction>(actionResult.NextAction);
    }
}