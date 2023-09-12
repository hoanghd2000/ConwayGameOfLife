using NSubstitute;

namespace Conway.Tests;

public class RunActionTests
{
    [Fact]
    public void Should_Prompt_User_To_Return_To_Main_Menu_When_No_Live_Cells_Are_Entered()
    {
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState(5, 5, 3, new List<Cell>());
        var action = new RunAction(console);

        action.Execute(gameState);
        
        console.Received(1).WriteLine("End of generation. Press any key to return to main menu");
        console.Received(1).ReadLine();
    }
    
    [Fact]
    public void Should_Initialize_Board_From_GameState()
    {
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState(5, 5, 3, 
            new List<Cell> { new(0, 0), new(1, 1), new(1, 2) });
        var action = new RunAction(console);

        var board = action.InitializeBoard(gameState);
        
        Assert.True(board[0, 0]);
        Assert.True(board[1, 1]);
        Assert.True(board[2, 1]);
        for (var i = 0; i < gameState.Height; i++)
        {
            for (var j = 0; j < gameState.Width; j++)
            {
                if (gameState.LiveCells.Contains(new Cell(j, i)))
                {
                    continue;
                }
                Assert.False(board[i, j]);
            }
        }
    }

    [Theory]
    [InlineData(5, 5)]
    [InlineData(0, 5)]
    [InlineData(5, 3)]
    [InlineData(-1, 3)]
    [InlineData(3, -1)]
    public void Should_Throw_Exception_When_Target_Cell_For_Live_Neighbours_Counting_Is_Out_Of_Range(int x, int y)
    {
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState(5, 5, 3, new List<Cell> ());
        var action = new RunAction(console);
        
        var board = action.InitializeBoard(gameState);
        Assert.Throws<ApplicationException>(() => action.CountNumLiveNeighbors(board, gameState, x, y));
    }

    [Fact]
    public void Should_Count_Number_Of_Live_Neighbours_For_Cell_In_A_1x1_Board()
    {
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState(1, 1, 3, new List<Cell> { new(0, 0) });
        var action = new RunAction(console);

        var board = action.InitializeBoard(gameState);
        var numLiveNeighbors = action.CountNumLiveNeighbors(board, gameState, 0, 0);
        
        Assert.Equal(0, numLiveNeighbors);
    }

    [Fact]
    public void Should_Count_Number_Of_Live_Neighbours_For_Cell_In_A_1x2_Board()
    {
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState(2, 1, 3, new List<Cell> { new(1, 0) });
        var action = new RunAction(console);

        var board = action.InitializeBoard(gameState);
        var numLiveNeighbors = action.CountNumLiveNeighbors(board, gameState, 0, 0);
        
        Assert.Equal(1, numLiveNeighbors);
    }
    
    [Fact]
    public void Should_Count_Number_Of_Live_Neighbours_For_Centre_Cell_In_A_1x3_Board()
    {
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState(3, 1, 3, new List<Cell> { new(0, 0), new(2, 0) });
        var action = new RunAction(console);

        var board = action.InitializeBoard(gameState);
        var numLiveNeighbors = action.CountNumLiveNeighbors(board, gameState,1, 0);
        
        Assert.Equal(2, numLiveNeighbors);
    }
    
    [Theory]
    [InlineData(0, 0)]
    [InlineData(2, 0)]
    public void Should_Count_Number_Of_Live_Neighbours_For_Edge_Cell_In_A_1x3_Board(int x, int y)
    {
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState(3, 1, 3, new List<Cell> { new(0, 0), new(2, 0) });
        var action = new RunAction(console);

        var board = action.InitializeBoard(gameState);
        var numLiveNeighbors = action.CountNumLiveNeighbors(board, gameState, x, y);
        
        Assert.Equal(0, numLiveNeighbors);
    }
    
    [Fact]
    public void Should_Count_Number_Of_Live_Neighbours_For_Cell_In_A_2x1_Board()
    {
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState(1, 2, 3, new List<Cell> { new(0, 1), new(0, 0) });
        var action = new RunAction(console);

        var board = action.InitializeBoard(gameState);
        var numLiveNeighbors = action.CountNumLiveNeighbors(board, gameState, 0, 0);
        
        Assert.Equal(1, numLiveNeighbors);
    }
    
