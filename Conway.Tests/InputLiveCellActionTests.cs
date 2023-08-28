﻿using NSubstitute;

namespace Conway.Tests;

public class InputLiveCellActionTests
{
    [Fact]
    public void Should_Prompt_For_Live_Cell()
    {
        var console = Substitute.For<IConsoleFacade>();
        var action = new InputLiveCellAction(console, new GameState());

        action.Execute();
        
        console.Received().WriteLine("Please enter live cell coordinate in x y format, ~ to clear all the previously entered cells or # to go back to main menu:");
    }
    
    [Fact]
    public void Should_Return_DisplayMenuAction_After_Execution()
    {
        var console = Substitute.For<IConsoleFacade>();
        var action = new InputLiveCellAction(console, new GameState());

        var actionResult = action.Execute();

        Assert.IsType<DisplayMenuAction>(actionResult.NextAction);
    }
    
    [Theory]
    [InlineData("asdf safasdf asfdf")]
    [InlineData("5 6 7")]
    public void Should_Display_Error_And_Prompt_Again_After_User_Enters_Invalid_Number_Of_Word_Tokens(string input)
    {
        var console = Substitute.For<IConsoleFacade>();
        var action = new InputLiveCellAction(console, new GameState());
        console.ReadLine().Returns(input);
        
        action.Execute();
        
        console.Received(3).WriteLine("Wrong format for live cell!");
        console.Received(3).WriteLine("Please enter live cell coordinate in x y format, ~ to clear all the previously entered cells or # to go back to main menu:");
        console.Received(3).ReadLine();
    }
    
    [Theory]
    [InlineData("fsadfe")]
    [InlineData("567")]
    public void Should_Display_Error_And_Prompt_Again_After_User_Enters_Wrong_Termination_Characters(string input)
    {
        var console = Substitute.For<IConsoleFacade>();
        var action = new InputLiveCellAction(console, new GameState());
        console.ReadLine().Returns(input);
        
        action.Execute();
        
        console.Received(3).WriteLine("Wrong format for live cell!");
        console.Received(3).WriteLine("Please enter live cell coordinate in x y format, ~ to clear all the previously entered cells or # to go back to main menu:");
        console.Received(3).ReadLine();
    }
    
    [Theory]
    [InlineData("fsadfe dsafds")]
    [InlineData("5 ttt")]
    [InlineData("ttt 5")]
    public void Should_Display_Error_And_Prompt_Again_After_User_Enters_Non_Numerical_Input_For_Live_Cell_Coordinates(string input)
    {
        var console = Substitute.For<IConsoleFacade>();
        var action = new InputLiveCellAction(console, new GameState());
        console.ReadLine().Returns(input);
        
        action.Execute();
        
        console.Received(3).WriteLine("Live cell coordinates must be numerical!");
        console.Received(3).WriteLine("Please enter live cell coordinate in x y format, ~ to clear all the previously entered cells or # to go back to main menu:");
        console.Received(3).ReadLine();
    }
    
    [Fact]
    public void Should_Record_All_User_Inputted_Live_Cell_Coordinates_Until_Terminating_Character()
    {
        var console = Substitute.For<IConsoleFacade>();
        var action = new InputLiveCellAction(console, new GameState(LiveCells: new List<Cell>()));
        console.ReadLine().Returns("1 2", "2 2", "3 1", "#");
        
        var actionResult = action.Execute();
        var newGameState = actionResult.GameState;
        
        Assert.Equal(3, newGameState.LiveCells.Count);
        var expectedLiveCells = new List<Cell> { new(1, 2), new (2, 2), new(3, 1) };
        Assert.Equal(expectedLiveCells, newGameState.LiveCells);
    }

    [Fact]
    public void Should_Clear_All_Previously_Entered_Cells_If_Tilda_Is_Entered()
    {
        var console = Substitute.For<IConsoleFacade>();
        var action = new InputLiveCellAction(console, new GameState(LiveCells: new List<Cell>()));
        console.ReadLine().Returns("1 2", "2 2", "3 1", "~");
        
        var actionResult = action.Execute();
        var newGameState = actionResult.GameState;
        
        Assert.Empty(newGameState.LiveCells);
    }
    
    [Fact]
    public void Should_Retain_CurrentGameState_Other_Than_LiveCells_When_Input_Successful()
    {
        var console = Substitute.For<IConsoleFacade>();
        var currentGameState = new GameState(10, 2, 5, new List<Cell>());
        var action = new InputLiveCellAction(console, currentGameState);
        console.ReadLine().Returns("1 2", "2 2", "3 1", "~");

        var actionResult = action.Execute();
        
        Assert.Equal(currentGameState.Width, actionResult.GameState.Width);
        Assert.Equal(currentGameState.Height, actionResult.GameState.Height);
        Assert.Equal(currentGameState.NumGen, actionResult.GameState.NumGen);
    }
    
    [Fact]
    public void Should_Retain_CurrentGameState_Other_Than_LiveCells_When_Input_Unsuccessful()
    {
        var console = Substitute.For<IConsoleFacade>();
        var currentGameState = new GameState(10, 2, 5, new List<Cell>());
        var action = new InputLiveCellAction(console, currentGameState);

        var actionResult = action.Execute();
        
        Assert.Equal(currentGameState.Width, actionResult.GameState.Width);
        Assert.Equal(currentGameState.Height, actionResult.GameState.Height);
        Assert.Equal(currentGameState.NumGen, actionResult.GameState.NumGen);
    }
}