using Level;
using UnityEngine;
using VContainer;

namespace Player.View
{
    public class ExtraBonusesView : MonoBehaviour
    {
        [SerializeField] private GameObject[] extraBonusesImages;

        private BonusLoader _bonusLoader;
        private Scores _scores;
        
        [Inject]
        private void Construct(BonusLoader bonusLoader, Scores scores)
        {
            _bonusLoader = bonusLoader;
            _scores = scores;
        }

        private void Start() => _bonusLoader.OnExtraBonusCollected += OnExtraBonusCollected;

        private void OnDestroy() => _bonusLoader.OnExtraBonusCollected -= OnExtraBonusCollected;

        private void OnExtraBonusCollected() => extraBonusesImages[_scores.CollectedExtraBonuses - 1].gameObject.SetActive(true);
    }
}