namespace Conway;

public interface IDisplayMenuActionFactory
{
    DisplayMenuAction Create();
}

public class DisplayMenuActionFactory : IDisplayMenuActionFactory
{
    private readonly IConsoleFacade _console;
    
    public DisplayMenuActionFactory(IConsoleFacade console)
    {
        _console = console;
    }
    
    public DisplayMenuAction Create()
    {
        return new DisplayMenuAction(_console);
    }
}