using System;

using UnityEngine;

namespace TriFighter {
    public interface IMovementController {
        Vector3 MoveSpeed { get; }
        bool DEBUG { get; set; }
        void MoveByInput(Vector3 inputAxis);
    }
    
    public sealed class MovementController : IMovementController {
        private const float DRAG_RATIO = 0.02f;
        private readonly Transform _transform;
        private readonly float _maxMoveSpeed;
        private Vector3 _moveSpeed;
        private readonly float _dragSpeed;

        public Vector3 MoveSpeed => _moveSpeed;
        public bool DEBUG { get; set; }
            

        public MovementController(Transform transform, float maxMoveSpeed) {
            _transform = transform;
            _maxMoveSpeed = maxMoveSpeed;
            _moveSpeed = new Vector2();
            
            _dragSpeed = _maxMoveSpeed * DRAG_RATIO;
            Log($"Drag: {_dragSpeed}");
        }

        public void MoveByInput(Vector3 inputAxis) {
            var isNotZero = inputAxis != Vector3.zero;
            var preInputmoveSpeed = _moveSpeed;
            ApplyInput(inputAxis);

            if (isNotZero) {
                Log($"Move speed pre input: {preInputmoveSpeed}");
                Log($"Move speed after input: {_moveSpeed}");
            }
            
            ApplyDrag();
            ClampSpeed();
            if (isNotZero) Log($"Move speed after clamp: {_moveSpeed}");

            MoveBy();
            LimitByArea();
        }

        private void ApplyInput(Vector3 input) => 
            _moveSpeed += input;

        private void ApplyDrag() {
            if (_moveSpeed.x < 0) {
                _moveSpeed.x += _dragSpeed;
                if (_moveSpeed.x >= 0) _moveSpeed.x = 0;
            }
            
            if (_moveSpeed.x > 0) {
                _moveSpeed.x -= _dragSpeed;
                if (_moveSpeed.x <= 0) _moveSpeed.x = 0;
            }

            if (_moveSpeed.y < 0) {
                _moveSpeed.y += _dragSpeed;
                if (_moveSpeed.y >= 0) _moveSpeed.y = 0;
            }

            if (_moveSpeed.y > 0) {
                _moveSpeed.y -= _dragSpeed;
                if (_moveSpeed.y <= 0) _moveSpeed.y = 0;
            }
        }
        
        private void ClampSpeed() =>
            _moveSpeed.Set(
                Mathf.Clamp(_moveSpeed.x, -_maxMoveSpeed, _maxMoveSpeed),
                Mathf.Clamp(_moveSpeed.y, -_maxMoveSpeed, _maxMoveSpeed),
                0
            );

        private void MoveBy() => _transform.Translate(_moveSpeed * Time.deltaTime);

        private void LimitByArea() {
            var position = _transform.position;
            position.x = Mathf.Clamp(position.x, -5f, 5f);
            _transform.position = position;
        }
        
        private void NotifyMove(object publishEvent) {
            var noticeevent = publishEvent as MoveTargetEvent;
            
        }

        private void Log(string message) {
            if (DEBUG)
                Debug.Log(message);
        }
    }

    public sealed class MoveTargetEvent {
        public Vector3 Position { get; set; }
    }
}