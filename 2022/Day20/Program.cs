namespace Day20;

internal class Program
{
    private static void Main(string[] args)
    {
        CircularList<int> circularList = new(new List<int>() { 1, 2, -3, 3, -2, 0, 4 });
        Console.WriteLine(circularList.GetNodeByValue(0));
    }
}
