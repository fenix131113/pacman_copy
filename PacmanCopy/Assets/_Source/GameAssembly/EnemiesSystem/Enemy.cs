using System.Collections.Generic;
using Entities;
using Level;
using Player;
using UnityEngine;
using Utils;
using VContainer;

namespace EnemiesSystem
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] protected MovableEntity entity;
        [SerializeField] protected LayerMask interactableLayer;

        protected readonly MoveDirection[] MoveDirections =
            { MoveDirection.UP, MoveDirection.RIGHT, MoveDirection.DOWN, MoveDirection.LEFT };

        protected bool IsRotateCooldown;
        protected PlayerHealth PlayerHealth;
        protected Map Map;

        [Inject]
        private void Construct(PlayerHealth playerHealth, Map map)
        {
            PlayerHealth = playerHealth;
            Map = map;

            entity.OnPathEnded += OnPathEnded;
        }

        protected virtual void OnPathEnded()
        {
        }
        
        protected void StartPath(FloorCell targetCell, List<FloorCell> exceptCells = null, bool resetPos = true)
        {
            entity.CheckCurrentCell();
            entity.StartPathMove(Map.GetPathToCell(entity.CurrentCell, targetCell, exceptCells), resetPos);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, interactableLayer))
                return;

            PlayerHealth.TakeDamage();
        }
    }
}