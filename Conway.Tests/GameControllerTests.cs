using NSubstitute;

namespace Conway.Tests;

public class GameControllerTests
{
    [Fact]
    public void Should_Start_The_Game_With_DisplayMenuAction_And_A_Fresh_GameState()
    {
        var gameController = new GameController();
        var freshGameState = new GameState(LiveCells: new List<Cell>());

        Assert.IsType<DisplayMenuAction>(gameController.CurrentAction);
        Assert.Equal(freshGameState.Width, gameController.CurrentGameState.Width);
        Assert.Equal(freshGameState.Height, gameController.CurrentGameState.Height);
        Assert.Equal(freshGameState.NumGen, gameController.CurrentGameState.NumGen);
        Assert.Equal(freshGameState.LiveCells, gameController.CurrentGameState.LiveCells);
    }

    [Fact]
    public void Should_Keep_Executing_The_Next_Action_Until_Reaching_TerminateAction()
    {
        var gameController = new GameController();
        var gameState = new GameState(LiveCells: new List<Cell>());

        var currentAction = Substitute.For<IAction>();
        var nextAction1 = Substitute.For<IAction>();
        var nextAction2 = Substitute.For<IAction>();
        currentAction.Execute().Returns(new ActionResult(gameState, nextAction1));
        nextAction1.Execute().Returns(new ActionResult(gameState, nextAction2));
        nextAction2.Execute().Returns(new ActionResult(gameState, new TerminateAction()));

        gameController.CurrentAction = currentAction;
        gameController.Play();

        currentAction.Received(1).Execute();
        nextAction1.Received(1).Execute();
        nextAction2.Received(1).Execute();
    }

    [Fact]
    public void Should_Update_CurrentGameState_And_CurrentAction_Until_Reaching_TerminateAction()
    {
        var gameController = new GameController();

        var currentAction = Substitute.For<IAction>();
        var nextAction1 = Substitute.For<IAction>();
        var terminateAction = new TerminateAction();

        var gameState1 = new GameState(1, 1, 1, new List<Cell> { new(1, 1) });
        currentAction.Execute().Returns(new ActionResult(gameState1, nextAction1));

        var gameState2 = new GameState(2, 2, 2, new List<Cell> { new(2, 2) });
        nextAction1.Execute().Returns(new ActionResult(gameState2, terminateAction));

        gameController.CurrentAction = currentAction;
        gameController.Play();

        Assert.Equal(gameState2, gameController.CurrentGameState);
        Assert.Equal(terminateAction, gameController.CurrentAction);
    }
}