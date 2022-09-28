namespace TriFighter.FuSM {
    public sealed class AngryState : State {
        private PerceptionData _perceptionData;
        
        public override float CalculateActivation(PerceptionData perceptionData) {
            if(perceptionData.Distance <= perceptionData.TriggerRange)
                SetActivationValue(perceptionData.Distance);
            
            return base.CalculateActivation(perceptionData);
        }
        
        public override ReactionData Update(ReactionData reactionData) {
            base.Update(reactionData);
        }

        public AngryState(FuSMMachineType machineType, IAIControl aiControl) 
            : base(machineType, aiControl) { }
    }
}