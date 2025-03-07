using Level;

namespace Entities.Data
{
    public class PathRotationGroup
    {
        public FloorCell Cell { get; private set; }
        public MoveDirection Direction { get; private set; }

        public PathRotationGroup(FloorCell cell, MoveDirection direction)
        {
            Cell = cell;
            Direction = direction;
        }
    }
}