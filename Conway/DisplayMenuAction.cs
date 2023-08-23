namespace Conway;

public class DisplayMenuAction
{
    public void Display(IConsoleFacade console)
    {
        console.WriteLine(@"Welcome to Conway's Game of Life
[1] Specify grid size
[2] Specify number of generation
[3] Specify initial live cells
[4] Run
Please enter your selection");
    }
}