using System;

using TriFighter.Types;

namespace TriFighter.FuSM {
    public enum StateIdentifier {
        None = 0,
        Cruise = 1
    }
    public interface IState {
        StateIdentifier StateID { get; }
        void Init(FloatRange activationRange);
        
        ReactionData Update(ReactionData reactionData);
        void Enter(ReactionData reactionData);
        void Exit(ReactionData reactionData);

        float CalculateActivation(PerceptionData perceptionData);
        bool IsActive { get; }
        float ActivationLevel { get; }
        void SetActivationValue(float value);
        ReactionData GetReactionData { get; }
    }

    [System.Serializable]
    public abstract class State : IState {
        private readonly FuSMMachineType _machineType;
        private readonly IAIControl _aiControl;
        
        private FloatRange _activationRange;
        private float _lastActivationLevel;
        private float _activationLevel;

        internal PerceptionData _perceptionData;
        internal ReactionData _reactionData;
        private StateIdentifier _stateID;

        public StateIdentifier StateID {
            get => _stateID;
            set => _stateID = value;
        }
        
        public PerceptionData Perception => _perceptionData;
        public ReactionData GetReactionData => _reactionData;

        public float ActivationLevel => _activationLevel;

        public bool IsActive => _activationRange.IsInRange(_activationLevel);

        public State(FuSMMachineType machineType, IAIControl aiControl) {
            _machineType = machineType;
            _aiControl = aiControl;
        }

        // state initiation
        public virtual void Init(FloatRange activationRange) {
            _activationRange = activationRange;
            _activationLevel = activationRange.min;
        }
        
        // state execution methods
        public virtual ReactionData Update(ReactionData reactionData) {
            return new ReactionData();}
        public virtual void Enter(ReactionData reactionData) {}
        public virtual void Exit(ReactionData reactionData) {}

        // activation level methods
        public virtual float CalculateActivation(PerceptionData perceptionData) {
            _perceptionData = perceptionData;
            
            return _activationLevel;
        }
        
        public virtual void SetActivationValue(float value) {
            _lastActivationLevel = _activationLevel;
            _activationLevel = _activationRange.BoundToRange(value);
        }
    }
}