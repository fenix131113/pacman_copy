using System;
using System.Collections;
using System.Collections.Generic;
using Level.Data;
using Player;
using Unity.Mathematics;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Level
{
    public class BonusLoader : MonoBehaviour
    {
        [SerializeField] private AudioSource miniBonusSource;
        [SerializeField] private AudioSource bigBonusSource;
        [SerializeField] private FloorCell[] floorCell;
        [SerializeField] private List<FloorCell> extraBonusAllowCells;
        [SerializeField] private Bonus extraBonus;
        [SerializeField] private float minTimeToExtraBonus;
        [SerializeField] private float maxTimeToExtraBonus;
        
        public int SpawnedExtraBonusCount { get; private set; }

        private readonly List<Bonus> _spawnedBonuses = new();
        private Scores _scores;

        private const int MAX_EXTRA_BONUS_COUNT = 4;

        public int AllBonusesCount => _spawnedBonuses.Count;

        public event Action OnBigBonusCollected;
        public event Action OnExtraBonusCollected;

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
                case BonusType.EXTRA_BONUS:
                    _scores.AddExtraBonusScores();
                    OnExtraBonusCollected?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bonusType), bonusType, null);
            }
        }

        private void Awake()
        {
            foreach (var floor in floorCell)
            {
                floor.Init(this,
                    floor.BonusPrefab.BonusType == BonusType.MINI_BONUS ? miniBonusSource : bigBonusSource);

                if (!floor.CanSpawnBonus)
                    continue;

                var spawned = floor.SpawnBonus();
                if (spawned)
                    _spawnedBonuses.Add(spawned);
            }
        }

        private void Start() => StartCoroutine(ExtraBonusesCoroutine());

        private IEnumerator ExtraBonusesCoroutine()
        {
            while (SpawnedExtraBonusCount < MAX_EXTRA_BONUS_COUNT)
            {
                yield return new WaitForSeconds(Random.Range(minTimeToExtraBonus, maxTimeToExtraBonus));

                SpawnedExtraBonusCount++;
                var selectedCell = extraBonusAllowCells[Random.Range(0, extraBonusAllowCells.Count)];
                Instantiate(extraBonus, selectedCell.transform.position, quaternion.identity).Init(this, bigBonusSource);
                extraBonusAllowCells.Remove(selectedCell);
            }
        }
    }
}