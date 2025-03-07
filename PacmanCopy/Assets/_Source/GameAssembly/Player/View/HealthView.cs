using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Player.View
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private Image[] healthImages;

        private PlayerHealth _playerHealth;

        [Inject]
        private void Construct(PlayerHealth playerHealth) => _playerHealth = playerHealth;

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void OnHealthChanged()
        {
            for (var i = 0; i < _playerHealth.MaxHealth; i++)
                healthImages[i].gameObject.SetActive(false);
            
            for (var i = 0; i < _playerHealth.Health; i++)
                healthImages[i].gameObject.SetActive(true);
        }

        private void Bind() => _playerHealth.OnHealthChanged += OnHealthChanged;

        private void Expose() => _playerHealth.OnHealthChanged -= OnHealthChanged;
    }
}