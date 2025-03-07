using System.Collections.Generic;
using System.Linq;
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
            StartPath(GetMoveTarget());
        }

        protected override void OnPathEnded()
        {
            StartPath(GetMoveTarget());
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