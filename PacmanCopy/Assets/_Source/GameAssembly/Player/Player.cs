using Entities;
using UnityEngine;
using VContainer;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private MovableEntity entity;

        private IPlayerInput _playerInput;

        [Inject]
        private void Construct(IPlayerInput playerInput)
        {
            _playerInput = playerInput;
        }

        private void Update() => entity.Move();

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void CheckMoveRotation(Vector2 moveVector)
        {
            switch (moveVector.x)
            {
                case > 0:
                    entity.Rotate(MoveDirection.RIGHT);
                    break;
                case < 0:
                    entity.Rotate(MoveDirection.LEFT);
                    break;
                default:
                    switch (moveVector.y)
                    {
                        case > 0:
                            entity.Rotate(MoveDirection.UP);
                            break;
                        case < 0:
                            entity.Rotate(MoveDirection.DOWN);
                            break;
                    }

                    break;
            }
        }

        private void Bind()
        {
            _playerInput.OnMove += CheckMoveRotation;
        }

        private void Expose()
        {
            _playerInput.OnMove -= CheckMoveRotation;
        }
    }
}