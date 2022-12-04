using Morkwa.Interface;
using Morkwa.Mechanics.CommonBehaviours;
using UnityEngine;

namespace Morkwa.Camera
{
    public class CameraControl : CommonBehaviour, ICameraControl
    {
        private Transform _player;
        private Vector3 _defaultPosition;
        private bool _isPlayerEnable;

        public void SetPlayerTransform(Transform gameObject)
        {
            _player = gameObject;
            SetDefaultPosition();
        }

        private void SetDefaultPosition()
        {
            _defaultPosition = transform.position - _player.position;
            _isPlayerEnable = true;
        }

        private void FixedUpdate()
        {
            if (!_isPlayerEnable)
                return;

            transform.position = _player.position + _defaultPosition;
        }
    }
}