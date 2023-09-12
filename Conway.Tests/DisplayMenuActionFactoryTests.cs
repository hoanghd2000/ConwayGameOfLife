using NSubstitute;

namespace Conway.Tests;

public class DisplayMenuActionFactoryTests
{
    [Fact]
    public void Should_Create_Display_Menu_Action()
    {
        var console = Substitute.For<IConsoleFacade>();
        var factory = new DisplayMenuActionFactory(console);

        var action = factory.Create();

        Assert.IsType<DisplayMenuAction>(action);
        Assert.Same(console, action.Console);
    }
}