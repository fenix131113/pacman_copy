using System;
using UnityEngine;

namespace Player
{
    public class PcPlayerInput : IPlayerInput
    {
        public event Action<Vector2> OnMove;
        
        public void Tick()
        {
            OnMove?.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), -Input.GetAxisRaw("Vertical")).normalized);
        }
    }
}