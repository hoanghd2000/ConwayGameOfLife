namespace Conway;

public class TerminateAction : IAction
{
    public IConsoleFacade Console { get; set; }
    public ActionResult Execute()
    {
        throw new NotImplementedException();
    }
}