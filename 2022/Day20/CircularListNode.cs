using System.Text;

namespace Day20;

internal class CircularListNode<T> where T : struct, IEquatable<T>
{
    public T Value;
    public CircularListNode<T>? Next;
    public CircularListNode<T>? Previous;

    public CircularListNode(
        T value,
        CircularListNode<T>? next = null,
        CircularListNode<T>? previous = null
    )
    {
        Value = value;
        Next = next;
        Previous = previous;
    }

    public override string ToString()
    {
        StringBuilder builder = new();
        T firstValue = Value;
        builder.Append('[');
        builder.Append(Value);
        CircularListNode<T> currentNode = this;
        while (currentNode.Next?.Value.Equals(firstValue) == false)
        {
            currentNode = currentNode.Next;
            builder.Append(", ");
            builder.Append(currentNode.Value);
        }
        builder.Append(']');
        return builder.ToString();
    }
}
