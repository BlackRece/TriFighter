using System;

using BlackRece.Events;

using TriFighter.FuSM;
using TriFighter.Types;

using UnityEngine;

namespace TriFighter {
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public sealed class ShipController : MonoBehaviour {
        [SerializeField] private bool DEBUG;
        [SerializeField] private StringEvent _debugMsgEvent;
        [SerializeField] private Vector3Event _positionEvent;
        
        [SerializeField] private AIInputController _aiInputController;
        [SerializeField] private AICursorController _aiCursorController;
        [SerializeField] private AIWeaponController _aiWeaponController;

        [SerializeField] private Transform _projectileOrigin;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private float _bulletSpeed = 5f;

        [SerializeField] private float _maxMoveSpeed = 0.1f;
        [SerializeField] private Vector3 _startingPosition = new Vector3(-15, 0, 0);

        [SerializeField] private FloatRange _playRange = new FloatRange(-30f, -5f, true);

        private IInputController _inputController;
        private ICursorController _cursorController;
        private IMovementController _movementController;
        private IWeaponController _weaponController;

        private bool _hasAIInput => _aiInputController != null;
        private bool _hasAICursor => _aiCursorController != null;
        private bool _hasAIWeapon => _aiWeaponController != null;

        public AIInputController AIInputController => _aiInputController;
        public Vector2 DEBUG_Axis;
        public Vector3 DEBUG_Speed;
        public Vector3 DEBUG_Position;
        
        private float _delay;
        private bool _shipActive;
        private const float MAX_DELAY = 1f;
        private Vector3 _lastPosition;

        public bool IsShipActive => _shipActive;

        public void Start() {
            
            if (_bulletPrefab == null)
                throw new ArgumentNullException(
                    nameof(_bulletPrefab),
                    "No [BULLET] game object selected.");

            if (ConfigureAIControllers())
                return;
            
            SetShipToActive(true);

            var shipTransform = transform;

            _inputController = new InputController();

            _cursorController = new CursorController();
            
            _cursorController.CreateMarker("TargetMarker", shipTransform);

            var limitData = new MovementController.MovementLimitData {
                    MaxMoveSpeed = _maxMoveSpeed,
                    PlayRange = _playRange
                };

            _movementController = new MovementController(shipTransform, limitData);
            
            // TODO: Replace with animation of player flying into view...
            shipTransform.position = _startingPosition;

            var projectileData = new ProjectileData {
                Origin = _projectileOrigin,
                Prefab = _bulletPrefab,
                ContainerName = "Bullets",
                FireDelay = 0.5f,
                Speed = 5.0f
            };

            _weaponController = new WeaponController(projectileData);
            
            _weaponController = new WeaponController(
                _bulletPrefab,
                "Bullets",
                0.5f
                );
        }
        
        public void Update() {
            if (!_shipActive) 
            {
                //return;
            }
            
            if (DEBUG != _movementController.DEBUG) _movementController.DEBUG = DEBUG;
            _lastPosition = transform.position;

            _inputController.Update();
            var cursorPosition = _cursorController.UpdatePosition(_inputController.MousePosition);
            
            _movementController.MoveByInput(_inputController.Axis);

            DEBUG_Axis = _inputController.Axis;
            DEBUG_Speed = _movementController.MoveSpeed;
            DEBUG_Position = _lastPosition;

            _weaponController.Update();
            
            if (_hasAIInput) {
                _aiInputController.UpdateShipPosition(_lastPosition);
                gameObject.SetActive(!_aiInputController.IsOutOfPlayArea);
                return;
            }

            //NotifyDisplay($"Axis: {_inputController.Axis.x}:{_inputController.Axis.y}");
            //NotifyDisplay($"Position: {_lastPosition.x}:{_lastPosition.y}");

            CameraController.UpdatePlayerPosition(_lastPosition);

            _weaponController.ActivateWeapon(
                _inputController.MouseButtonLeft,
                new BulletData {
                    Origin = _lastPosition,
                    Target = cursorPosition,
                    Speed = _bulletSpeed
                }
            );
        }

        public void SetShipToActive(bool state) => _shipActive = state;

        private bool ConfigureAIControllers() {
            if (!_hasAIInput)
                return false;

            _aiInputController.Init(StateIdentifier.Cruise, 1.0f);
            
            _inputController = (IInputController) _aiInputController;

            _cursorController = (ICursorController) _aiCursorController;

            var limitData = new MovementController.MovementLimitData {
                MaxMoveSpeed = _aiInputController.MaxMoveSpeed,
                PlayRange = _aiInputController.PlayArea
            };

            _movementController = new MovementController(transform, limitData);

            _weaponController = _aiWeaponController;

            SetShipToActive(false);
            
            return _hasAIInput;
        }
        
        private void OnCollisionEnter(Collision collision) {
            if (!gameObject.activeSelf)
                return;
            _movementController.ApplyCollisionWith(collision.transform.position);
            NotifyImmediate($"COLLISION: {collision.gameObject.name}");
            //Log($"Collision with {collision.gameObject.name}");
        }

        private void BroadCastPosition() {
            if (_positionEvent == null) return;
            _positionEvent.Raise(_lastPosition);
        }

        private void NotifyDisplay(string message) {
            if ((_delay -= Time.deltaTime) > 0)
                return;

            _delay = MAX_DELAY;
            NotifyImmediate(message);
        }

        private void NotifyImmediate(string message) {
            if (_debugMsgEvent == null) return;
            _debugMsgEvent.Raise(message);
        }
        
        private void Log(string message) {
            if (DEBUG)
                Debug.Log(message);
        }
    }

    public sealed class MouseEvent {
        public Vector3 MousePosition { get; set; }
    }
}