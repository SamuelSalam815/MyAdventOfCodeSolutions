namespace Day21;

internal class MonkeyOperationNode : IMonkeyTreeNode
{
    private long? _cachedValue = null;
    private bool? _cachedHasHumanDescendant = null;
    private long? _cachedInferredValue = null;
    private readonly string _leftChildName;
    private readonly string _rightChildName;
    private readonly IDictionary<string, IMonkeyTreeNode> _nodeMap;

    public readonly Func<long, long, long> CalculateOperation;
    public readonly Func<long, long, long> CalculateLeftChildOperation;
    public readonly Func<long, long, long> CalculateRightChildOperation;

    public IMonkeyTreeNode RightChild => _nodeMap[_rightChildName];
    public IMonkeyTreeNode LeftChild => _nodeMap[_leftChildName];

    public string Name { get; private set; }

    public MonkeyOperationNode(
        string name,
        string leftChildName,
        string rightChildName,
        IDictionary<string, IMonkeyTreeNode> nodeMap,
        Func<long, long, long> calculateValueOperation,
        Func<long, long, long> calculateLeftChildOperation,
        Func<long, long, long> calculateRightChildOperation)
    {
        Name = name;
        _leftChildName = leftChildName;
        _rightChildName = rightChildName;
        _nodeMap = nodeMap;

        CalculateOperation = calculateValueOperation;
        CalculateLeftChildOperation = calculateLeftChildOperation;
        CalculateRightChildOperation = calculateRightChildOperation;
    }
    public long GetValue()
    {
        if (_cachedValue is not null)
        {
            return _cachedValue.Value;
        }

        _cachedValue = CalculateOperation(LeftChild.GetValue(), RightChild.GetValue());

        return _cachedValue.Value;
    }

    private bool HasHumanDescendant()
    {
        if (_cachedHasHumanDescendant is not null)
        {
            return _cachedHasHumanDescendant.Value;
        }

        if (LeftChild.Name.Equals(Program.HumanName) || (LeftChild is MonkeyOperationNode leftOperation && leftOperation.HasHumanDescendant()))
        {
            _cachedHasHumanDescendant = true;
        }
        else if (RightChild.Name.Equals(Program.HumanName) || (RightChild is MonkeyOperationNode rightOperation && rightOperation.HasHumanDescendant()))
        {
            _cachedHasHumanDescendant = true;
        }
        else
        {
            _cachedHasHumanDescendant = false;
        }

        return _cachedHasHumanDescendant.Value;
    }

    private MonkeyOperationNode GetParentNode()
    {
        foreach (KeyValuePair<string, IMonkeyTreeNode> entry in _nodeMap)
        {
            IMonkeyTreeNode node = entry.Value;
            if (node is MonkeyOperationNode potentialParentNode)
            {
                if (ReferenceEquals(potentialParentNode.LeftChild, this) || ReferenceEquals(potentialParentNode.RightChild, this))
                {
                    return potentialParentNode;
                }
            }
        }

        throw new Exception($"Parent node of node {Name} not found");
    }

    public long InferValue()
    {
        if (_cachedInferredValue is not null)
        {
            return _cachedInferredValue.Value;
        }

        if (Name.Equals(Program.RootName))
        {
            throw new Exception("Invalid operation on root node");
        }

        MonkeyOperationNode parent = GetParentNode();
        bool isThisLeftChild = ReferenceEquals(parent.LeftChild, this);

        IMonkeyTreeNode siblingNode;
        Func<long, long, long> inferenceFunction;

        if (isThisLeftChild)
        {
            siblingNode = parent.RightChild;
            inferenceFunction = parent.CalculateLeftChildOperation;
        }
        else
        {
            siblingNode = parent.LeftChild;
            inferenceFunction = parent.CalculateRightChildOperation;
        }

        if (parent.Name.Equals(Program.RootName))
        {
            _cachedInferredValue = siblingNode.GetValue();
        }
        else
        {
            _cachedInferredValue = inferenceFunction(parent.InferValue(), siblingNode.GetValue());
        }

        return _cachedInferredValue.Value;
    }

}
