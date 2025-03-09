using System;
using System.Collections.Generic;
using Entities;
using Player;
using UnityEngine;

namespace Level
{
    public class FloorCell : MonoBehaviour
    {
        [field: SerializeField] public bool CanSpawnBonus { get; private set; } = true;

        [field: SerializeField] public Bonus BonusPrefab { get; private set; }
        [SerializeField] private LayerMask floorLayer;

        [field: SerializeField] public FloorCell UpCell { get; private set; }
        [field: SerializeField] public FloorCell RightCell { get; private set; }
        [field: SerializeField] public FloorCell DownCell { get; private set; }
        [field: SerializeField] public FloorCell LeftCell { get; private set; }

        private AudioSource _bonusSound;
        private BonusLoader _bonusLoader;

        public void Init(BonusLoader bonusLoader, AudioSource bonusSound, Bonus bonusPrefabOverride = null)
        {
            _bonusLoader = bonusLoader;
            _bonusSound = bonusSound;
            
            if(bonusPrefabOverride)
                BonusPrefab = bonusPrefabOverride;
        }

        private void Awake()
        {
            LoadNeighbours();
        }

        public List<FloorCell> GetNeighboursList()
        {
            var neighbours = new List<FloorCell>();
            
            if(UpCell)
                neighbours.Add(UpCell);
            
            if(RightCell)
                neighbours.Add(RightCell);

            if (DownCell)
                neighbours.Add(DownCell);
            
            if(LeftCell)
                neighbours.Add(LeftCell);
            
            return neighbours;
        }

        private void LoadNeighbours()
        {
            UpCell ??= Physics2D
                .OverlapPoint(
                    new Vector3(transform.position.x, transform.position.y + transform.lossyScale.y,
                        transform.position.z), floorLayer)?.GetComponent<FloorCell>();
            RightCell ??= Physics2D
                .OverlapPoint(
                    new Vector3(transform.position.x + transform.lossyScale.x, transform.position.y,
                        transform.position.z), floorLayer)?.GetComponent<FloorCell>();
            DownCell ??= Physics2D
                .OverlapPoint(
                    new Vector3(transform.position.x, transform.position.y - transform.lossyScale.y,
                        transform.position.z), floorLayer)?.GetComponent<FloorCell>();
            LeftCell ??= Physics2D
                .OverlapPoint(
                    new Vector3(transform.position.x - transform.lossyScale.x, transform.position.y,
                        transform.position.z), floorLayer)?.GetComponent<FloorCell>();
        }

        public Bonus SpawnBonus()
        {
            if (!CanSpawnBonus)
                return null;

            var spawned = Instantiate(BonusPrefab, transform.position, Quaternion.identity);
            spawned.Init(_bonusLoader, _bonusSound);
            return spawned;
        }
        
        public MoveDirection GetDirectionBySecondCells(FloorCell to)
        {
            return to switch
            {
                _ when to == UpCell => MoveDirection.UP, 
                _ when to == RightCell => MoveDirection.RIGHT, 
                _ when to == DownCell => MoveDirection.DOWN, 
                _ when to == LeftCell => MoveDirection.LEFT, 
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}