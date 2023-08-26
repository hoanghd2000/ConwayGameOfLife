using NSubstitute;

namespace Conway.Tests;

public class InputGridSizeActionTests
{
    [Fact]
    public void Should_Prompt_For_Grid_Size()
    {
        // Arrange
        var console = Substitute.For<IConsoleFacade>();
        var action = new InputGridSizeAction(console);
    
        // Act
        action.Execute();
    
        // Assert
        console.Received(1).WriteLine("Please enter grid size in w h format (example: 10 15):");
    }

    [Fact]
    public void Should_Return_DisplayMenuAction_After_Execution()
    {
        var console = Substitute.For<IConsoleFacade>();
        var action = new InputGridSizeAction(console);

        var actionResult = action.Execute();

        Assert.IsType<DisplayMenuAction>(actionResult.NextAction);
    }

    [Theory]
    [InlineData(10, 15)]
    [InlineData(7, 19)]
    public void Should_Record_User_Inputted_Grid_Size(int width, int height)
    {
        // Arrange
        var console = Substitute.For<IConsoleFacade>();
        var action = new InputGridSizeAction(console);
        console.ReadLine().Returns($"{width} {height}");
        
        // Act
        var actionResult = action.Execute();
        var newGameState = actionResult.GameState;
        
        // Assert
        Assert.Equal(width, newGameState.Width);
        Assert.Equal(height, newGameState.Height);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1 7 5")]
    [InlineData("afsdf fsadf asdf 66")]
    public void Should_Display_Error_When_User_Enter_Invalid_Number_Of_Word_Tokens(string input)
    {
        var console = Substitute.For<IConsoleFacade>();
        var action = new InputGridSizeAction(console);
        console.ReadLine().Returns(input);
        
        action.Execute();
        
        console.Received(1).WriteLine("Wrong format for grid size!!! Please enter 2 integers in w h format (example: 10 15).");
    }

    [Theory]
    [InlineData("ab cd")]
    [InlineData("8 cd")]
    [InlineData("cd 7")]
    public void Should_Display_Error_When_User_Enter_Non_Numeric_Input(string input)
    {
        var console = Substitute.For<IConsoleFacade>();
        var action = new InputGridSizeAction(console);
        console.ReadLine().Returns(input);
        
        action.Execute();
        
        console.Received(1).WriteLine("Width and Height must be numerical!!! Please enter 2 integers in w h format (example: 10 15).");
    }
}