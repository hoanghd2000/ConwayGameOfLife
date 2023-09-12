using NSubstitute;

namespace Conway.Tests;

public class InputLiveCellActionTests
{
    private readonly IConsoleFacade _console;
    private readonly IDisplayMenuActionFactory _displayMenuActionFactory;

    public InputLiveCellActionTests()
    {
        _console = Substitute.For<IConsoleFacade>();
        _displayMenuActionFactory = Substitute.For<IDisplayMenuActionFactory>();
        _displayMenuActionFactory.Get().Returns(new DisplayMenuAction(_console, new List<IAction>()));
    }

    [Fact]
    public void Should_Prompt_For_Live_Cell()
    {
        var currentGameState = new GameState(LiveCells: new List<Cell>());
        var action = new InputLiveCellAction(_console, _displayMenuActionFactory);
        _console.ReadLine().Returns("1 2", "#");

        action.Execute(currentGameState);
        
        _console.Received().WriteLine("Please enter live cell coordinate in x y format, ~ to clear all the previously entered cells or # to go back to main menu:");
    }
    
    [Fact]
    public void Should_Return_DisplayMenuAction_After_Execution()
    {
        var currentGameState = new GameState(LiveCells: new List<Cell>());
        var action = new InputLiveCellAction(_console, _displayMenuActionFactory);
        _console.ReadLine().Returns("1 2", "#");

        var actionResult = action.Execute(currentGameState);

        Assert.IsType<DisplayMenuAction>(actionResult.NextAction);
    }
    
    [Theory]
    [InlineData("asdf safasdf asfdf")]
    [InlineData("5 6 7")]
    public void Should_Display_Error_And_Prompt_Again_After_User_Enters_Invalid_Number_Of_Word_Tokens(string input)
    {
        var currentGameState = new GameState();
        var action = new InputLiveCellAction(_console, _displayMenuActionFactory);
        _console.ReadLine().Returns(input, input, "#");
        
        action.Execute(currentGameState);
        
        _console.Received(2).WriteLine("Invalid input for input live cell action!");
        _console.Received(3).WriteLine("Please enter live cell coordinate in x y format, ~ to clear all the previously entered cells or # to go back to main menu:");
        _console.Received(3).ReadLine();
    }
    
    [Theory]
    [InlineData("fsadfe")]
    [InlineData("567")]
    public void Should_Display_Error_And_Prompt_Again_After_User_Enters_Wrong_Termination_Characters(string input)
    {
        var currentGameState = new GameState();
        var action = new InputLiveCellAction(_console, _displayMenuActionFactory);
        _console.ReadLine().Returns(input, input, "#");
        
        
        action.Execute(currentGameState);
        
        _console.Received(2).WriteLine("Invalid input for input live cell action!");
        _console.Received(3).WriteLine("Please enter live cell coordinate in x y format, ~ to clear all the previously entered cells or # to go back to main menu:");
        _console.Received(3).ReadLine();
    }
    
    [Theory]
    [InlineData("fsadfe dsafds")]
    [InlineData("5 ttt")]
    [InlineData("ttt 5")]
    public void Should_Display_Error_And_Prompt_Again_After_User_Enters_Non_Numerical_Input_For_Live_Cell_Coordinates(string input)
    {
        var currentGameState = new GameState();
        var action = new InputLiveCellAction(_console, _displayMenuActionFactory);
        _console.ReadLine().Returns(input, input, "#");
        
        action.Execute(currentGameState);
        
        _console.Received(2).WriteLine("Live cell coordinates must be numerical!");
        _console.Received(3).WriteLine("Please enter live cell coordinate in x y format, ~ to clear all the previously entered cells or # to go back to main menu:");
        _console.Received(3).ReadLine();
    }
    
    [Fact]
    public void Should_Record_All_User_Inputted_Live_Cell_Coordinates_Until_Terminating_Character()
    {
        var currentGameState = new GameState(LiveCells: new List<Cell>());
        var action = new InputLiveCellAction(_console, _displayMenuActionFactory);
        _console.ReadLine().Returns("1 2", "2 2", "3 1", "#");
        
        var actionResult = action.Execute(currentGameState);
        var newGameState = actionResult.GameState;
        
        Assert.Equal(3, newGameState.LiveCells.Count);
        var expectedLiveCells = new List<Cell> { new(1, 2), new (2, 2), new(3, 1) };
        Assert.Equal(expectedLiveCells, newGameState.LiveCells);
    }

    [Fact]
    public void Should_Clear_All_Previously_Entered_Cells_If_Tilda_Is_Entered()
    {
        var currentGameState = new GameState(LiveCells: new List<Cell>());
        var action = new InputLiveCellAction(_console, _displayMenuActionFactory);
        _console.ReadLine().Returns("1 2", "2 2", "3 1", "~", "#");
        
        var actionResult = action.Execute(currentGameState);
        var newGameState = actionResult.GameState;
        
        Assert.Empty(newGameState.LiveCells);
    }
    
    [Fact]
    public void Should_Clear_All_Previously_Entered_Cells_If_Tilda_Is_Entered_And_Save_Cells_Entered_After_That()
    {
        var currentGameState = new GameState(LiveCells: new List<Cell>());
        var action = new InputLiveCellAction(_console, _displayMenuActionFactory);
        _console.ReadLine().Returns("1 2", "2 2", "3 1", "~", "5 5", "#");
        
        var actionResult = action.Execute(currentGameState);
        var newGameState = actionResult.GameState;
        
        var expectedLiveCells = new List<Cell> { new(5, 5) };
        Assert.Equal(expectedLiveCells, newGameState.LiveCells);
    }
    
    [Fact]
    public void Should_Retain_CurrentGameState_Other_Than_LiveCells_When_Input_Successful()
    {
        var currentGameState = new GameState(10, 2, 5, new List<Cell>());
        var action = new InputLiveCellAction(_console, _displayMenuActionFactory);
        _console.ReadLine().Returns("1 2", "2 2", "3 1", "#");

        var actionResult = action.Execute(currentGameState);
        
        Assert.Equal(currentGameState.Width, actionResult.GameState.Width);
        Assert.Equal(currentGameState.Height, actionResult.GameState.Height);
        Assert.Equal(currentGameState.NumGen, actionResult.GameState.NumGen);
    }
}