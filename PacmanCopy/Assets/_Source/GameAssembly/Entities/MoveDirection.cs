using System;

namespace Entities
{
    public static class MoveDirectionUtils
    {
        public static int GetDirectionDegrees(MoveDirection moveDirection)
        {
            return moveDirection switch
            {
                MoveDirection.UP => 270,
                MoveDirection.RIGHT => 0,
                MoveDirection.DOWN => 90,
                MoveDirection.LEFT => 180,
                _ => throw new ArgumentOutOfRangeException(nameof(moveDirection), moveDirection, null)
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