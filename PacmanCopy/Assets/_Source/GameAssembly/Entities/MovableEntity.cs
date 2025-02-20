using Core;
using UnityEngine;

namespace Entities
{
    public class MovableEntity : MonoBehaviour
    {
        [SerializeField] private LayerMask obstaclesLayer;
        [SerializeField] private float speed;

        private MoveDirection _currentDirection;
        private bool _canMove;
        private Ray _obstaclesRay;
        private Ray _rotationFirstRay;
        private Ray _rotationSecondRay;

        private const float MAX_ROTATION_OFFSET = 0.025f;

        private void Start() => Rotate(_currentDirection);

        public void Move()
        {
            if (!_canMove)
                return;

            CheckForObstacles();
            transform.position += transform.right * (speed * Time.deltaTime);
        }

        public void Rotate(MoveDirection direction)
        {
            var angleInDegrees = (Vector3.forward * MoveDirectionUtils.GetDirectionDegrees(direction)).z;
            var zDirection = new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad),
                Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);

            _rotationFirstRay = new Ray(transform.position, zDirection);

            var xClamp = Mathf.RoundToInt(transform.localPosition.x);
            var yClamp = Mathf.RoundToInt(transform.localPosition.y);


            if (Physics2D.Raycast(_rotationFirstRay.origin, _rotationFirstRay.direction,
                    GameConstants.UNIT_SIZE / 2 + MAX_ROTATION_OFFSET * 1.5f, obstaclesLayer)
                || Vector3.Distance(new Vector2(xClamp, yClamp), transform.localPosition) > MAX_ROTATION_OFFSET)
                return;

            transform.rotation =
                Quaternion.Euler(Vector3.forward * MoveDirectionUtils.GetDirectionDegrees(_currentDirection));
            _currentDirection = direction;

            CheckForObstacles();
        }

        private void CheckForObstacles()
        {
            _obstaclesRay = new Ray(transform.position, transform.right);

            if (Physics2D.Raycast(_obstaclesRay.origin, _obstaclesRay.direction, GameConstants.UNIT_SIZE / 2,
                    obstaclesLayer))
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
            Gizmos.DrawRay(_obstaclesRay.origin, _obstaclesRay.direction * (GameConstants.UNIT_SIZE / 2));
        }
    }
}