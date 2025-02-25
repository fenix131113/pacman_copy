using System;
using UnityEngine;

namespace Player
{
    public class Scores
    {
        public int Score { get; private set; }
        
        public event Action OnScoreChanged;

        public void AddScore(int score)
        {
            Score = Mathf.Clamp(Score + score, 0, int.MaxValue);
            OnScoreChanged?.Invoke();
        }
    }
}