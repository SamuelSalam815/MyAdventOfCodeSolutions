using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day21;

internal class MonkeyOperationNode : IMonkeyTreeNode
{
    private long? _cachedValue = null;
    private readonly string _leftChildName;
    private readonly string _rightChildName;
    private readonly Func<long, long, long> _operation;
    private readonly IDictionary<string, IMonkeyTreeNode> _nodeMap;

    public MonkeyOperationNode(string leftChildName, string rightChildName, Func<long, long, long> operation, IDictionary<string, IMonkeyTreeNode> nodeMap)
    {
        _leftChildName = leftChildName;
        _rightChildName = rightChildName;
        _operation = operation;
        _nodeMap = nodeMap;
    }

    private long NodeNameToValue(string nodeName)
    {
        if (_nodeMap.TryGetValue(nodeName, out IMonkeyTreeNode? childNode) && childNode is not null)
        {
            return childNode.GetValue();
        }

        throw new Exception($"Required the value of node with name {nodeName}. Could not find this node.");
    }
    public long GetValue()
    {
        if (_cachedValue is not null)
        {
            return _cachedValue.Value;
        }

        long leftValue = NodeNameToValue(_leftChildName);
        long rightValue = NodeNameToValue(_rightChildName);
        _cachedValue = _operation(leftValue, rightValue);

        return _cachedValue.Value;
    }
}
