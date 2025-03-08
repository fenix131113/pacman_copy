using System;
using Player.Data;
using UnityEngine;
using VContainer;

namespace Player
{
    public class Scores
    {
        public int Score { get; private set; }
        public int CollectedBonuses { get; private set; }

        private readonly ScoresSettingsSO _settings;
        private int _killMultiplier = 1;
        
        public event Action OnScoreChanged;

        [Inject]
        public Scores(ScoresSettingsSO settings) => _settings = settings;

        public void AddMiniBonusScores()
        {
            Score = Mathf.Clamp(Score + _settings.MiniBonusScores, 0, int.MaxValue);
            OnScoreChanged?.Invoke();
        }

        public void AddCollectedBonus() => CollectedBonuses++;

        public void AddKillScores()
        {
            Score = Mathf.Clamp(Score + _settings.KillScores * _killMultiplier, 0, int.MaxValue);
            _killMultiplier++;
        }

        public void ClearKillMultiplier() => _killMultiplier = 1;
    }
}