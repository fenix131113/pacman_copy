using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnemiesSystem;
using EnemiesSystem.Data;
using Player;
using UnityEngine;
using VContainer;

namespace Level
{
    public class PlayerAttackSwitcher : MonoBehaviour
    {
        [SerializeField] private List<Enemy> enemies;
        [SerializeField] private float effectDuration;

        private BonusLoader _bonusLoader;
        private Scores _scores;

        [Inject]
        private void Construct(BonusLoader bonusLoader, Scores scores)
        {
            _bonusLoader = bonusLoader;
            _scores = scores;
        }

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void StartEffect()
        {
            foreach (var enemy in enemies.Where(enemy => enemy.CurrentState != EnemyState.DEAD))
                enemy.SetEnemyState(EnemyState.ESCAPE);

            StartCoroutine(EffectTimerCoroutine());
        }

        private void StopEffect()
        {
            foreach (var enemy in enemies)
                enemy.SetEnemyState(EnemyState.ATTACK);

            _scores.ClearKillMultiplier();
        }

        private void Bind() => _bonusLoader.OnBigBonusCollected += StartEffect;

        private void Expose() => _bonusLoader.OnBigBonusCollected -= StartEffect;

        private IEnumerator EffectTimerCoroutine()
        {
            yield return new WaitForSeconds(effectDuration);

            StopEffect();
        }
    }
}