using System.Collections.Generic;
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

        [Inject]
        private void Construct(Scores scores) => _scores = scores;

        private void Awake()
        {
            foreach (var floor in floorCell)
            {
                floor.Init(_scores);
                if (!floor.CanSpawnBonus)
                    continue;

                var spawned = floor.SpawnBonus();
                if(spawned)
                    _spawnedBonuses.Add(spawned);
            }
        }
    }
}