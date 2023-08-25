namespace Conway;

public class DisplayMenuAction: IAction
{
    public IConsoleFacade Console { get; set; }

    public DisplayMenuAction(IConsoleFacade console)
    {
        Console = console;
    }

    public IAction Execute()
    {
        Console.WriteLine(@"Welcome to Conway's Game of Life
[1] Specify grid size
[2] Specify number of generation
[3] Specify initial live cells
[4] Run
Please enter your selection");
        return new TerminateAction();
    }
}