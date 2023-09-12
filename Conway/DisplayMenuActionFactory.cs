namespace Conway;

public interface IDisplayMenuActionFactory
{
    DisplayMenuAction Get();
}

public class DisplayMenuActionFactory : IDisplayMenuActionFactory
{
    private readonly IConsoleFacade _console;
    private readonly IEnumerable<IAction> _actions;
    private DisplayMenuAction? _displayMenuAction;
    
    public DisplayMenuActionFactory(IConsoleFacade console, IEnumerable<IAction> actions)
    {
        _console = console;
        _actions = actions;
    }
    
    public DisplayMenuAction Get()
    {
        return _displayMenuAction ??= new DisplayMenuAction(_console, _actions);
    }
}