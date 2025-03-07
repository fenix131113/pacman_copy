using System.Collections;
using System.Linq;
using Entities;
using Entities.Data;
using UnityEngine;
using Utils;

namespace EnemiesSystem
{
    public class RedEnemy : Enemy
    {
        [SerializeField] private MovableEntity playerMovable;
        [SerializeField] private TriggerHandler attackTrigger;
        [SerializeField] private float rotateCooldown;
        [SerializeField] [Range(0.1f, 5)] private float rebuildPathToPlayerCooldown;

        private bool _playerInRange;
        private Coroutine _rebuildPathCoroutine;

        private void Start()
        {
            attackTrigger.OnEnterTrigger += OnAttackTriggerEnter;
            attackTrigger.OnExitTrigger += OnAttackTriggerExit;
        }

        private void Update()
        {
            var result = entity.CustomMoveEntity();

            if (entity.MovementControlType == MovementControlType.PATH)
                return;

            if (IsRotateCooldown)
                return;

            var exceptDir = MoveDirectionUtils.InverseDirection(entity.CurrentDirection);

            var newDirections = MoveDirections.Except(new[] { exceptDir }).ToList();

            var toRotate = newDirections.Where(direction => entity.CanRotate(direction)).ToList();

            var rndRotate = Random.Range(0, toRotate.Count + 1);

            if (toRotate.Count > 0 && rndRotate != 0)
            {
                entity.Rotate(toRotate[rndRotate - 1]);
                StartCoroutine(RotateCooldownCoroutine());
                return;
            }

            if (result)
                return;

            var moveDirections = MoveDirections.Except(new[] { entity.CurrentDirection }).ToList();
            entity.Rotate(moveDirections[Random.Range(0, moveDirections.Count)]);
            StartCoroutine(RotateCooldownCoroutine());
        }

        protected override void OnPathEnded()
        {
            if (_playerInRange)
            {
                _playerInRange = false;
                
                if (_rebuildPathCoroutine != null)
                    StopCoroutine(_rebuildPathCoroutine);
            }

            entity.NativeSetMovementType(MovementControlType.CUSTOM);
        }

        private void OnAttackTriggerEnter(GameObject obj)
        {
            _playerInRange = true;
            if (entity.MovementControlType == MovementControlType.PATH)
                return;

            if (_rebuildPathCoroutine != null)
                StopCoroutine(_rebuildPathCoroutine);

            _rebuildPathCoroutine = StartCoroutine(RebuildPathToPlayerCooldownCoroutine());
            StartPath(playerMovable.CurrentCell);
        }

        private void OnAttackTriggerExit(GameObject obj)
        {
            _playerInRange = false;

            if (_rebuildPathCoroutine != null)
            {
                StopCoroutine(_rebuildPathCoroutine);
                _rebuildPathCoroutine = null;
            }

            entity.NativeSetMovementType(MovementControlType.CUSTOM);
        }

        private IEnumerator RotateCooldownCoroutine()
        {
            IsRotateCooldown = true;

            yield return new WaitForSeconds(rotateCooldown);

            IsRotateCooldown = false;
        }

        private IEnumerator RebuildPathToPlayerCooldownCoroutine()
        {
            while (_playerInRange)
            {
                yield return new WaitForSeconds(rebuildPathToPlayerCooldown);

                if (!_playerInRange)
                    yield break;

                StartPath(playerMovable.CurrentCell, null, false);
            }
        }
    }
}