namespace Conway;

public class GameController
{
    private IConsoleFacade Console { get; }
    public GameState CurrentGameState { get; set; }
    private IAction CurrentAction { get; set; }

    public GameController()
    {
        Console = new ConsoleFacade();
        CurrentAction = new DisplayMenuAction(Console);
        CurrentGameState = new GameState();
    }

    public void Play()
    {
        try
        {
            while (CurrentAction is not TerminateAction)
            {
                var actionResult = CurrentAction.Execute();
                CurrentGameState = actionResult.GameState;
                CurrentAction = actionResult.NextAction;

                if (CurrentAction is TerminateAction)
                {
                    var input = Console.ReadLine();
                    var inputNum = int.Parse(input);
                    CurrentAction = GetAction(inputNum);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private IAction GetAction(int inputNum)
    {
        switch (inputNum)
        {
            case 1:
                return new InputGridSizeAction(Console, CurrentGameState);
            default:
                return new TerminateAction();
        }
    }
}