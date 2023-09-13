using NSubstitute;

namespace Conway.Tests;

public class InputNumberOfGenerationActionTests
{
    private readonly IConsoleFacade _console;

    public InputNumberOfGenerationActionTests()
    {
        _console = Substitute.For<IConsoleFacade>();
    }
    
    [Fact]
    public void Should_Have_Correct_Message()
    {
        var action = new InputNumberOfGenerationAction(_console);

        Assert.Equal("Specify number of generation", action.Message);
    }

    [Fact]
    public void Should_Prompt_For_Number_Of_Generation()
    {
        var gameState = new GameState();
        var action = new InputNumberOfGenerationAction(_console);
        _console.ReadLine().Returns("5");

        action.Execute(gameState);
        
        _console.Received().WriteLine("Please enter the number of generation (3-20):");
    }
    
    [Fact]
    public void Should_Return_BackToMenuAction_After_Execution()
    {
        var gameState = new GameState();
        var action = new InputNumberOfGenerationAction(_console);
        _console.ReadLine().Returns("5");

        var actionResult = action.Execute(gameState);

        Assert.IsType<BackToMenuAction>(actionResult.NextAction);
    }
    
    [Theory]
    [InlineData(10)]
    [InlineData(7)]
    public void Should_Record_User_Inputted_Number_Of_Generation(int numGen)
    {
        // Arrange
        var gameState = new GameState();
        var action = new InputNumberOfGenerationAction(_console);
        _console.ReadLine().Returns($"{numGen}");

        // Act
        var actionResult = action.Execute(gameState);
        var newGameState = actionResult.GameState;

        // Assert
        Assert.Equal(numGen, newGameState.NumGen);
    }

    [Theory]
    [InlineData("4 5")]
    [InlineData("asdf safasdf asfdf")]
    public void Should_Display_Error_And_Prompt_Again_After_User_Enters_Invalid_Number_Of_Word_Tokens(string input)
    {
        var gameState = new GameState();
        var action = new InputNumberOfGenerationAction(_console);
        _console.ReadLine().Returns(input, input, "5");
        
        action.Execute(gameState);
        
        _console.Received(2).WriteLine("Wrong format for number of generation!");
        _console.Received(3).WriteLine("Please enter the number of generation (3-20):");
        _console.Received(3).ReadLine();
    }
    
    [Theory]
    [InlineData("asdf")]
    [InlineData("mcjs")]
    public void Should_Display_Error_And_Prompt_Again_After_User_Enters_Non_Numerical_Input(string input)
    {
        var gameState = new GameState();
        var action = new InputNumberOfGenerationAction(_console);
        _console.ReadLine().Returns(input, input, "5");
        
        action.Execute(gameState);
        
        _console.Received(2).WriteLine("Number of generation must be numerical!");
        _console.Received(3).WriteLine("Please enter the number of generation (3-20):");
        _console.Received(3).ReadLine();
    }
    
    [Theory]
    [InlineData("21")]
    [InlineData("2")]
    public void Should_Display_Error_And_Prompt_Again_After_User_Enters_Out_Of_Range_NumGen(string input)
    {
        var gameState = new GameState();
        var action = new InputNumberOfGenerationAction(_console);
        _console.ReadLine().Returns(input, input, "5");
        
        action.Execute(gameState);
        
        _console.Received(2).WriteLine("Number of generation must be between 3 and 20 inclusive!");
        _console.Received(3).WriteLine("Please enter the number of generation (3-20):");
        _console.Received(3).ReadLine();
    }
    
    [Fact]
    public void Should_Retain_CurrentGameState_Other_Than_NumGen()
    {
        var currentGameState = new GameState(10, 2, 5, new List<Cell> { new(0, 0), new(1, 1) });
        var action = new InputNumberOfGenerationAction(_console);
        _console.ReadLine().Returns("3");

        var actionResult = action.Execute(currentGameState);
        
        Assert.Equal(currentGameState.Width, actionResult.GameState.Width);
        Assert.Equal(currentGameState.Height, actionResult.GameState.Height);
        Assert.Equal(currentGameState.LiveCells, actionResult.GameState.LiveCells);
    }
}