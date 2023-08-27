namespace Conway;

public record GameState(int Width = 0, int Height = 0, int NumGen = 0, List<Cell> LiveCells = null);