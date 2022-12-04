using Morkwa.Interface;
using Morkwa.MainData;
using Morkwa.Mechanics.CommonConfigBehaviours;
using UnityEngine;

namespace Morkwa.Mechanics
{
    public sealed class Game : CommonConfigBehaviour, IGame
    {
        [SerializeField] private GameObject _spawner;
        private ISpawning _iSpawning;
        [SerializeField] private GameObject _inputControl;
        private IControlable _iControlable;
        [SerializeField] private GameObject _audio;
        private IAudio _iAudio;
        [SerializeField] private GameObject _uIManager;
        private IUIProcessing _iUiProcessing;
        [SerializeField] private GameObject _cameraControl;
        private ICameraControl _iCameraControl;

        private bool _isPlayGame;
        public IPlayer IPlayer { get; private set; }

        private void Start()
        {
            SetInterfaces();

            _iSpawning.SetSpawningConfiguration(this, MainConfigurator.SettingGame.GameFieldConfig);
            SetStatusActiveGame(true);
            SetTimeScale(false);
        }

        private void SetInterfaces()
        {
            if (_spawner.TryGetComponent(out ISpawning spawning))
                _iSpawning = spawning;
            if (_inputControl.TryGetComponent(out IControlable controlable))
                _iControlable = controlable;
            if (_uIManager.TryGetComponent(out IUIProcessing uI))
                _iUiProcessing = uI;
            if (_audio.TryGetComponent(out IAudio iAudio))
                _iAudio = iAudio;
            if (_cameraControl.TryGetComponent(out ICameraControl iCameraControl))
                _iCameraControl = iCameraControl;
        }

        public void StartGame()
        {
            SetTimeScale(true);
        }

        private void SetStatusActiveGame(bool isOn)
        {
            _isPlayGame = isOn;
        }

        public void SetPlayer(IPlayer player)
        {
            IPlayer = player;
            _iControlable.SetPlayer(player.GetMoveble());
            _iCameraControl.SetPlayerTransform(IPlayer.GetTransform());
        }

        public EnemyCharacterConfig GetEnemyConfig()
        {
            return MainConfigurator.SettingGame.CharacterConfig;
        }

        public ISpawning GetSpawner()
        {
            return _iSpawning;
        }

        public Transform GetObjectAreFollowing()
        {
            return IPlayer.GetTransform();
        }

        public bool GetPlayGameStatus()
        {
            return _isPlayGame;
        }

        public IAudio GetAudioManager()
        {
            return _iAudio.GetAudioManager();
        }

        public void GameOver(bool isWin)
        {
            SetStatusActiveGame(false);

            _iUiProcessing.GameOver(isWin);

            if (isWin)
            {
                _iAudio.PlaySoundFinish();
            }
            else
            {
                _iAudio.PlaySoundGameOver();
            }

            SetTimeScale(false);
        }

        private void SetTimeScale(bool isPlay)
        {
            if (isPlay)
                Time.timeScale = 1f;
            else
                Time.timeScale = 0f;
        }
    }
}