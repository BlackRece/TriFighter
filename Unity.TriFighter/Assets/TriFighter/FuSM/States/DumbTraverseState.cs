using TriFighter.Types;

namespace TriFighter.FuSM {
    public sealed class DumbState : IState {
        private StateIdentifier _stateID;
        private bool _isActive;
        private float _activationLevel;
        private ReactionData _getReactionData;

        //private IAIControl _parent = ai
        public DumbState(FuSMMachineType machineType, IAIControl aiControl)  {
            
        }

        public StateIdentifier StateID {
            get => _stateID;
            set => _stateID = value;
        }

        public void Init(FloatRange activationRange) {
            throw new System.NotImplementedException();
        }

        public void Update(ReactionData reactionData) {
            throw new System.NotImplementedException();
        }

        public void Enter(ReactionData reactionData) {
            throw new System.NotImplementedException();
        }

        public void Exit(ReactionData reactionData) {
            throw new System.NotImplementedException();
        }

        public float CalculateActivation(PerceptionData perceptionData) {
            throw new System.NotImplementedException();
        }

        public bool IsActive {
            get => _isActive;
            set => _isActive = value;
        }

        public float ActivationLevel {
            get => _activationLevel;
            set => _activationLevel = value;
        }

        public void SetActivationValue(float value) {
            throw new System.NotImplementedException();
        }

        public ReactionData GetReactionData {
            get => _getReactionData;
            set => _getReactionData = value;
        }
    }
}