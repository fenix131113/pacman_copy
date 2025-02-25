using Player;
using UnityEngine;

namespace Level
{
    public class FloorCell : MonoBehaviour
    {
        [SerializeField] private bool spawnBonus = true;
        [SerializeField] private Bonus bonusPrefab;

        private Scores _scores;
        
        public void Init(Scores scores) => _scores = scores;

        private void Start()
        {
            if(spawnBonus)
                Instantiate(bonusPrefab, transform.position, Quaternion.identity).Init(_scores);
        }
    }
}