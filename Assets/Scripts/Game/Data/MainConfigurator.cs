using System;
using UnityEngine;

namespace Morkwa.MainData
{
    [CreateAssetMenu(menuName = "ScriptableObject/MainConfigurator", fileName = "MainConfigurator")]
    public class MainConfigurator : ScriptableObject
    {
        public Setting SettingGame;
    }

    [Serializable]
    public struct Setting
    {
        [Header("Game field settings")]
        public GameFieldConfig GameFieldConfig;

        [Space]
        [Header("Characters settings")]
        public EnemyCharacterConfig CharacterConfig;

        [Space]
        [Header("Balance (Noise) setting")]
        public BalanceConfig BalanceConfig;
    }

    [Serializable]
    public struct GameFieldConfig
    {
        [SerializeField] private Vector2 _sizeGameField;
        public Vector2 SizeGameField => _sizeGameField;

        [SerializeField, Range(1, 1000)] private int _countObstacle;
        public int CountObstacle => _countObstacle;

        [SerializeField, Range(1, 200)] private int _countEnemy;
        public int CountEnemy => _countEnemy;
    }

    [Serializable]
    public struct EnemyCharacterConfig
    {
        [SerializeField, Range(1, 12)] private float _commonSpeed;
        public float CommonSpeed => _commonSpeed;

        [SerializeField] private float _waitTime;
        public float WaitTime => _waitTime;

        [SerializeField, Range(1, 28)] private float _acceleration;
        public float Acceleration => _acceleration;

        [SerializeField, Range(5, 100)] private float _alarmClearTimer;
        public float AlarmClearTimer => _alarmClearTimer;

        [SerializeField] private Color32 _defaultColor;
        public Color32 DefaultColor => _defaultColor;

        [SerializeField] private Color32 _hunterColor;
        public Color32 HunterColor => _hunterColor;

        [SerializeField, Range(1, 80)] private float _distanceView;
        public float DistanceView => _distanceView;

        [SerializeField, Range(0, 360)] private float _angleView;
        public float AngleView => _angleView;
    }

    [Serializable]
    public struct BalanceConfig
    {
        [SerializeField] private float _addingNoisePerSecond;
        public float AddingNoisePerSecond => _addingNoisePerSecond;

        [SerializeField] private float _noiseReductionInHalfSeconds;
        public float NoiseReductionInHalfSeconds => _noiseReductionInHalfSeconds;

        [SerializeField] private float _noiseDetection;
        public float NoiseDetection => _noiseDetection;
    }
}