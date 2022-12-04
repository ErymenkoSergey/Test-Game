using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Morkwa.Mechanics.CommonBehaviours;
using Morkwa.Interface;

namespace Morkwa.UI
{
    public class UIGame : CommonBehaviour, IUIProcessing
    {
        [SerializeField] private GameObject _game;
        private IGame _iGame;

        [Space]
        [SerializeField] private string _winText = "You Win";
        [SerializeField] private string _looseText = "You Loose ";

        [Space]
        [SerializeField] private Slider _noiseSlider;
        [SerializeField] private GameObject _startPanel;
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private Button _iReadyButton;
        [SerializeField] private Button _replayGameButton;
        [SerializeField] private Button _menuGameButton;
        [SerializeField] private TextMeshProUGUI _textGameOver;

        private void OnEnable()
        {
            _iReadyButton.onClick.AddListener(PlayGame);
            _replayGameButton.onClick.AddListener(ReplayGame);
            _menuGameButton.onClick.AddListener(Menu);
        }

        private void OnDisable()
        {
            _iReadyButton.onClick.RemoveListener(PlayGame);
            _replayGameButton.onClick.RemoveListener(ReplayGame);
            _menuGameButton.onClick.RemoveListener(Menu);
        }

        private void Start()
        {
            SetGame();
            DefauleState();
        }

        private void SetGame()
        {
            if (_game.TryGetComponent(out IGame game))
                _iGame = game;
        }

        private void DefauleState()
        {
            _startPanel.SetActive(true);
        }

        public void SetNoiseDetected(float value)
        {
            _noiseSlider.maxValue = value;
        }

        public void SetValueNoiseSlider(float value)
        {
            _noiseSlider.value = value;
        }

        public void GameOver(bool isWin)
        {
            OpenPausePanel(true);

            if (isWin)
            {
                _textGameOver.text = _winText;
            }
            else
            {
                _textGameOver.text = _looseText;
            }
        }

        public void OpenPausePanel(bool isOpen)
        {
            _pausePanel.SetActive(isOpen);
        }

        private void PlayGame()
        {
            _startPanel.SetActive(false);
            _iGame.StartGame();
        }

        private void ReplayGame()
        {
            SceneManager.LoadScene("Game");
        }

        private void Menu()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}