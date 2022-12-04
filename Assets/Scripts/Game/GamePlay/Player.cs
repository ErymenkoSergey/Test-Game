using Morkwa.Interface;
using Morkwa.MainData;
using Morkwa.OtherContent;
using UnityEngine;

namespace Morkwa.Mechanics.Characters
{
    public sealed class Player : BaceCharacter, IMoveble, IPlayer
    {
        private float _turnCoeficent = 10;

        [SerializeField] private Rigidbody _body;

        private Vector3 _position;
        private Vector3 _moveDirection;

        private readonly float _distanceNoise = 0.2f;

        private bool _isMovingUp;
        private bool _isMovingDown;
        private bool _isMovingLeft;
        private bool _isMovingRight;

        public override void SetInfo(IGame game)
        {
            IGame = game;
            IGame.SetPlayer(this);
            IAudio = IGame.GetAudioManager();
            _moveDirection = Vector3.zero;
            _position = _body.position;

            SetConfiguration();
        }

        public override void SetConfiguration()
        {
            EnemyCharacterConfig _configEnemy = IGame.GetEnemyConfig();
            Speed = _configEnemy.CommonSpeed;
        }

        private void FixedUpdate()
        {
            if (!IGame.GetPlayGameStatus())
                return;

            Move();
            AddNoiseValue();
        }

        public override void Move()
        {
            _moveDirection = Vector3.zero;

            if (_isMovingUp)
                _moveDirection += Vector3.forward * Speed;
            if (_isMovingDown)
                _moveDirection += Vector3.back * Speed;
            if (_isMovingLeft)
                _moveDirection += Vector3.left * Speed;
            if (_isMovingRight)
                _moveDirection += Vector3.right * Speed;

            if (_moveDirection != Vector3.zero)
                SetAnimatorStatus(true);
            else
                SetAnimatorStatus(false);

            Vector3 direction = Vector3.RotateTowards(transform.forward, _moveDirection, Speed * _turnCoeficent, 0.0f);
            transform.localRotation = Quaternion.LookRotation(direction);

            _body.MovePosition(_body.position + _moveDirection * Time.fixedDeltaTime);
        }

        private void AddNoiseValue()
        {
            if (Vector3.Distance(_position, _body.transform.position) > _distanceNoise)
            {
                IAudio.AddNoiseValue();
                _position = _body.transform.position;
            }
        }

        private void SetAnimatorStatus(bool isOn)
        {
            if (isOn)
            {
                SetAnimatorStatus("State", 1);

                if (!StepsSource.isPlaying)
                {
                    StepsSource.clip = StepsSound;
                    StepsSource.Play();
                }
            }
            else
            {
                SetAnimatorStatus("State", 0);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {
                IGame.GameOver(false);
                _moveDirection = Vector3.zero;
            }

            if (collision.gameObject.TryGetComponent(out Exit exit))
            {
                IGame.GameOver(true);
            }
        }

        public void Move(Controls controls, bool isOn)
        {
            switch (controls)
            {
                case Controls.Up:
                    _isMovingUp = isOn;
                    break;
                case Controls.Down:
                    _isMovingDown = isOn;
                    break;
                case Controls.Left:
                    _isMovingLeft = isOn;
                    break;
                case Controls.Right:
                    _isMovingRight = isOn;
                    break;
            }
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public IMoveble GetMoveble()
        {
            return this;
        }

        public override void SetAnimatorStatus(string name, int value)
        {
            _animator.SetInteger(name, value);
        }
    }
}