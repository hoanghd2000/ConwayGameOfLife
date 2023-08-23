namespace Conway;

public interface IConsoleFacade
{
    public void WriteLine(string line);
}

public class ConsoleFacade : IConsoleFacade
{
    public void WriteLine(string line)
    {
        Console.WriteLine(line);
    }
}