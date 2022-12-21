namespace Day17;

public class Repeater<T>
{
        public int Index { get; private set; }
        private readonly T[] items;
        public Repeater(IEnumerable<T> items, int startingIndex = 0)
        {
                this.items = items.ToArray();
                Index = startingIndex;
        }

        public T NextValue()
        {
                T item = this.items[Index++];

                if(Index == items.Length)
                {
                        Index = 0;
                }

                return item;
        }
}
