namespace Conway;

public interface IConsoleFacade
{
    public string? ReadLine();
    public void WriteLine(string line);
}

public class ConsoleFacade : IConsoleFacade
{
    public string? ReadLine()
    {
        return Console.ReadLine();
    }
    
    public void WriteLine(string line)
    {
        Console.WriteLine(line);
    }
}