using System;

using TriFighter.FuSM;
using TriFighter.Types;

using UnityEngine;

namespace TriFighter {
    [CreateAssetMenu(menuName = "TriFighter Objects/AI Controllers/New AI InputController")]
    public sealed class AIInputController : ScriptableObject, IInputController {
        [SerializeField] private StateLibrary _stateLibrary;
        
        public FloatRange PlayArea => _playArea;
        public bool IsOutOfPlayArea => !_playArea.IsInRange(_shipPosition.x);

        public Vector2 Axis => _axis;
        public float MaxMoveSpeed => _maxMoveSpeed;

        public Vector3 MousePosition => _mousePosition;
        public bool MouseButtonLeft => _mouseButtonLeft;
        
        private readonly FloatRange _playArea = new FloatRange(-100, +100, true);
        private Vector2 _axis;
        private Vector3 _mousePosition;
        private bool _mouseButtonLeft;
        private float _maxMoveSpeed;
        private Vector3 _shipPosition;
        private Vector3 _targetPosition;
        private bool _hasTarget;
        private bool _isHoming;
        
        private IAIControl _aiControl;
        private FloatRange _yPlayArea;
        private FloatRange _xPlayArea;

        public void Init(StateIdentifier stateID, float stateLevel) {
            var machineType = _stateLibrary == null 
                ? FuSMMachineType.None 
                : _stateLibrary.MachineType;

            
            _aiControl = new AIControl(this, machineType, _stateLibrary.States);
        }

        public void Update() {
        //from this point call FSM methods to move AI controlled ships
            _mousePosition = Vector3.zero;

            if (_aiControl != null) {
                _aiControl.UpdatePerceptions(new PerceptionData {
                    //TODO: get play area from enemyManager and pass to enemy
                    ShipPosition = _shipPosition,
                    TargetPosition = _targetPosition,
                    
                    HealthRange = new IntRange(0, 10),
                    CurrentHealth = 5,
                    
                    XPlayArea = _xPlayArea,
                    YPlayArea = _yPlayArea
                });
                
                _aiControl.Update();

                var reactions = _aiControl.ReactionsData;
                _axis = reactions.Axis;

                return;
            }
            
            MoveLeft();
            TrackTarget(_targetPosition);
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

        //TODO: move to movement controller!
        public void SetPlayArea(Transform botLeft, Transform topRight) {
            _xPlayArea = new FloatRange(botLeft.position.x, topRight.position.x);
            _yPlayArea = new FloatRange(botLeft.position.y, topRight.position.y);
        }
    }
}