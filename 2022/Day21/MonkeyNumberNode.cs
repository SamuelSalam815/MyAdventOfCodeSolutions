﻿namespace Day21;

internal class MonkeyNumberNode : IMonkeyTreeNode
{
    public string Name { get; private set; }

    private readonly long _value;
    public MonkeyNumberNode(string name, long value)
    {
        Name = name;
        _value = value;
    }
    public long GetValue() => _value;

    public long InferValue() => _value;
}
