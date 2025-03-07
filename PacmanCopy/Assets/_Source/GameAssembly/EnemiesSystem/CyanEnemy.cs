using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Data;
using Level;
using UnityEngine;
using Utils;

namespace EnemiesSystem
{
    public class CyanEnemy : Enemy
    {
        [SerializeField] private MovableEntity playerMovable;
        [SerializeField] private TriggerHandler escapeTrigger;
        [SerializeField] private LayerMask floorLayer;
        [SerializeField] private float rotateCooldown;
        [SerializeField] private float escapeMinRadius;
        [SerializeField] [Range(0, 1)] private float returnChance = 0.25f; // Check return chance every next path node

        private bool _returnedToPlayer;

        private void Start()
        {
            escapeTrigger.OnEnterTrigger += OnEscapeTriggerEnter;
            escapeTrigger.OnExitTrigger += OnEscapeTriggerExit;
            entity.OnNextNodeReached += CheckReturnChance;
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
            StartPath(GetEscapeCell(), new List<FloorCell> { playerMovable.CurrentCell });
            _returnedToPlayer = false;
        }

        private void CheckReturnChance()
        {
            if (_returnedToPlayer)
                return;

            var rnd = Random.Range(0f, 1f);

            if (rnd > returnChance)
                return;

            _returnedToPlayer = true;
            StartPath(playerMovable.CurrentCell);
        }

        private void OnEscapeTriggerEnter(GameObject obj)
        {
            if (entity.MovementControlType == MovementControlType.PATH)
                return;

            _returnedToPlayer = false;
            StartPath(GetEscapeCell(), new List<FloorCell> { playerMovable.CurrentCell });
        }

        private void OnEscapeTriggerExit(GameObject obj) => entity.NativeSetMovementType(MovementControlType.CUSTOM);

        private FloorCell GetEscapeCell()
        {
            var cells = Physics2D.OverlapCircleAll(transform.position, escapeMinRadius, floorLayer)
                .Select(f => f.GetComponent<FloorCell>())
                .Where(cell => cell)
                .ToList();

            var exceptedCells = Map.MapCells.Except(cells).ToList();

            return exceptedCells.Count > 0 ? exceptedCells[Random.Range(0, exceptedCells.Count)] : null;
        }

        private IEnumerator RotateCooldownCoroutine()
        {
            IsRotateCooldown = true;

            yield return new WaitForSeconds(rotateCooldown);

            IsRotateCooldown = false;
        }
    }
}