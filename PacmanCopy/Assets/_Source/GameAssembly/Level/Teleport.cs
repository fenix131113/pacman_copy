using Entities;
using UnityEngine;
using Utils;

namespace Level
{
    public class Teleport : MonoBehaviour
    {
        [SerializeField] private FloorCell teleportDestination;
        [SerializeField] private LayerMask interactionLayer;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, interactionLayer) ||
                !other.gameObject.TryGetComponent(out MovableEntity entity))
                return;
            
            entity.Teleport(teleportDestination, false);
        }
    }
}