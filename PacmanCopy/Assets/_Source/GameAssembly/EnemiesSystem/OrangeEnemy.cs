using System.Collections.Generic;
using System.Linq;
using EnemiesSystem.Data;
using Level;
using UnityEngine;

namespace EnemiesSystem
{
    public class OrangeEnemy : Enemy
    {
        [SerializeField] private List<FloorCell> allowTargets;
        
        private FloorCell _lastTarget;

        private void Start()
        {
            StartDefaultLogic();
        }

        private void StartDefaultLogic()
        {
            StartPath(GetMoveTarget());
        }

        protected override void OnEnemyStateChanged()
        {
            if(CurrentState == EnemyState.ATTACK)
                StartDefaultLogic();
        }

        protected override void OnPathEnded()
        {
            if (CurrentState == EnemyState.DEAD)
            {
                StartCoroutine(BaseRecoverCoroutine());
                return;
            }

            StartPath(GetMoveTarget());
        }
        
        protected override void OnTeleported()
        {
            SetEnemyState(EnemyState.ATTACK);
            StartDefaultLogic();
        }

        private FloorCell GetMoveTarget()
        {
            var allow = allowTargets.Except(new[] { _lastTarget }).ToList();
            var selected = allow[Random.Range(0, allow.Count)];
            _lastTarget = selected;
            return selected;
        }
    }
}