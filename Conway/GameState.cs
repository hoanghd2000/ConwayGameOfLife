namespace Conway;

public record GameState(int Width, int Height, int NumGen, List<Cell> LiveCells);