    [Fact]
    public void Should_Count_Number_Of_Live_Neighbours_For_Centre_Cell_In_A_3x1_Board()
    {
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState(1, 3, 3, new List<Cell> { new(0, 0), new(0, 2), new (0, 1) });
        var action = new RunAction(console);

        var board = action.InitializeBoard(gameState);
        var numLiveNeighbors = action.CountNumLiveNeighbors(board, gameState,0, 1);
        
        Assert.Equal(2, numLiveNeighbors);
    }
    
    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 2)]
    public void Should_Count_Number_Of_Live_Neighbours_For_Edge_Cell_In_A_3x1_Board(int x, int y)
    {
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState(1, 3, 3, new List<Cell> { new(0, 0), new(0, 2), new (0, 1) });
        var action = new RunAction(console);

        var board = action.InitializeBoard(gameState);
        var numLiveNeighbors = action.CountNumLiveNeighbors(board, gameState, x, y);
        
        Assert.Equal(1, numLiveNeighbors);
    }

    [Fact]
    public void Should_Count_Number_Of_Live_Neighbours_For_A_Cell_In_A_Board_With_Both_Height_And_Width_Larger_Than_Or_Equal_To_3()
    {
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState(3, 3, 3, new List<Cell> { new(1, 0), new(1, 1), new (1, 2), new(0, 1), new(2, 1) });
        var action = new RunAction(console);

        var board = action.InitializeBoard(gameState);
        var numLiveNeighbors1 = action.CountNumLiveNeighbors(board, gameState, 1, 1);
        var numLiveNeighbors2 = action.CountNumLiveNeighbors(board, gameState, 0, 1);
        var numLiveNeighbors3 = action.CountNumLiveNeighbors(board, gameState, 1, 0);
        var numLiveNeighbors4 = action.CountNumLiveNeighbors(board, gameState, 2, 1);
        var numLiveNeighbors5 = action.CountNumLiveNeighbors(board, gameState, 1, 2);
        
        Assert.Equal(4, numLiveNeighbors1);
        Assert.Equal(3, numLiveNeighbors2);
        Assert.Equal(3, numLiveNeighbors3);
        Assert.Equal(3, numLiveNeighbors4);
        Assert.Equal(3, numLiveNeighbors5);
    }

    [Fact]
    public void Should_Ensure_That_LiveCell_With_Fewer_Than_2_Live_Neighbours_Dies()
    {
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState(5, 5, 3, new List<Cell> { new(3, 0), new(4, 0), new (2, 2), new(3, 1) });
        var action = new RunAction(console);

        var board = action.InitializeBoard(gameState);
        var resultBoard = action.ProcessOneIteration(board, gameState);
        
        Assert.False(resultBoard[2, 2]);
    }
    
    [Fact]
    public void Should_Ensure_That_LiveCell_With_2_Or_3_Live_Neighbours_Survives()
    {
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState(5, 5, 3, new List<Cell> { new(3, 0), new(4, 0), new (2, 2), new(3, 1), new( 2, 0) });
        var action = new RunAction(console);

        var board = action.InitializeBoard(gameState);
        var resultBoard = action.ProcessOneIteration(board, gameState);
        
        Assert.True(resultBoard[0, 3]);
        Assert.True(resultBoard[0, 4]);
    }
    
    [Fact]
    public void Should_Ensure_That_LiveCell_With_More_Than_3_Live_Neighbours_Dies()
    {
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState(5, 5, 3, new List<Cell> { new(3, 0), new(4, 0), new (2, 2), new(3, 1), new( 2, 0) });
        var action = new RunAction(console);

        var board = action.InitializeBoard(gameState);
        var resultBoard = action.ProcessOneIteration(board, gameState);
        
        Assert.False(resultBoard[1, 3]);
    }
    
    [Fact]
    public void Should_Ensure_That_DeadCell_With_Exactly_3_Live_Neighbours_Becomes_LiveCell()
    {
        var console = Substitute.For<IConsoleFacade>();
        var gameState = new GameState(5, 5, 3, new List<Cell> { new(3, 0), new(4, 0), new (2, 2), new(3, 1), new( 2, 0) });
        var action = new RunAction(console);

        var board = action.InitializeBoard(gameState);
        var resultBoard = action.ProcessOneIteration(board, gameState);
        
        Assert.True(resultBoard[1, 4]);
    }
    
    // [Fact]
    // public void Should_Run_The_Number_Of_Generations_Specified()
    // {
    //     var console = Substitute.For<IConsoleFacade>();
    //     var gameState = new GameState(5, 5, 3, new List<Cell> { new(3, 0), new(4, 0), new (2, 2), new(3, 1), new( 2, 0) });
    //     var action = new RunAction(console, gameState);
    //
    //     var board = action.Execute();
    //     
    //     
    // }
}