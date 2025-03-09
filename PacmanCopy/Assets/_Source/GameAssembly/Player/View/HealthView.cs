using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Player.View
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private Image[] healthImages;
        [SerializeField] private float healthDecreaseAnimTime = 0.5f;
        [SerializeField] private AudioSource damageSoundSource;
        [SerializeField] private AudioSource musicSource;

        private const float HEALTH_BLINKING_INTERVAL = 0.1f;

        private PlayerHealth _playerHealth;

        [Inject]
        private void Construct(PlayerHealth playerHealth) => _playerHealth = playerHealth;

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void OnHealthChanged()
        {
            StartCoroutine(HealthDecreaseAnimCoroutine(healthImages.Where(img => img.gameObject.activeSelf).ToList()[^1]));
        }

        private void Bind() => _playerHealth.OnHealthChanged += OnHealthChanged;

        private void Expose() => _playerHealth.OnHealthChanged -= OnHealthChanged;

        private IEnumerator HealthDecreaseAnimCoroutine(Image target)
        {
            var elapsedTime = 0f;
            
            var musicTempVolume = musicSource.volume;
            musicSource.volume = 0f;
            damageSoundSource.PlayOneShot(damageSoundSource.clip);
            
            while (elapsedTime < healthDecreaseAnimTime)
            {
                yield return new WaitForSecondsRealtime(healthDecreaseAnimTime / (healthDecreaseAnimTime / HEALTH_BLINKING_INTERVAL));
                elapsedTime += HEALTH_BLINKING_INTERVAL;
                
                target.gameObject.SetActive(!target.gameObject.activeSelf);
            }

            musicSource.volume = musicTempVolume;
            target.gameObject.SetActive(false);
        }
    }
}