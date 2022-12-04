using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Morkwa.Mechanics.CommonBehaviours;

public class UIMenu : CommonBehaviour
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _openSettingPanelButton;
    [SerializeField] private Button _clouseSettingPanelButton;
    [SerializeField] private Button _quitGameButton;

    [SerializeField] private GameObject _settingPanel;

    private void OnEnable()
    {
        _startGameButton.onClick.AddListener(StartGame);
        _openSettingPanelButton.onClick.AddListener(() => OpenSettingPanel(true));
        _clouseSettingPanelButton.onClick.AddListener(() => OpenSettingPanel(false));
        _quitGameButton.onClick.AddListener(QiutGame);
    }

    private void OnDisable()
    {
        _startGameButton.onClick.RemoveListener(StartGame);
        _openSettingPanelButton.onClick.RemoveListener(() => OpenSettingPanel(true));
        _clouseSettingPanelButton.onClick.RemoveListener(() => OpenSettingPanel(false));
        _quitGameButton.onClick.RemoveListener(QiutGame);
    }

    private void StartGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }

    private void OpenSettingPanel(bool isOpen)
    {
        _settingPanel.SetActive(isOpen);
    }

    private void QiutGame()
    {
        Application.Quit();
    }
}
