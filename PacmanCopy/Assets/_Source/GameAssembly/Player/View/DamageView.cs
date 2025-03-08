using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;
using Utils;
using VContainer;

namespace Player.View
{
    public class DamageView : MonoBehaviour
    {
        [SerializeField] private AlphaBlinking playerBlinking;
        [SerializeField] private float effectDuration;
        [SerializeField] private List<MovableEntity> movablesToReturn;

        private PlayerHealth _health;

        [Inject]
        private void Construct(PlayerHealth health) => _health = health;

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void DamageEffect()
        {
            playerBlinking.StartBlinking();
            
            if (_health.Health != 0)
                Time.timeScale = 0;
            
            StartCoroutine(EffectCoroutine());
        }

        private void Bind() => _health.OnHealthChanged += DamageEffect;

        private void Expose() => _health.OnHealthChanged -= DamageEffect;

        private IEnumerator EffectCoroutine()
        {
            yield return new WaitForSecondsRealtime(effectDuration);

            foreach (var movable in movablesToReturn)
                movable.ReturnToStart();

            if (_health.Health != 0)
                Time.timeScale = 1;

            playerBlinking.StopBlinking();
        }
    }
}