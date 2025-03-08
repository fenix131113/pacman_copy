using Level.Data;
using UnityEngine;
using Utils;

namespace Level
{
    public class Bonus : MonoBehaviour
    {
        [SerializeField] private LayerMask interactLayer;
        [SerializeField] private BonusType bonusType;

        private BonusLoader _bonusLoader;

        public void Init(BonusLoader bonusLoader) => _bonusLoader = bonusLoader;

        private void OnTriggerEnter2D(Collider2D
            other)
        {
            if(!LayerService.CheckLayersEquality(other.gameObject.layer, interactLayer))
                return;
            
            _bonusLoader.OnBonusCollected(bonusType);
            gameObject.SetActive(false);
        }
    }
}