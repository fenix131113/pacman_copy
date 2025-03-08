using System.Collections;
using System.Linq;
using EnemiesSystem.Data;
using Entities;
using Entities.Data;
using UnityEngine;

namespace EnemiesSystem
{
    public class PinkEnemy : Enemy
    {
        [SerializeField] private float rotateCooldown;

        private void Update()
        {
            if (CurrentState != EnemyState.DEAD)
                DefaultLogic();
        }

        private void DefaultLogic()
        {
            var result = entity.CustomMoveEntity();

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
        
        protected override void OnTeleported()
        {
            IsRotateCooldown = false;
            SetEnemyState(EnemyState.ATTACK);
            entity.NativeSetMovementType(MovementControlType.CUSTOM);
        }

        protected override void OnPathEnded()
        {
            if(CurrentState == EnemyState.DEAD)
                StartCoroutine(BaseRecoverCoroutine());
        }

        private IEnumerator RotateCooldownCoroutine()
        {
            IsRotateCooldown = true;

            yield return new WaitForSecondsRealtime(rotateCooldown);

            IsRotateCooldown = false;
        }
    }
}