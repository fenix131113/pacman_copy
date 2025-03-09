using Level.Data;
using UnityEngine;
using Utils;

namespace Level
{
    public class Bonus : MonoBehaviour
    {
        [SerializeField] private LayerMask interactLayer;
        [field: SerializeField] public BonusType BonusType { get; private set; }

        private AudioSource _bonusSound;
        private BonusLoader _bonusLoader;

        public void Init(BonusLoader bonusLoader, AudioSource bonusSound)
        {
            _bonusLoader = bonusLoader;
            _bonusSound = bonusSound;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, interactLayer))
                return;

            _bonusSound.PlayOneShot(_bonusSound.clip);
            _bonusLoader.OnBonusCollected(BonusType);
            gameObject.SetActive(false);
        }
    }
}