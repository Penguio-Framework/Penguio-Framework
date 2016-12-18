namespace Engine.Interfaces
{
    public class DragGesture
    {
        public DragGesture(int distance, Direction direction)
        {
            Distance = distance;
            Direction = direction;
        }

        public int Distance { get; set; }
        public Direction Direction { get; set; }
    }
}