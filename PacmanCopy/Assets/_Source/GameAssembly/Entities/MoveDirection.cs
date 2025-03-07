using System;

namespace Entities
{
    public static class MoveDirectionUtils
    {
        public static int GetDirectionDegrees(MoveDirection moveDirection)
        {
            return moveDirection switch
            {
                MoveDirection.UP => 90,
                MoveDirection.RIGHT => 0,
                MoveDirection.DOWN => 270,
                MoveDirection.LEFT => 180,
                _ => throw new ArgumentOutOfRangeException(nameof(moveDirection), moveDirection, null)
            };
        }

        public static MoveDirection InverseDirection(MoveDirection moveDirection)
        {
            return moveDirection switch
            {
                MoveDirection.UP => MoveDirection.DOWN,
                MoveDirection.RIGHT => MoveDirection.LEFT,
                MoveDirection.DOWN => MoveDirection.UP,
                MoveDirection.LEFT => MoveDirection.RIGHT,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public enum MoveDirection
    {
        UP = 0,
        RIGHT = 1,
        DOWN = 2,
        LEFT = 3
    }
}