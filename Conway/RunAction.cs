namespace Conway;

public class RunAction : IAction
{
    private IConsoleFacade Console { get; }
    private GameState CurrentGameState { get; }

    public RunAction(IConsoleFacade console, GameState currentGameState)
    {
        Console = console;
        CurrentGameState = currentGameState;
    }

    public ActionResult Execute()
    {
        if (!CurrentGameState.LiveCells.Any())
        {
            Console.WriteLine("End of generation. Press any key to return to main menu");
            Console.ReadLine();
            return new ActionResult(CurrentGameState, new DisplayMenuAction(Console, CurrentGameState));
        }
        
        var board = InitializeBoard();
        Console.WriteLine("Initial position");
        PrintBoard(board);
        
        var isTerminateChar = false;
        for (var i = 0; i < CurrentGameState.NumGen; i++)
        {
            Console.WriteLine("Enter > to go to next generation or # to go back to main menu");
            Console.WriteLine("");
            
            var isNextGenChar = false;
            while (!isNextGenChar && !isTerminateChar)
            {
                var input = Console.ReadLine();
                isNextGenChar = TryParseNextGenChar(input);
                isTerminateChar = TryParseTerminatingChar(input);
            }

            if (isTerminateChar)
            {
                break;
            }
            
            board = ProcessOneIteration(board);
            Console.WriteLine($"Generation {i+1}");
            PrintBoard(board);
        }

        if (!isTerminateChar)
        {
            Console.WriteLine("End of generation. Press any key to return to main menu");
            Console.ReadLine();
        }
        
        return new ActionResult(CurrentGameState, new DisplayMenuAction(Console, CurrentGameState));
    }

    public bool[, ] InitializeBoard()
    {
        var board = new bool[CurrentGameState.Height, CurrentGameState.Width];
        foreach (var liveCell in CurrentGameState.LiveCells)
        {
            board[liveCell.Y, liveCell.X] = true;
        }

        return board;
    }

    public bool[, ] ProcessOneIteration(bool[, ] board)
    {
        var resultBoard = new bool[CurrentGameState.Height, CurrentGameState.Width];
        for (var i = 0; i < CurrentGameState.Height; i++)
        {
            for (var j = 0; j < CurrentGameState.Width; j++)
            {
                var numLiveNeighbors = CountNumLiveNeighbors(board, j, i);
                if (board[i, j])
                {
                    if (numLiveNeighbors is 2 or 3)
                    {
                        resultBoard[i, j] = true;
                    }
                    else
                    {
                        resultBoard[i, j] = false;
                    }
                }
                else
                {
                    if (numLiveNeighbors == 3)
                    {
                        resultBoard[i, j] = true;
                    }
                    else
                    {
                        resultBoard[i, j] = false;
                    }
                }
            }
        }

        return resultBoard;
    }

    public int CountNumLiveNeighbors(bool[, ] board, int x, int y)
    {
        if (x < 0 || x >= CurrentGameState.Width || y < 0 || y >= CurrentGameState.Height)
        {
            throw new ApplicationException("Cell coordinates out of range!");
        }

        var numLiveNeighbors = 0;

        var xLeftBound = x - 1 < 0 ? 0 : x - 1;
        var xRightBound = x + 1 > CurrentGameState.Width - 1 ? CurrentGameState.Width - 1 : x + 1;
        var yLeftBound = y - 1 < 0 ? 0 : y - 1;
        var yRightBound = y + 1 > CurrentGameState.Height - 1 ? CurrentGameState.Height - 1 : y + 1;

        for (var i = xLeftBound; i <= xRightBound; i++)
        {
            for (var j = yLeftBound; j <= yRightBound; j++)
            {
                if (i == x && j == y)
                {
                    continue;
                }
                
                if (board[j, i])
                {
                    numLiveNeighbors++;
                }
            }
        }

        return numLiveNeighbors;
    }

    private void PrintBoard(bool[,] board)
    {
        for (var i = 0; i < board.GetLength(0); i++)
        {
            for (var j = 0; j < board.GetLength(1); j++)
            {
                System.Console.Write(board[i, j] ? "o" : ".");
            }
            System.Console.WriteLine();
        }
    }

    private bool TryParseNextGenChar(string input)
    {
        var inputTokens = input.Split();
        if (inputTokens.Length != 1)
        {
            return false;
        }

        return inputTokens[0] == ">";
    }
    
    private bool TryParseTerminatingChar(string input)
    {
        var inputTokens = input.Split();
        if (inputTokens.Length != 1)
        {
            return false;
        }

        return inputTokens[0] == "#";
    }
}