using System;

using TriFighter.Types;

using UnityEngine;

namespace TriFighter {
    [CreateAssetMenu(menuName = "TriFighter Objects/AI Controllers/New AI InputController")]
    public sealed class AIInputController : ScriptableObject, IInputController {
        public FloatRange PlayArea => _playArea;
        public bool IsOutOfPlayArea => !_playArea.IsInRange(_shipPosition.x);

        public Vector2 Axis => _axis;
        public float MaxMoveSpeed => _maxMoveSpeed;

        public Vector3 MousePosition => _mousePosition;
        public bool MouseButtonLeft => _mouseButtonLeft;
        
        private readonly FloatRange _playArea = new FloatRange(-100, +100);
        private Vector2 _axis;
        private Vector3 _mousePosition;
        private bool _mouseButtonLeft;
        private float _maxMoveSpeed;
        private Vector3 _shipPosition;
        private Vector3 _targetPosition;
        private bool _hasTarget;
        private bool _isHoming;

        public void Update() {
            // _axis.Set(
            //     Random.Range(-1f,1f),
            //     Random.Range(-1f,1f)
            // );
            
            MoveLeft();
            TrackTarget(_targetPosition);
            
            _mousePosition = Vector3.zero;
        }

        public void UpdateShipPosition(Vector3 position) => _shipPosition = position;
        public void UpdateTargetPosition(Vector3 position) {
            if (_hasTarget) {
                if (!_isHoming) return;
            }
            
            _targetPosition = position;
            _hasTarget = true;
        }
        
        public void SetMaxMoveSpeed(float maxMoveSpeed) => _maxMoveSpeed = maxMoveSpeed;
        
        private void MoveLeft() => _axis = Vector2.left;

        private void TrackTarget(Vector3 target) {
            if (Math.Abs(_shipPosition.y - target.y) < 0.01f)
                return;

            _axis.y = _shipPosition.y < target.y
                ? +_maxMoveSpeed
                : -_maxMoveSpeed;
        }
    }
}