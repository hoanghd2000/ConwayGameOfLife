namespace Conway;

public class TerminateAction : IAction
{
    public IConsoleFacade Console { get; set; }
    public IAction Execute()
    {
        throw new NotImplementedException();
    }
}