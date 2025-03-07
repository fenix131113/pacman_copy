using System;
using UnityEngine;

namespace Utils
{
    public class TriggerHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask interactionLayer;
        
        public event Action<GameObject> OnEnterTrigger;
        public event Action<GameObject> OnExitTrigger;

        private void OnDestroy()
        {
            OnEnterTrigger = null;
            OnExitTrigger = null;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, interactionLayer))
                return;
            
            OnEnterTrigger?.Invoke(other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, interactionLayer))
                return;
            
            OnExitTrigger?.Invoke(other.gameObject);
        }
    }
}