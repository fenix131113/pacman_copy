using TMPro;
using UnityEngine;
using VContainer;

namespace Player.View
{
    public class ScoresView : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoresLabel;

        private Scores _scores;

        [Inject]
        private void Construct(Scores scores) => _scores = scores;

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void Redraw()
        {
            scoresLabel.text = _scores.Score.ToString();
        }

        private void Bind() => _scores.OnScoreChanged += Redraw;

        private void Expose() => _scores.OnScoreChanged -= Redraw;
    }
}