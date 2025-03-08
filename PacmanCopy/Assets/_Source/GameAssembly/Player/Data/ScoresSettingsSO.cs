using UnityEngine;

namespace Player.Data
{
    [CreateAssetMenu(fileName = "ScoresSettings", menuName = "Config/ScoresSettings")]
    public class ScoresSettingsSO : ScriptableObject
    {
        [field: SerializeField] public int MiniBonusScores { get; private set; }
        [field: SerializeField] public int KillScores { get; private set; }
    }
}