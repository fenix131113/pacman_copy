using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.View
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private GameObject settingsPanel;

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void LoadGame() => SceneManager.LoadScene(sceneBuildIndex: 1);
        private void ToggleSettings() => settingsPanel.SetActive(!settingsPanel.activeSelf);

        private void Bind()
        {
            startGameButton.onClick.AddListener(LoadGame);
            settingsButton.onClick.AddListener(ToggleSettings);
        }

        private void Expose()
        {
            startGameButton.onClick.RemoveAllListeners();
            settingsButton.onClick.RemoveAllListeners();
        }
    }
}