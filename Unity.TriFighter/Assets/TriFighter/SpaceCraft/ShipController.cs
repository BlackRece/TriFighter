using System;

using BlackRece.Events;

using UnityEngine;

namespace TriFighter {
    public sealed class ShipController : MonoBehaviour {
        [SerializeField] private AIInputController _aiInputController;
        [SerializeField] private AICursorController _aiCursorController;
        [SerializeField] private AIWeaponController _aiWeaponController;

        [SerializeField] private StringEvent _debugMsgEvent;
        [SerializeField] private bool DEBUG;

        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private float _bulletSpeed = 5f;

        [SerializeField] private float _maxMoveSpeed = 0.1f;

        private IInputController _inputController;
        private ICursorController _cursorController;
        private IMovementController _movementController;
        
        private IWeaponController _weaponController;

        private bool _hasAIInput => _aiInputController != null;
        private bool _hasAICursor => _aiCursorController != null;
        private bool _hasAIWeapon => _aiWeaponController != null;
        
        // public Vector3 DEBUG_Mouse;
        // public Vector3 DEBUG_Cursor;
        // public Vector3 DEBUG_Cam;
        public Vector2 DEBUG_Axis;
        public Vector3 DEBUG_Speed;
        public Vector3 DEBUG_Position;
        
        private float _delay;
        private float MAX_DELAY = 1f;

        public void Start() {
            _inputController = _hasAIInput
                ? (IInputController) _aiInputController
                : new InputController();

            _cursorController = _hasAICursor
                ? (ICursorController) _aiCursorController
                : new CursorController();
            
            _cursorController.CreateMarker("TargetMarker", transform);
            
            _movementController = _hasAIInput
                ? new MovementController(transform, _aiInputController.MaxMoveSpeed)
                : new MovementController(transform, _maxMoveSpeed);

            if (_bulletPrefab == null)
                throw new ArgumentNullException(
                    nameof(_bulletPrefab),
                    "No [BULLET] game object selected."); 
            
            _weaponController = _hasAIWeapon
                ? (IWeaponController) _aiWeaponController
                : new WeaponController(_bulletPrefab, "Bullets", 0.5f);
        }

        public void Update() {
            if (DEBUG != _movementController.DEBUG) _movementController.DEBUG = DEBUG;
            var shipPosition = transform.position;
            
            _inputController.Update();
            var cursorPosition = _cursorController.UpdatePosition(_inputController.MousePosition);
            
            _movementController.MoveByInput(_inputController.Axis);

            DEBUG_Axis = _inputController.Axis;
            DEBUG_Speed = _movementController.MoveSpeed;
            DEBUG_Position = shipPosition;

            _weaponController.Update();
            
            if(_hasAIInput)
                return;

            NotifyDisplay($"Axis: {_inputController.Axis.x}:{_inputController.Axis.y}");
            NotifyDisplay($"Position: {shipPosition.x}:{shipPosition.y}");

            CameraController.UpdatePlayerPosition(shipPosition);

            _weaponController.ActivateWeapon(
                _inputController.MouseButtonLeft,
                new BulletData {
                    Origin = shipPosition,
                    Target = cursorPosition,
                    Speed = _bulletSpeed
                }
            );
        }

        private void NotifyDisplay(string message) {
            if (_debugMsgEvent == null)
                return;

            if ((_delay -= Time.deltaTime) > 0)
                return;

            _delay = MAX_DELAY;
            _debugMsgEvent.Raise(message);
        }
    }

    public sealed class MouseEvent {
        public Vector3 MousePosition { get; set; }
    }
}