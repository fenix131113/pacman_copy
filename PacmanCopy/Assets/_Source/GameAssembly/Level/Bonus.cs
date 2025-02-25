using Player;
using UnityEngine;
using Utils;
using VContainer;

namespace Level
{
    public class Bonus : MonoBehaviour
    {
        [SerializeField] private int score;
        [SerializeField] private LayerMask interactLayer;

        private Scores _scores;

        [Inject]
        private void Construct(Scores scores) => _scores = scores;

        public void Init(Scores scores) => _scores = scores;

        private void OnTriggerEnter2D(Collider2D
            other)
        {
            if(!LayerService.CheckLayersEquality(other.gameObject.layer, interactLayer))
                return;
            
            _scores.AddScore(score);
            gameObject.SetActive(false);
        }
    }
}