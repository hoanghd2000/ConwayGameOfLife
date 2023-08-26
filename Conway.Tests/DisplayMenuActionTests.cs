using NSubstitute;

namespace Conway.Tests;

public class DisplayMenuActionTests
{
    [Fact]
    public void Should_Display_Menu_Action()
    {
        var console = Substitute.For<IConsoleFacade>();
        var action = new DisplayMenuAction(console);

        action.Execute();

        console.Received(1).WriteLine(@"Welcome to Conway's Game of Life
[1] Specify grid size
[2] Specify number of generation
[3] Specify initial live cells
[4] Run
Please enter your selection");
        
    }

    [Fact]
    public void Should_Return_TerminateAction_After_Execution()
    {
        var console = Substitute.For<IConsoleFacade>();
        var action = new DisplayMenuAction(console);

        var actionResult = action.Execute();

        Assert.IsType<TerminateAction>(actionResult.NextAction);
    }
}