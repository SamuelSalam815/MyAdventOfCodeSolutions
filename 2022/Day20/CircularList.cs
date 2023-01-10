namespace Day20;

internal class CircularList<T> where T : struct, IEquatable<T>
{
    private Dictionary<T, CircularListNode<T>> valueToNodeMap;

    public CircularList(List<T> values)
    {
        valueToNodeMap = new();
        if (values.Count == 0)
        {
            return;
        }

        foreach (T value in values)
        {
            valueToNodeMap.Add(value, new CircularListNode<T>(value));
        }

        for (int i = 0; i < values.Count; i++)
        {
            int nextIndex = i + 1;
            if (i == values.Count - 1)
            {
                nextIndex = 0;
            }
            int previousIndex = i - 1;
            if (i == 0)
            {
                previousIndex = values.Count - 1;
            }
            valueToNodeMap[values[i]].Next = valueToNodeMap[values[nextIndex]];
            valueToNodeMap[values[i]].Previous = valueToNodeMap[values[previousIndex]];
        }
    }

    public CircularListNode<T> GetNodeByValue(T value) => valueToNodeMap[value];
}
