namespace Conway;

public class RunAction : IAction
{
    public string Message { get; }
    private IConsoleFacade Console { get; }

    public RunAction(IConsoleFacade console)
    {
        Message = "Run";
        Console = console;
    }

    public ActionResult Execute(GameState currentGameState)
    {
        if (!currentGameState.LiveCells.Any())
        {
            Console.WriteLine("End of generation. Press any key to return to main menu");
            Console.ReadLine();
            return new ActionResult(currentGameState, new BackToMenuAction());
        }
        
        var board = InitializeBoard(currentGameState);
        Console.WriteLine("Initial position");
        PrintBoard(board);
        
        var isTerminateChar = false;
        for (var i = 0; i < currentGameState.NumGen; i++)
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
            
            board = ProcessOneIteration(board, currentGameState);
            Console.WriteLine($"Generation {i+1}");
            PrintBoard(board);
        }

        if (!isTerminateChar)
        {
            Console.WriteLine("End of generation. Press any key to return to main menu");
            Console.ReadLine();
        }
        
        return new ActionResult(currentGameState, new BackToMenuAction());
    }

    public bool[, ] InitializeBoard(GameState currentGameState)
    {
        var board = new bool[currentGameState.Height, currentGameState.Width];
        foreach (var liveCell in currentGameState.LiveCells)
        {
            board[liveCell.Y, liveCell.X] = true;
        }

        return board;
    }

    public bool[, ] ProcessOneIteration(bool[, ] board, GameState currentGameState)
    {
        var resultBoard = new bool[currentGameState.Height, currentGameState.Width];
        for (var i = 0; i < currentGameState.Height; i++)
        {
            for (var j = 0; j < currentGameState.Width; j++)
            {
                var numLiveNeighbors = CountNumLiveNeighbors(board, currentGameState, j, i);
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

    public int CountNumLiveNeighbors(bool[, ] board, GameState currentGameState, int x, int y)
    {
        if (x < 0 || x >= currentGameState.Width || y < 0 || y >= currentGameState.Height)
        {
            throw new ApplicationException("Cell coordinates out of range!");
        }

        var numLiveNeighbors = 0;

        var xLeftBound = x - 1 < 0 ? 0 : x - 1;
        var xRightBound = x + 1 > currentGameState.Width - 1 ? currentGameState.Width - 1 : x + 1;
        var yLeftBound = y - 1 < 0 ? 0 : y - 1;
        var yRightBound = y + 1 > currentGameState.Height - 1 ? currentGameState.Height - 1 : y + 1;

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