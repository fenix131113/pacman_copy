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
        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioClip defaultMusicTheme;
        [SerializeField] private AudioClip playerAttackMusic;

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
            musicAudioSource.clip = playerAttackMusic;
            musicAudioSource.Play();
            
            foreach (var enemy in enemies.Where(enemy => enemy.CurrentState != EnemyState.DEAD))
                enemy.SetEnemyState(EnemyState.ESCAPE);

            StartCoroutine(EffectTimerCoroutine());
        }

        private void StopEffect()
        {
            musicAudioSource.clip = defaultMusicTheme;
            musicAudioSource.Play();
            
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