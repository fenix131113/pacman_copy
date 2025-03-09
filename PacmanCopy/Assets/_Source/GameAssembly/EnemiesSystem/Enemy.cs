using System;
using System.Collections;
using System.Collections.Generic;
using EnemiesSystem.Data;
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
        [SerializeField] private SpriteRenderer enemyRenderer;
        [SerializeField] private float recoverTime;

        protected readonly MoveDirection[] MoveDirections =
            { MoveDirection.UP, MoveDirection.RIGHT, MoveDirection.DOWN, MoveDirection.LEFT };

        public EnemyState CurrentState { get; protected set; }

        protected bool IsRotateCooldown;
        protected PlayerHealth PlayerHealth;
        protected Scores _scores;
        protected Map Map;
        protected Color StartColor;
        protected Vector2Int StartPosition;

        [Inject]
        private void Construct(PlayerHealth playerHealth, Map map, Scores scores)
        {
            PlayerHealth = playerHealth;
            Map = map;
            _scores = scores;

            entity.OnPathEnded += OnPathEnded;
            entity.OnTeleported += OnTeleported;

            StartColor = enemyRenderer.color;
            StartPosition = new Vector2Int(Mathf.RoundToInt(entity.transform.position.x), Mathf.RoundToInt(entity.transform.position.y));
        }

        public void SetEnemyState(EnemyState state)
        {
            CurrentState = state;

            switch (state)
            {
                case EnemyState.ATTACK:
                    enemyRenderer.color = StartColor;
                    break;
                case EnemyState.ESCAPE:
                    enemyRenderer.color = Color.blue;
                    break;
                case EnemyState.DEAD:
                    enemyRenderer.color = Color.gray;
                    StartPath(Map.GetCell(StartPosition));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
            
            OnEnemyStateChanged();
        }

        protected virtual void OnTeleported(bool resetEntity)
        {
        }

        protected virtual void OnEnemyStateChanged()
        {
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

            if (CurrentState == EnemyState.ESCAPE)
            {
                _scores.AddKillScores();
                SetEnemyState(EnemyState.DEAD);
                return;
            }
            
            if(CurrentState != EnemyState.ATTACK)
                return;
            
            PlayerHealth.TakeDamage();
        }

        protected IEnumerator BaseRecoverCoroutine()
        {
            yield return new WaitForSeconds(recoverTime);
            
            SetEnemyState(EnemyState.ATTACK);
        }
    }
}