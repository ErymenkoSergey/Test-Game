using Morkwa.Interface;
using Morkwa.MainData;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace Morkwa.Mechanics.Characters
{
    public sealed class Enemy : BaceCharacter, IEnemy
    {
        [SerializeField] private MeshRenderer _torchRenderer;
        [SerializeField] private SkinnedMeshRenderer _bodyRenderer;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private GameObject _aiVision;
        private IVisions _iVisions;

        private float _defaultVolume = 1f;
        private AIStatus _currentStatusEnemy;
        private Color32 _defaultBodyColor;
        private bool _isPatrolStatus;
        private bool _movingStatus;
        private Vector3 _bugPosition;
        private float _timeBug;

        private bool _isAlertPlay;
        private int _randomPosition;

        public override void SetInfo(IGame game)
        {
            IGame = game;

            ISpawner = IGame.GetSpawner();
            IAudio = IGame.GetAudioManager();

            SetAiVision();
            SetConfiguration();
            SetAgentConfigurator();
            SetDefaultBodyColor();
            SetNewStatusEnemy(AIStatus.Patrol);
        }

        private void SetAiVision()
        {
            if (_aiVision.TryGetComponent(out IVisions iVisions))
                _iVisions = iVisions;
        }

        public override void SetConfiguration()
        {
            EnemyCharacterConfig _configEnemy = IGame.GetEnemyConfig();

            Speed = _configEnemy.CommonSpeed;
            TimeWait = _configEnemy.WaitTime;
            Acceleration = _configEnemy.Acceleration;
            AlarmClearTimer = _configEnemy.AlarmClearTimer;
            DefaultColor = _configEnemy.DefaultColor;
            HunterColor = _configEnemy.HunterColor;

            _iVisions.SetVision(_configEnemy.DistanceView, _configEnemy.AngleView);
        }

        private void SetAgentConfigurator()
        {
            _agent.speed = Speed;
            _agent.acceleration = Acceleration;
        }

        private void SetDefaultBodyColor()
        {
            _defaultBodyColor = _bodyRenderer.material.color;
        }

        private void FixedUpdate()
        {
            if (!IGame.GetPlayGameStatus())
                return;

            Move();
            HunterStatusActive();
        }

        private void ChangeTorchColor(Color32 color)
        {
            _torchRenderer.material.color = color;
        }

        private void ChangeBodyColor(Color32 color)
        {
            _bodyRenderer.material.color = color;
        }

        public override void Move()
        {
            if (!_isPatrolStatus)
                return;

            if (_movingStatus)
            {
                _agent.SetDestination(ISpawner.GetWallsList()[_randomPosition]);

                PlayStepSound();

                if (Vector3.Distance(transform.position, ISpawner.GetWallsList()[_randomPosition]) < 0.3f)
                {
                    SetAnimatorStatus("State", 0);
                    TimeWait = Time.time;
                    _movingStatus = false;
                }

                if (Vector3.Distance(transform.position, _bugPosition) < 0.4f && Time.time - _timeBug > 1.5f)
                {
                    SetNewRandomPointForPatrol();
                    _timeBug = Time.time;
                }
            }
            else
            {
                if (Time.time - TimeWait > 2.0f)
                {
                    SetNewRandomPointForPatrol();
                    _movingStatus = true;
                    _bugPosition = transform.position;
                    _timeBug = Time.time;
                }
            }
        }

        private void HunterStatusActive()
        {
            if (IAudio.GetCurrectNoise() >= IAudio.GetNoiseDetection() || _iVisions.GetIsSeeingStatus())
            {
                SetNewStatusEnemy(AIStatus.Hunter);

                _agent.SetDestination(IGame.IPlayer.GetTransform().position);
                PlayStepSound();
            }

            if (!_isAlertPlay && _iVisions.GetIsSeeingStatus())
            {
                _isAlertPlay = true;
                IAudio.PlaySoundAlert();
            }
        }

        private void SetNewStatusEnemy(AIStatus status)
        {
            if (_currentStatusEnemy == status)
                return;

            switch (status)
            {
                case AIStatus.Patrol:
                    ChangeTorchColor(DefaultColor);
                    ChangeBodyColor(_defaultBodyColor);
                    SetNewStatusPatrol(true);
                    break;
                case AIStatus.Hunter:
                    ChangeTorchColor(HunterColor);
                    ChangeBodyColor(HunterColor);
                    SetNewStatusPatrol(false);
                    StartCoroutine(AlarmClearTimers());
                    SetAnimatorStatus("State", 1);
                    break;
                default:
                    break;
            }

            _currentStatusEnemy = status;
        }

        private IEnumerator AlarmClearTimers()
        {
            yield return new WaitForSeconds(AlarmClearTimer);
            SetPatrolState();
        }

        private void SetPatrolState()
        {
            _isAlertPlay = false;
            _iVisions.SetStatusIsSeeing(false);
            IAudio.SetDefaultState();

            SetNewStatusEnemy(AIStatus.Patrol);
        }

        private void SetNewStatusPatrol(bool isOn)
        {
            _isPatrolStatus = isOn;
        }

        private void PlayStepSound()
        {
            if (StepsSource.isPlaying)
                return;

            StepsSource.PlayOneShot(StepsSound, _defaultVolume);
        }

        private void SetNewRandomPointForPatrol()
        {
            _randomPosition = Random.Range(0, ISpawner.GetWallsList().Count);
            _agent.SetDestination(ISpawner.GetWallsList()[_randomPosition]);
            SetAnimatorStatus("State", 1);
        }

        public override void SetAnimatorStatus(string name, int value)
        {
            _animator.SetInteger(name, value);
        }
    }
}