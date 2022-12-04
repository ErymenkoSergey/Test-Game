using Morkwa.Interface;
using Morkwa.Mechanics.CommonConfigBehaviours;
using UnityEngine;

namespace Morkwa.Mechanics.MainAudio
{
    public class Audio : CommonConfigBehaviour, IAudio
    {
        [SerializeField] private GameObject _uIGame;
        private IUIProcessing _iUIProcessing;

        [Space]
        [SerializeField] private AudioClip _alertSound;
        [SerializeField] private AudioClip _winSound;
        [SerializeField] private AudioClip _loseSound;

        [Space]
        [SerializeField] private AudioSource _soundSource;
        [SerializeField] private float _defaultVolume = 1f;

        private float _timeUpdate = 0.2f;
        private float _timeNoiseCoeficentAdd;
        private float _timeNoiseCoeficentRemove;
        private float _removeCoeficent = 10f;

        private float _currentNoise;
        private float _noiseDetection;
        private float _noisePerSecond;
        private float _noiseReductionLevel;
        private bool _isAlertPlay;

        private void Start()
        {
            SetUIProcessing();
        }

        private void SetUIProcessing()
        {
            if (_uIGame.TryGetComponent(out IUIProcessing processing))
            {
                _iUIProcessing = processing;
                SetAudioConfig();
            }
            else
                Debug.LogError($"Not Found UIGame {gameObject.name}");
        }

        private void SetAudioConfig()
        {
            var configurator = MainConfigurator.SettingGame.BalanceConfig;

            _noiseDetection = configurator.NoiseDetection;
            _noisePerSecond = configurator.AddingNoisePerSecond;
            _noiseReductionLevel = configurator.NoiseReductionInHalfSeconds;

            _timeNoiseCoeficentAdd = _noisePerSecond * _timeUpdate;
            _timeNoiseCoeficentRemove = (_noiseReductionLevel * _timeUpdate) / _removeCoeficent;

            _iUIProcessing.SetNoiseDetected(_noiseDetection);
        }

        public void AddNoiseValue()
        {
            if (_currentNoise <= _noiseDetection)
                _currentNoise += _timeNoiseCoeficentAdd;
        }

        private void FixedUpdate()
        {
            Noise();
        }

        private void Noise()
        {
            if (_currentNoise >= _noiseDetection)
            {
                PlaySoundAlert();
            }

            if (_currentNoise >= 0)
                _currentNoise -= _timeNoiseCoeficentRemove;

            _iUIProcessing.SetValueNoiseSlider(_currentNoise);
        }

        public void PlaySoundAlert()
        {
            if (!_isAlertPlay)
            {
                PlaySound(_alertSound);
                _isAlertPlay = true;
            }
        }

        public void SetDefaultState()
        {
            _isAlertPlay = false;
        }

        public IAudio GetAudioManager()
        {
            return this;
        }

        public void PlaySoundFinish()
        {
            PlaySound(_winSound);
        }

        public void PlaySoundGameOver()
        {
            PlaySound(_loseSound);
        }

        private void PlaySound(AudioClip clip)
        {
            _soundSource.PlayOneShot(clip, _defaultVolume);
        }

        public float GetCurrectNoise()
        {
            return _currentNoise;
        }

        public float GetNoiseDetection()
        {
            return _noiseDetection;
        }
    }
}