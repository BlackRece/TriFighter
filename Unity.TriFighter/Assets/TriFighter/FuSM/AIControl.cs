using System.Collections.Generic;

using UnityEngine;

namespace TriFighter.FuSM {
    public interface IAIControl {
        Machine Machine { get; }
        ReactionData ReactionsData { get; }
        void Update();
        void UpdatePerceptions(PerceptionData perceptionData);
        void Init();
    }
    
    public sealed class AIControl : IAIControl {
        public Machine Machine => _machine;
        public ReactionData ReactionsData => _reactionData;
        //perception data
        //(public so that states can share it)
        private PerceptionData _perceptionData;
        //data
        private Machine _machine;
        private Vector2 _axis;
        private ReactionData _reactionData;

        public AIControl(
            IInputController controller,
            FuSMMachineType machineType,
            List<StateIdentifier> states) {
            _machine = new Machine(this, machineType);
            _machine.AddStates(states);
        }

        public void Update() {
            UpdatePerceptions(_perceptionData);
            _reactionData = _machine.UpdateMachine();
        }

        public void UpdatePerceptions(PerceptionData perceptionData) {
            _perceptionData = perceptionData;
        }

        public void Init() { }

        
    }
}