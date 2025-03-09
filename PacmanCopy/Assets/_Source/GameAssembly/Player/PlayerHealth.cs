using System;
using UnityEngine;

namespace Player
{
    public class PlayerHealth
    {
        public int Health { get; private set; } = 4;
        public int MaxHealth { get; private set; } = 4;
        
        public event Action OnHealthChanged;

        public void TakeDamage()
        {
            Health = Mathf.Clamp(Health - 1, 0, MaxHealth);
            OnHealthChanged?.Invoke();
        }
    }
}
