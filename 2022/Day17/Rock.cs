namespace Day17;

public class Rock : List<Position>
{
        public Rock() : base() { }

        public Rock(IEnumerable<Position> positions) : base(positions) { }

        public long MaxX { get; private set; }
        public long MinX { get; private set; }
        public long MaxY { get; private set; }
        public long MinY { get; private set; }

        public new void Add( Position position)
        {
                if(this.Count == 0)
                {
                        MinX = MaxX = position.X;
                        MinY = MaxY = position.Y;
                }
                else
                {
                        if (position.X < MinX) MinX = position.X;
                        if (position.X > MaxX) MaxX = position.X;

                        if (position.Y < MinY) MinY = position.Y;
                        if (position.Y > MaxY) MaxY = position.Y;
                }

                base.Add(position);
        }

        public void Translate(long deltaX, long deltaY)
        {
                for(int positionIndex = 0; positionIndex < this.Count; positionIndex++)
                {
                        this[positionIndex] = new Position(this[positionIndex].X + deltaX, this[positionIndex].Y + deltaY);
                        
                }
                MinX += deltaX;
                MaxX += deltaX;

                MinY += deltaY;
                MaxY += deltaY;
        }
}
