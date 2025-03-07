using System;
using UnityEngine;

namespace Player
{
    public class PlayerHealth
    {
        public int Health { get; private set; } = 3;
        public int MaxHealth { get; private set; } = 3;
        
        public event Action OnHealthChanged;

        public void TakeDamage()
        {
            Health = Mathf.Clamp(Health - 1, 0, MaxHealth);
            OnHealthChanged?.Invoke();
        }

        public void HealOne()
        {
            Health = Mathf.Clamp(Health + 1, 0, MaxHealth);
            OnHealthChanged?.Invoke();
        }
    }
}
