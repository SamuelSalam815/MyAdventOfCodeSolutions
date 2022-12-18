namespace Day17;

public class Repeater<T>
{
        private int index;
        private readonly T[] items;
        public Repeater(IEnumerable<T> items, int startingIndex = 0)
        {
                this.items = items.ToArray();
                index = startingIndex;
        }

        public T NextValue()
        {
                T item = this.items[index++];

                if(index == items.Length)
                {
                        index = 0;
                }

                return item;
        }
}
