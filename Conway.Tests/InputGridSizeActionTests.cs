using NSubstitute;

namespace Conway.Tests;

public class InputGridSizeActionTests
{
    private readonly IConsoleFacade _console;
    private readonly IDisplayMenuActionFactory _displayMenuActionFactory;

    public InputGridSizeActionTests()
    {
        _console = Substitute.For<IConsoleFacade>();
        _displayMenuActionFactory = Substitute.For<IDisplayMenuActionFactory>();
        _displayMenuActionFactory.Get().Returns(new DisplayMenuAction(_console, new List<IAction>()));
    }
    
    [Fact]
    public void Should_Prompt_For_Grid_Size()
    {
        // Arrange
        var gameState = new GameState();
        var action = new InputGridSizeAction(_console, _displayMenuActionFactory);
        _console.ReadLine().Returns("1 1");

        // Act
        action.Execute(gameState);

        // Assert
        _console.Received().WriteLine("Please enter grid size in w h format (example: 10 15):");
    }

    [Fact]
    public void Should_Return_DisplayMenuAction_After_Execution()
    {
        var gameState = new GameState();
        var action = new InputGridSizeAction(_console, _displayMenuActionFactory);
        _console.ReadLine().Returns("1 1");

        var actionResult = action.Execute(gameState);

        Assert.IsType<DisplayMenuAction>(actionResult.NextAction);
    }

    [Theory]
    [InlineData(10, 15)]
    [InlineData(7, 19)]
    public void Should_Record_User_Inputted_Grid_Size(int width, int height)
    {
        // Arrange
        var gameState = new GameState();
        var action = new InputGridSizeAction(_console, _displayMenuActionFactory);
        _console.ReadLine().Returns($"{width} {height}");

        // Act
        var actionResult = action.Execute(gameState);
        var newGameState = actionResult.GameState;

        // Assert
        Assert.Equal(width, newGameState.Width);
        Assert.Equal(height, newGameState.Height);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1 7 5")]
    [InlineData("afsdf fsadf asdf 66")]
    public void Should_Display_Error_And_Prompt_Again_When_User_Enters_Invalid_Number_Of_Word_Tokens(string input)
    {
        var gameState = new GameState();
        var action = new InputGridSizeAction(_console, _displayMenuActionFactory);
        _console.ReadLine().Returns(input, input, "1 1");

        action.Execute(gameState);

        _console.Received(2).WriteLine("Wrong format for grid size!!!");
        _console.Received(3).WriteLine("Please enter grid size in w h format (example: 10 15):");
        _console.Received(3).ReadLine();
    }

    [Theory]
    [InlineData("ab cd")]
    [InlineData("8 cd")]
    [InlineData("cd 7")]
    public void Should_Display_Error_And_Prompt_Again_When_User_Enters_Non_Numeric_Input(string input)
    {
        var gameState = new GameState();
        var action = new InputGridSizeAction(_console, _displayMenuActionFactory);
        _console.ReadLine().Returns(input, input, "1 1");

        action.Execute(gameState);

        _console.Received(2).WriteLine("Width and Height must be numerical!!!");
        _console.Received(3).WriteLine("Please enter grid size in w h format (example: 10 15):");
        _console.Received(3).ReadLine();
    }

    [Theory]
    [InlineData("26 1")]
    [InlineData("3 30")]
    [InlineData("-1 0")]
    public void Should_Display_Error_And_Prompt_Again_When_User_Enters_Out_Of_Range_Input(string input)
    {
        var gameState = new GameState();
        var action = new InputGridSizeAction(_console, _displayMenuActionFactory);
        _console.ReadLine().Returns(input, input, "1 1");

        action.Execute(gameState);

        _console.Received(2).WriteLine("Width and Height must be between 1 and 25 inclusive!!!");
        _console.Received(3).WriteLine("Please enter grid size in w h format (example: 10 15):");
        _console.Received(3).ReadLine();
    }

    [Fact]
    public void Should_Retain_CurrentGameState_Other_Than_Width_And_Height()
    {
        var currentGameState = new GameState(5, 5, 5, new List<Cell> { new(0, 0), new(1, 1) });
        var action = new InputGridSizeAction(_console, _displayMenuActionFactory);
        _console.ReadLine().Returns("3 12");

        var actionResult = action.Execute(currentGameState);
        
        Assert.Equal(currentGameState.NumGen, actionResult.GameState.NumGen);
        Assert.Equal(currentGameState.LiveCells, actionResult.GameState.LiveCells);
    }
}