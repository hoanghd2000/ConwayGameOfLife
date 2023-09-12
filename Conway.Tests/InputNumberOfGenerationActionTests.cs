using NSubstitute;

namespace Conway.Tests;

public class InputNumberOfGenerationActionTests
{
    [Fact]
    public void Should_Prompt_For_Number_Of_Generation()
    {
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState();
        var action = new InputNumberOfGenerationAction(console);
        console.ReadLine().Returns("5");

        action.Execute(gameState);
        
        console.Received().WriteLine("Please enter the number of generation (3-20):");
    }
    
    [Fact]
    public void Should_Return_DisplayMenuAction_After_Execution()
    {
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState();
        var action = new InputNumberOfGenerationAction(console);
        console.ReadLine().Returns("5");

        var actionResult = action.Execute(gameState);

        Assert.IsType<DisplayMenuAction>(actionResult.NextAction);
    }
    
    [Theory]
    [InlineData(10)]
    [InlineData(7)]
    public void Should_Record_User_Inputted_Number_Of_Generation(int numGen)
    {
        // Arrange
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState();
        var action = new InputNumberOfGenerationAction(console);
        console.ReadLine().Returns($"{numGen}");

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
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState();
        var action = new InputNumberOfGenerationAction(console);
        console.ReadLine().Returns(input, input, "5");
        
        action.Execute(gameState);
        
        console.Received(2).WriteLine("Wrong format for number of generation!");
        console.Received(3).WriteLine("Please enter the number of generation (3-20):");
        console.Received(3).ReadLine();
    }
    
    [Theory]
    [InlineData("asdf")]
    [InlineData("mcjs")]
    public void Should_Display_Error_And_Prompt_Again_After_User_Enters_Non_Numerical_Input(string input)
    {
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState();
        var action = new InputNumberOfGenerationAction(console);
        console.ReadLine().Returns(input, input, "5");
        
        action.Execute(gameState);
        
        console.Received(2).WriteLine("Number of generation must be numerical!");
        console.Received(3).WriteLine("Please enter the number of generation (3-20):");
        console.Received(3).ReadLine();
    }
    
    [Theory]
    [InlineData("21")]
    [InlineData("2")]
    public void Should_Display_Error_And_Prompt_Again_After_User_Enters_Out_Of_Range_NumGen(string input)
    {
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState();
        var action = new InputNumberOfGenerationAction(console);
        console.ReadLine().Returns(input, input, "5");
        
        action.Execute(gameState);
        
        console.Received(2).WriteLine("Number of generation must be between 3 and 20 inclusive!");
        console.Received(3).WriteLine("Please enter the number of generation (3-20):");
        console.Received(3).ReadLine();
    }
    
    [Fact]
    public void Should_Retain_CurrentGameState_Other_Than_NumGen()
    {
        var console = Substitute.For<IConsoleFacade>();
        var currentGameState = new GameState(10, 2, 5, new List<Cell> { new(0, 0), new(1, 1) });
        var action = new InputNumberOfGenerationAction(console);
        console.ReadLine().Returns("3");

        var actionResult = action.Execute(currentGameState);
        
        Assert.Equal(currentGameState.Width, actionResult.GameState.Width);
        Assert.Equal(currentGameState.Height, actionResult.GameState.Height);
        Assert.Equal(currentGameState.LiveCells, actionResult.GameState.LiveCells);
    }
}