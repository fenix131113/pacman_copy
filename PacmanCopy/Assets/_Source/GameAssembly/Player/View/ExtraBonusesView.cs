using Level;
using UnityEngine;
using VContainer;

namespace Player.View
{
    public class ExtraBonusesView : MonoBehaviour
    {
        [SerializeField] private GameObject[] extraBonusesImages;

        private BonusLoader _bonusLoader;
        
        [Inject]
        private void Construct(BonusLoader bonusLoader) => _bonusLoader = bonusLoader;

        private void Start() => _bonusLoader.OnExtraBonusCollected += OnExtraBonusCollected;

        private void OnDestroy() => _bonusLoader.OnExtraBonusCollected -= OnExtraBonusCollected;

        private void OnExtraBonusCollected() => extraBonusesImages[_bonusLoader.SpawnedExtraBonusCount - 1].gameObject.SetActive(true);
    }
}