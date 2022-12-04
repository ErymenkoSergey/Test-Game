using Morkwa.Interface;
using Morkwa.Mechanics.CommonBehaviours;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Morkwa.Mechanics.Input
{
    public class InputControl : CommonBehaviour, IControlable
    {
        private IMoveble _iMoveblePlayer;

        [SerializeField] private InputActionAsset _inputActions;
        private InputActionMap _playerActionMap;

        private InputAction _up;
        private InputAction _down;
        private InputAction _left;
        private InputAction _right;

        private void OnEnable()
        {
            SetLinks();
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void SetLinks()
        {
            _playerActionMap = _inputActions.FindActionMap("Player");

            _up = _playerActionMap.FindAction("UpMove");
            _down = _playerActionMap.FindAction("DownMove");
            _left = _playerActionMap.FindAction("LeftMove");
            _right = _playerActionMap.FindAction("RightMove");
        }

        private void Subscribe()
        {
            _up.started += UpMove;
            _up.canceled += UpMove;
            _down.started += DownMove;
            _down.canceled += DownMove;
            _left.started += LeftMove;
            _left.canceled += LeftMove;
            _right.started += RightMove;
            _right.canceled += RightMove;

            _playerActionMap.Enable();
            _inputActions.Enable();
        }

        public void SetPlayer(IMoveble moveble)
        {
            _iMoveblePlayer = moveble;
        }

        private void UnSubscribe()
        {
            _up.started -= UpMove;
            _up.canceled -= UpMove;
            _down.started -= DownMove;
            _down.canceled -= DownMove;
            _left.started -= LeftMove;
            _left.canceled -= LeftMove;
            _right.started -= RightMove;
            _right.canceled -= RightMove;

            _playerActionMap.Disable();
            _inputActions.Disable();
        }

        private void UpMove(InputAction.CallbackContext Context)
        {
            if (Context.started)
            {
                _iMoveblePlayer.Move(Controls.Up, true);
            }
            if (Context.canceled)
            {
                _iMoveblePlayer.Move(Controls.Up, false);
            }
        }

        private void DownMove(InputAction.CallbackContext Context)
        {
            if (Context.started)
            {
                _iMoveblePlayer.Move(Controls.Down, true);
            }
            if (Context.canceled)
            {
                _iMoveblePlayer.Move(Controls.Down, false);
            }
        }

        private void LeftMove(InputAction.CallbackContext Context)
        {
            if (Context.started)
            {
                _iMoveblePlayer.Move(Controls.Left, true);
            }
            if (Context.canceled)
            {
                _iMoveblePlayer.Move(Controls.Left, false);
            }
        }

        private void RightMove(InputAction.CallbackContext Context)
        {
            if (Context.started)
            {
                _iMoveblePlayer.Move(Controls.Right, true);
            }
            if (Context.canceled)
            {
                _iMoveblePlayer.Move(Controls.Right, false);
            }
        }
    }
}