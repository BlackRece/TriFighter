using TriFighter.Types;

using UnityEngine;

namespace TriFighter.FuSM {
    public sealed class CruiseState : IState {
        public StateIdentifier StateID => _stateID;

        public float ActivationLevel {
            get => _activationLevel;
            set => _activationLevel = value;
        }

        public bool IsActive {
            get => _isActive;
            set => _isActive = value;
        }

        public ReactionData GetReactionData {
            get => _reactionData;
            set => _reactionData = value;
        }
        
        private bool _isActive;
        private float _activationLevel;
        private float _lastActivationLevel;
        private ReactionData _reactionData;
        private FloatRange _activationRange;
        private PerceptionData _perceptionData;
        private readonly FuSMMachineType _machineType;
        private readonly IAIControl _aiControl;
        private StateIdentifier _stateID;

        public CruiseState(FuSMMachineType machineType, IAIControl aiControl) {
            _machineType = machineType;
            _aiControl = aiControl;
            _stateID = StateIdentifier.Cruise;
        }
        
        public void Init(FloatRange activationRange) {
            _activationRange = activationRange;
            _activationLevel = activationRange.max;
        }

        public float CalculateActivation(PerceptionData perceptionData) {
            _perceptionData = perceptionData;

            return _perceptionData.YPlayArea.IsInRange(_perceptionData.ShipPosition.y)
                ? 1.0f
                : 0.0f;
        }

        public void SetActivationValue(float value) {
            _lastActivationLevel = _activationLevel;
            _activationLevel = _activationRange.BoundToRange(value);
        }

        public void Enter(ReactionData reactionData) { }
        
        public ReactionData Update(ReactionData reactionData) {
            _reactionData.Axis = reactionData.Axis + Vector2.left;
            return _reactionData;
        }

        public void Exit(ReactionData reactionData) { }
    }
}