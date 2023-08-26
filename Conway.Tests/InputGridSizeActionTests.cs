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
        console.Received().WriteLine("Please enter grid size in w h format (example: 10 15):");
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
    public void Should_Display_Error_When_User_Enters_Invalid_Number_Of_Word_Tokens(string input)
    {
        var console = Substitute.For<IConsoleFacade>();
        var action = new InputGridSizeAction(console);
        console.ReadLine().Returns(input);
        
        action.Execute();
        
        console.Received(3).WriteLine("Wrong format for grid size!!!");
    }

    [Theory]
    [InlineData("ab cd")]
    [InlineData("8 cd")]
    [InlineData("cd 7")]
    public void Should_Display_Error_When_User_Enters_Non_Numeric_Input(string input)
    {
        var console = Substitute.For<IConsoleFacade>();
        var action = new InputGridSizeAction(console);
        console.ReadLine().Returns(input);
        
        action.Execute();
        
        console.Received(3).WriteLine("Width and Height must be numerical!!!");
    }

    [Theory]
    [InlineData("26 1")]
    [InlineData("3 30")]
    [InlineData("-1 0")]
    public void Should_Display_Error_And_Prompt_Again_When_User_Enters_Out_Of_Range_Input(string input)
    {
        var console = Substitute.For<IConsoleFacade>();
        var action = new InputGridSizeAction(console);
        console.ReadLine().Returns(input);
        
        action.Execute();
        
        console.Received(3).WriteLine("Width and Height must be between 1 and 25 inclusive!!!");
        console.Received(3).ReadLine();
    }
}