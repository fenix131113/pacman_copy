using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace Level.View
{
    public class EndGameView : MonoBehaviour
    {
        [SerializeField] private GameObject menu;
        [SerializeField] private TMP_Text resultLabel;
        [SerializeField] private string winMessage;
        [SerializeField] private string looseMessage;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;

        private PlayerHealth _playerHealth;
        private BonusLoader _bonusLoader;
        private Scores _scores;

        [Inject]
        private void Construct(PlayerHealth playerHealth, BonusLoader bonusLoader, Scores scores)
        {
            _playerHealth = playerHealth;
            _bonusLoader = bonusLoader;
            _scores = scores;
        }

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void Win() => ActivateEndMenu(winMessage);

        private void Loose()
        {
            Time.timeScale = 0;
            ActivateEndMenu(looseMessage);
        }

        private void CheckLooseConditions()
        {
            if (_playerHealth.Health == 0)
                Loose();
        }
        
        private void CheckWinConditions()
        {
            if (_scores.CollectedBonuses == _bonusLoader.AllBonusesCount)
                Win();
        }

        private void ActivateEndMenu(string message)
        {
            resultLabel.text = message;
            menu.SetActive(true);
        }

        private static void Restart()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private static void LoadMainMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }

        private void Bind()
        {
            restartButton.onClick.AddListener(Restart);
            mainMenuButton.onClick.AddListener(LoadMainMenu);
            _playerHealth.OnHealthChanged += CheckLooseConditions;
            _scores.OnScoreChanged += CheckWinConditions;
        }

        private void Expose()
        {
            restartButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.RemoveAllListeners();
            _playerHealth.OnHealthChanged -= CheckLooseConditions;
            _scores.OnScoreChanged -= CheckWinConditions;
        }
    }
}