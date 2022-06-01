using System;

using UnityEngine;

namespace TriFighter {
    public sealed class ShipController : MonoBehaviour {
        [SerializeField] private AIInputController _aiInputController;
        [SerializeField] private AICursorController _aiCursorController;
        [SerializeField] private AIWeaponController _aiWeaponController;

        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private float _bulletSpeed = 5f;

        private IInputController _inputController;
        private ICursorController _cursorController;
        private IMovementController _movementController;
        
        private IWeaponController _weaponController;
        //private IBullet _bullet;

        private bool _hasAIInput => _aiInputController != null;
        private bool _hasAICursor => _aiCursorController != null;
        private bool _hasAIWeapon => _aiWeaponController != null;
        
        public Vector3 DEBUG_Mouse;
        public Vector3 DEBUG_Cursor;
        public Vector3 DEBUG_Cam;

        public void Start() {
            _inputController = _hasAIInput
                ? (IInputController) _aiInputController
                : new InputController();

            _cursorController = _hasAICursor
                ? (ICursorController) _aiCursorController
                : new CursorController();
            
            _cursorController.CreateMarker("TargetMarker", transform);
            
            _movementController = _hasAIInput
                ? new MovementController(transform, 5f, 1f)
                : new MovementController(transform, 10f, 5f);

            if (_bulletPrefab == null)
                throw new ArgumentNullException(nameof(_bulletPrefab), "No [BULLET] game object selected."); 
            
            _weaponController = _hasAIWeapon
                ? (IWeaponController) _aiWeaponController
                : new WeaponController(_bulletPrefab, "Bullets", 0.5f);
        }

        public void Update() {
            var shipPosition = transform.position;
            
            _inputController.Update();
            var cursorPosition = _cursorController.Update(_inputController.MousePosition);
            
            _movementController.ProcessInput(_inputController.Axis);
            _movementController.AdjustRotation(cursorPosition);

            _weaponController.Update();
            
            if(_hasAIInput)
                return;

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
    }

    public sealed class MouseEvent {
        public Vector3 MousePosition { get; set; }
    }
}