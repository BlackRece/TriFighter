using UnityEngine;

namespace TriFighter {
    public sealed class ShipController : MonoBehaviour {
        [SerializeField] private AIInputController _aiInputController = null;
        [SerializeField] private AICursorController _aiCursorController = null;

        private IInputController _inputController;
        private IMovementController _movementController;
        private ICursorController _cursorController;

        private bool _hasAIInput => _aiInputController != null;
        private bool _hasAICursor => _aiCursorController != null;
        
        private object _weaponHandler;

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
            
            _weaponHandler = new object();
        }

        public void Update() {
            _inputController.Update();
            var cursorPosition = _cursorController.Update(_inputController.MousePosition);
            
            _movementController.ProcessInput(_inputController.Axis);
            _movementController.AdjustRotation(cursorPosition);

            if(!_hasAIInput)
                CameraController.UpdatePlayerPosition(transform.position);
        }
    }

    public sealed class MouseEvent {
        public Vector3 MousePosition { get; set; }
    }
}