using System;
using System.Collections.Generic;
using Core;
using Entities.Data;
using Level;
using UnityEngine;
using VContainer;

namespace Entities
{
    public class MovableEntity : MonoBehaviour
    {
        [field: SerializeField] public LayerMask ObstaclesLayer { get; private set; }
        [SerializeField] private float speed;

        public MoveDirection CurrentDirection { get; private set; }
        public FloorCell CurrentCell { get; private set; }

        public MovementControlType MovementControlType { get; private set; }
        private EntityPath _currentPath;
        private bool _canMove;
        private Ray _obstaclesRay;
        private Ray _rotationRay;
        private Map _map;

        public event Action OnPathEnded;
        public event Action OnNextNodeReached;

        public const float MAX_ROTATION_OFFSET = 0.1f;

        [Inject]
        private void Construct(Map map) => _map = map;

        private void Awake() => Rotate(CurrentDirection);

        private void Start() => CheckCurrentCell();

        private void OnDestroy()
        {
            OnPathEnded = null;
            OnNextNodeReached = null;
        }

        private void Update()
        {
            if (MovementControlType == MovementControlType.PATH)
                Move();
        }

        #region Movement

        private bool Move()
        {
            CheckCurrentCell();
            CheckForObstacles();

            if (!_canMove)
                return false;

            transform.position += transform.right * (speed * Time.deltaTime);

            if (_currentPath == null)
                return true;

            if (!_currentPath.IsTargetPathValid)
            {
                _currentPath = null;
                MovementControlType = MovementControlType.CUSTOM;
                OnPathEnded?.Invoke();
            }
            else if (MovementControlType == MovementControlType.PATH &&
                     _currentPath.TargetPath[0].Cell == CurrentCell && CanRotate(_currentPath.TargetPath[0].Direction))
            {
                OnNextNodeReached?.Invoke();
                Rotate(_currentPath.TargetPath[0].Direction);
                _currentPath.TargetPath.RemoveAt(0);
            }

            return true;
        }

        public bool CustomMoveEntity()
        {
            return MovementControlType == MovementControlType.CUSTOM && Move();
        }

        public void StartPathMove(EntityPath path, bool resetPos = true)
        {
            if (!path.IsTargetPathValid || (path.IsTargetPathValid && path.TargetPath[0].Cell != CurrentCell))
                return;

            if (resetPos)
            {
                var xClamp = Mathf.RoundToInt(transform.localPosition.x);
                var yClamp = Mathf.RoundToInt(transform.localPosition.y);

                transform.localPosition = new Vector3(xClamp, yClamp, transform.localPosition.z);
            }

            _currentPath = path;
            Rotate(_currentPath.TargetPath[0].Direction);
            MovementControlType = MovementControlType.PATH;
        }

        public void Rotate(MoveDirection direction)
        {
            if (!CanRotate(direction))
                return;

            if (CurrentDirection != direction)
            {
                var xClamp = Mathf.RoundToInt(transform.localPosition.x);
                var yClamp = Mathf.RoundToInt(transform.localPosition.y);

                transform.localPosition = new Vector3(xClamp, yClamp, transform.localPosition.z);
            }

            CurrentDirection = direction;

            transform.rotation =
                Quaternion.Euler(Vector3.forward * MoveDirectionUtils.GetDirectionDegrees(CurrentDirection));

            CheckForObstacles();
        }

        #endregion

        #region Utils

        public bool CanRotate(MoveDirection direction)
        {
            var angleInDegrees = (Vector3.forward * MoveDirectionUtils.GetDirectionDegrees(direction)).z;
            var zDirection = new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad),
                Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);

            _rotationRay = new Ray(transform.position, zDirection);

            var xClamp = Mathf.RoundToInt(transform.localPosition.x);
            var yClamp = Mathf.RoundToInt(transform.localPosition.y);

            return !Physics2D.Raycast(_rotationRay.origin, _rotationRay.direction,
                       GameConstants.UNIT_SIZE / 2 + MAX_ROTATION_OFFSET * 1.5f, ObstaclesLayer)
                   && !(Vector3.Distance(new Vector2(xClamp, yClamp), transform.localPosition) > MAX_ROTATION_OFFSET);
        }

        public void NativeSetMovementType(MovementControlType movementType)
        {
            if (movementType == MovementControlType.PATH)
            {
                Debug.LogWarning("Native moving type \"PATH\" is not supported");
                return;
            }
            
            _currentPath = null;

            MovementControlType = movementType;
        }

        public void CheckCurrentCell()
        {
            CurrentCell =
                _map.GetCell(new Vector2Int(Mathf.RoundToInt(transform.localPosition.x),
                    Mathf.RoundToInt(transform.localPosition.y)));
        }

        private void CheckForObstacles()
        {
            _obstaclesRay = new Ray(transform.position, transform.right);

            if (Physics2D.Raycast(_obstaclesRay.origin, _obstaclesRay.direction,
                    GameConstants.UNIT_SIZE / 2 + 0.001f,
                    ObstaclesLayer))
            {
                _canMove = false;
                return;
            }

            _canMove = true;
        }

        #endregion

        #region Editor

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_rotationRay.origin,
                _rotationRay.direction * (GameConstants.UNIT_SIZE / 2 + MAX_ROTATION_OFFSET));
            Gizmos.DrawRay(_obstaclesRay.origin, _obstaclesRay.direction * (GameConstants.UNIT_SIZE / 2 + 0.001f));
        }

        private void OnDrawGizmosSelected()
        {
            if (_currentPath is not { IsTargetPathValid: true })
                return;

            Gizmos.color = Color.cyan;
            foreach (var pathGroup in _currentPath.TargetPath)
            {
                Gizmos.DrawSphere(pathGroup.Cell.transform.position, 0.1f);
                Gizmos.DrawLine(pathGroup.Cell.transform.position, pathGroup.Direction switch
                {
                    MoveDirection.UP => pathGroup.Cell.transform.position + Vector3.up,
                    MoveDirection.RIGHT => pathGroup.Cell.transform.position + Vector3.right,
                    MoveDirection.DOWN => pathGroup.Cell.transform.position + Vector3.down,
                    MoveDirection.LEFT => pathGroup.Cell.transform.position + Vector3.left,
                    _ => throw new ArgumentOutOfRangeException()
                });
            }
        }

        #endregion
    }
}