namespace Conway;

public interface IAction
{
    public IConsoleFacade Console { get; set; }

    public IAction Execute();
}