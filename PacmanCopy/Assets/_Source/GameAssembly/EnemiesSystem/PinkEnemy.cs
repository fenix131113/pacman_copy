using System.Collections;
using System.Linq;
using Entities;
using UnityEngine;

namespace EnemiesSystem
{
    public class PinkEnemy : Enemy
    {
        [SerializeField] private float rotateCooldown;
        
        private void Update()
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
        
        private IEnumerator RotateCooldownCoroutine()
        {
            IsRotateCooldown = true;

            yield return new WaitForSeconds(rotateCooldown);

            IsRotateCooldown = false;
        }
    }
}