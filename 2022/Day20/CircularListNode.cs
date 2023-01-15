using System.Text;

namespace Day20;

internal class CircularListNode
{
    public long Value;
    public CircularListNode NextNode;
    public CircularListNode PreviousNode;


    public CircularListNode(IList<long> nextValues) : this(nextValues.GetEnumerator(), null, null) { }

    private CircularListNode(IEnumerator<long> ValueEnumerator, CircularListNode? PreviousNode, CircularListNode? FirstNode)
    {
        bool thisIsFirstNode = PreviousNode is null;
        if(thisIsFirstNode)
        {
            if(!ValueEnumerator.MoveNext())
            {
                throw new Exception("There must be at least one element in the collection");
            }
        }

        Value = ValueEnumerator.Current;
        if (ValueEnumerator.MoveNext())
        {
            NextNode = new(ValueEnumerator, this, FirstNode is null ? this : FirstNode);
            
            if(PreviousNode is not null)
            {
                this.PreviousNode = PreviousNode;
            }
            // If there is no previous value, then this is the first node
            // Exploit wrap-around mechanic to find the last node
            else
            {
                CircularListNode currentNode = NextNode;
                while(currentNode.NextNode != this)
                {
                    currentNode = currentNode.NextNode;
                }
                this.PreviousNode = currentNode;
            }
        }
        else // No values left, time to wrap-around
        {
            if(FirstNode is not null && PreviousNode is not null)
            {
                NextNode = FirstNode;
                this.PreviousNode = PreviousNode;
            }
            else // singleton case
            {
                NextNode = this;
                this.PreviousNode = this;
            }

        }
    }

    /// <summary>
    /// Prints the circular list, treating the node this is called on as the start of the list
    /// </summary>
    /// <returns>A string representation of the circular list</returns>
    public string GetListAsString()
    {
        StringBuilder builder = new();
        CircularListNode firstNode = this;
        builder.Append('[');
        builder.Append(Value);
        CircularListNode currentNode = this;
        while (currentNode.NextNode != firstNode)
        {
            currentNode = currentNode.NextNode;
            builder.Append(", ");
            builder.Append(currentNode.Value);
        }
        builder.Append(']');
        return builder.ToString();
    }

    public override string ToString() => Value.ToString();
}
