using System;
using System.Collections.Generic;
using Level.Data;
using Player;
using UnityEngine;
using VContainer;

namespace Level
{
    public class BonusLoader : MonoBehaviour
    {
        [SerializeField] private FloorCell[] floorCell;

        private readonly List<Bonus> _spawnedBonuses = new();
        private Scores _scores;
        
        public int AllBonusesCount => _spawnedBonuses.Count;

        public event Action OnBigBonusCollected;

        [Inject]
        private void Construct(Scores scores) => _scores = scores;

        public void OnBonusCollected(BonusType bonusType)
        {
            switch (bonusType)
            {
                case BonusType.MINI_BONUS:
                    _scores.AddCollectedBonus();
                    _scores.AddMiniBonusScores();
                    break;
                case BonusType.BIG_BONUS:
                    _scores.AddCollectedBonus();
                    OnBigBonusCollected?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bonusType), bonusType, null);
            }
        }

        private void Awake()
        {
            foreach (var floor in floorCell)
            {
                floor.Init(this);
                if (!floor.CanSpawnBonus)
                    continue;

                var spawned = floor.SpawnBonus();
                if(spawned)
                    _spawnedBonuses.Add(spawned);
            }
        }
    }
}