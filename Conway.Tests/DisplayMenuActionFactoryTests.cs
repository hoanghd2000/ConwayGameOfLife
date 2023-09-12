using NSubstitute;

namespace Conway.Tests;

public class DisplayMenuActionFactoryTests
{
    [Fact]
    public void Should_Create_New_Display_Menu_Action_If_There_Is_None_Existing()
    {
        var console = Substitute.For<IConsoleFacade>();
        var actions = new List<IAction>();
        var factory = new DisplayMenuActionFactory(console, actions);

        var action = factory.Get();

        Assert.IsType<DisplayMenuAction>(action);
        Assert.Same(console, action.Console);
        Assert.Equal(actions, action.Actions);
    }
    
    [Fact]
    public void Should_Return_Existing_Display_Menu_Action_If_There_Is_One_Existing()
    {
        var console = Substitute.For<IConsoleFacade>();
        var factory = new DisplayMenuActionFactory(console, new List<IAction>());

        var expectedAction = factory.Get();
        var resultingAction = factory.Get();

        Assert.IsType<DisplayMenuAction>(resultingAction);
        Assert.Equal(expectedAction, resultingAction);
    }
}