using TriFighter.Terrain;
using TriFighter.Types;

using UnityEngine;

namespace TriFighter {
    public interface IMovementController {
        Vector3 MoveSpeed { get; }
        bool DEBUG { get; set; }
        void MoveByInput(Vector3 inputAxis);
        void ApplyCollisionWith(Vector3 target);
    }
    
    public sealed class MovementController : IMovementController {
        private const float DRAG_RATIO = 0.02f;
        private readonly Transform _transform;
        private readonly FloatRange _playRange;
        private readonly float _maxMoveSpeed;
        private readonly float _dragSpeed;
        private Vector3 _moveSpeed;
        private FloatRange _verticalRange;

        public Vector3 MoveSpeed => _moveSpeed;
        public bool DEBUG { get; set; }
            
        public struct MovementLimitData {
            public float MaxMoveSpeed { get; set; }
            public FloatRange PlayRange { get; set; }
        }

        public MovementController(Transform transform, MovementLimitData limitData) {
            _transform = transform;
            _maxMoveSpeed = limitData.MaxMoveSpeed;
            _playRange = limitData.PlayRange;
            
            _moveSpeed = new Vector2();
            _dragSpeed = _maxMoveSpeed * DRAG_RATIO;
            
            //Log($"Drag: {_dragSpeed}");
        }

        public void MoveByInput(Vector3 inputAxis) {
            var isNotZero = inputAxis != Vector3.zero;
            var preInputmoveSpeed = _moveSpeed;
            ApplyInput(inputAxis);

            // if (isNotZero) {
            //     Log($"Move speed pre input: {preInputmoveSpeed}");
            //     Log($"Move speed after input: {_moveSpeed}");
            // }
            
            ApplyDrag();
            ClampSpeed();
            //if (isNotZero) Log($"Move speed after clamp: {_moveSpeed}");

            MoveBy();
            LimitByArea();
        }

        public void ApplyCollisionWith(Vector3 target) {
            var position = _transform.position;
            
            if (position.x < target.x) _moveSpeed.x = -_maxMoveSpeed;
            if (position.x > target.x) _moveSpeed.x = +_maxMoveSpeed;
            
            if (position.y < target.y) _moveSpeed.y = -_maxMoveSpeed;
            if (position.y > target.y) _moveSpeed.y = +_maxMoveSpeed;
            
            MoveBy();
        }

        private void ApplyInput(Vector3 input) => _moveSpeed += input;

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
            var range = TerrainManager.VerticleRange;
            var position = _transform.position;
            position.x = Mathf.Clamp(position.x, _playRange.min, _playRange.max);
            position.y = Mathf.Clamp(position.y, range.min + 1, range.max - 1);
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