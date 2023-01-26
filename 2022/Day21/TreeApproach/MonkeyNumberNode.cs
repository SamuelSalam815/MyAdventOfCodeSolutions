using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day21.TreeApproach;

internal class MonkeyNumberNode : IMonkeyTreeNode
{
    private readonly long _value;
    public MonkeyNumberNode(long value)
    {
        _value = value;
    }
    public long GetValue() => _value;
}
