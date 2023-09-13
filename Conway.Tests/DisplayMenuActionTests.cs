using NSubstitute;

namespace Conway.Tests;

public class DisplayMenuActionTests
{
    private readonly IConsoleFacade _console;
    private readonly List<IAction> _actions;

    public DisplayMenuActionTests()
    {
        _console = Substitute.For<IConsoleFacade>();
        var factory = Substitute.For<IDisplayMenuActionFactory>();
        _actions = new List<IAction> 
        { 
            new InputGridSizeAction(_console), 
            new InputNumberOfGenerationAction(_console), 
            new InputLiveCellAction(_console),
            new PrintGameStateAction(_console),
            new RunAction(_console),
            new TerminateAction()
        };
        factory.Get().Returns(new DisplayMenuAction(_console, _actions));
    }

    [Fact]
    public void Should_Print_Menu_With_ActionMessage_For_Each_Action()
    {
        var currentGameState = new GameState();
        var displayMenuAction = new DisplayMenuAction(_console, _actions);
        _console.ReadLine().Returns("1");
        
        displayMenuAction.Execute(currentGameState);

        _console.Received(1).WriteLine("Welcome to Conway's Game of Life");
        for (var i = 0; i < _actions.Count; i++)
        {
            _console.Received(1).WriteLine($"[{i + 1}] {_actions[i].Message}");
        }
        _console.Received(1).WriteLine("Please enter your selection");
    }

    [Theory]
    [InlineData("")]
    [InlineData("asjdlkf")]
    [InlineData("1 1")]
    [InlineData("1 2 3")]
    [InlineData("10")]
    public void Should_Display_Error_And_Prompt_Again_When_User_Enters_Invalid_Input(string input)
    {
        var currentGameState = new GameState();
        var action = new DisplayMenuAction(_console, _actions);
        _console.ReadLine().Returns(input, "1");

        action.Execute(currentGameState);

        _console.Received(1).WriteLine("Invalid input! Please enter your option between 1-5.");
        _console.Received(2).WriteLine("Welcome to Conway's Game of Life");
        _console.Received(2).WriteLine("Please enter your selection");
    }

    [Theory]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("3")]
    [InlineData("4")]
    [InlineData("5")]
    [InlineData("6")]
    public void Should_Return_CurrentGameState(string input)
    {
        var currentGameState = new GameState();
        var action = new DisplayMenuAction(_console, _actions);
        _console.ReadLine().Returns(input);

        var actionResult = action.Execute(currentGameState);

        Assert.Equal(currentGameState, actionResult.GameState);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    public void Should_Record_And_Return_User_Selected_NextAction(int input)
    {
        var currentGameState = new GameState();
        var action = new DisplayMenuAction(_console, _actions);
        _console.ReadLine().Returns(input.ToString());

        var actionResult = action.Execute(currentGameState);

        Assert.Equal(actionResult.NextAction, _actions[input - 1]);
    }
}