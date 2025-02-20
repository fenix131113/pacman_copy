using System;
using UnityEngine;
using VContainer.Unity;

namespace Player
{
    public interface IPlayerInput : ITickable
    {
        public event Action<Vector2> OnMove;
    }
}