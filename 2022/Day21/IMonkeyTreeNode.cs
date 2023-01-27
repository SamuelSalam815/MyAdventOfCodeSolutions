namespace Day21;

internal interface IMonkeyTreeNode
{
    public string Name { get; }
    public long GetValue();

    public long InferValue();
}
