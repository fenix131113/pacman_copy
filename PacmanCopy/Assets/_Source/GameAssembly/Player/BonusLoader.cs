using Level;
using UnityEngine;
using VContainer;

namespace Player
{
    public class BonusLoader : MonoBehaviour
    {
        [SerializeField] private FloorCell[] floorCell;

        private Scores _scores;
        
        [Inject]
        private void Construct(Scores scores) => _scores = scores;

        private void Awake()
        {
            foreach (var bonus in floorCell)
                bonus.Init(_scores);
        }
    }
}