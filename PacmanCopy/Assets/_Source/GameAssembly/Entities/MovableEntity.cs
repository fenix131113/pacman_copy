using Core;
using UnityEngine;

namespace Entities
{
    public class MovableEntity : MonoBehaviour
    {
        [field: SerializeField] public LayerMask ObstaclesLayer { get; private set; }
        [SerializeField] private float speed;

        public MoveDirection CurrentDirection { get; private set; }

        private bool _canMove;
        private Ray _obstaclesRay;
        private Ray _rotationFirstRay;
        private Ray _rotationSecondRay;

        public const float MAX_ROTATION_OFFSET = 0.1f;

        private void Start() => Rotate(CurrentDirection);

        public bool Move()
        {
            CheckForObstacles();

            if (!_canMove)
                return false;

            transform.position += transform.right * (speed * Time.deltaTime);
            return true;
        }

        public void Rotate(MoveDirection direction)
        {
            if (!CanRotate(direction))
                return;

            CurrentDirection = direction;
            transform.rotation =
                Quaternion.Euler(Vector3.forward * MoveDirectionUtils.GetDirectionDegrees(CurrentDirection));

            CheckForObstacles();
        }

        public bool CanRotate(MoveDirection direction)
        {
            var angleInDegrees = (Vector3.forward * MoveDirectionUtils.GetDirectionDegrees(direction)).z;
            var zDirection = new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad),
                Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);

            _rotationFirstRay = new Ray(transform.position, zDirection);

            var xClamp = Mathf.RoundToInt(transform.localPosition.x);
            var yClamp = Mathf.RoundToInt(transform.localPosition.y);

            return !Physics2D.Raycast(_rotationFirstRay.origin, _rotationFirstRay.direction,
                       GameConstants.UNIT_SIZE / 2 + MAX_ROTATION_OFFSET * 1.5f, ObstaclesLayer)
                   && !(Vector3.Distance(new Vector2(xClamp, yClamp), transform.localPosition) > MAX_ROTATION_OFFSET);
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

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_rotationFirstRay.origin,
                _rotationFirstRay.direction * (GameConstants.UNIT_SIZE / 2 + MAX_ROTATION_OFFSET));
            Gizmos.DrawRay(_obstaclesRay.origin, _obstaclesRay.direction * (GameConstants.UNIT_SIZE / 2 + 0.001f));
        }
    }
}