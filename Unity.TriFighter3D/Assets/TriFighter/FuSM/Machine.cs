using System;
using System.Collections.Generic;

using TriFighter.Types;

using UnityEngine;

namespace TriFighter.FuSM {
    public interface IMachine {
        void UpdatePerceptions(PerceptionData perceptionData);
        Vector2 Axis { get; }
    }

    public enum FuSMMachineType {
        None = 0,
        Dumb,
        Tracking,
        Homing,
        Bouncing
    }
    
    public sealed class Machine : IState, IMachine  {
        private readonly AIControl _parent;
        private readonly FuSMMachineType _machineType;
        public Vector2 Axis => _axis;
        
        private Vector2 _axis;
        private PerceptionData _perceptionData;
        private float _triggerRange;

        private Dictionary<StateIdentifier, IState> _states = new Dictionary<StateIdentifier, IState>();
        private List<IState> _activeStates = new List<IState>();
        private List<IState> _previousStates = new List<IState>();
        private bool _isActive;
        private StateIdentifier _stateID;
        private float _activationLevel;
        private ReactionData _getReactionData;
        private ReactionData _reactionData;

        public Machine(AIControl parent, FuSMMachineType machineType = FuSMMachineType.None) {
            _parent = parent;
            _machineType = machineType;
        }

        public StateIdentifier StateID => _stateID;

        public void Init(FloatRange activationRange) {
            throw new NotImplementedException();
        }

        public ReactionData Update(ReactionData reactionData) {
            _previousStates.Clear();

            if (_activeStates.Count < 1)
                return reactionData;

            _previousStates = _activeStates;
            _activeStates.Clear();
            
            var nonActiveStates = new List<IState>();
            
            foreach (var state in _states.Values) {
                if (state.CalculateActivation(_perceptionData) >= state.ActivationLevel)
                    _activeStates.Add(state);
                else
                    nonActiveStates.Add(state);
            }

            if (nonActiveStates.Count > 0) {
                foreach (var nonActiveState in nonActiveStates) {
                    nonActiveState.Exit(_reactionData);
                    _reactionData = nonActiveState.GetReactionData;
                } 
            }

            if (_activeStates.Count > 0) {
                foreach (var activeState in _activeStates) {
                    if(_previousStates.Contains(activeState))
                        activeState.Update(_reactionData);
                    else
                        activeState.Enter(_reactionData);

                    _reactionData = activeState.GetReactionData;
                }
            }

            return _reactionData;
        }

        public void Enter(ReactionData reactionData) {
            throw new NotImplementedException();
        }

        public void Exit(ReactionData reactionData) {
            throw new NotImplementedException();
        }

        public float CalculateActivation(PerceptionData perceptionData) {
            throw new NotImplementedException();
        }

        public bool IsActive {
            get => _isActive;
            set => _isActive = value;
        }

        public float ActivationLevel => _activationLevel;

        public void SetActivationValue(float value) {
            throw new NotImplementedException();
        }

        public ReactionData GetReactionData => _getReactionData;

        public void AddStates(List<StateIdentifier> states) {
            if(states.Count < 1)
                return;
            
            //_states.AddRange(states);
            foreach (var stateID in states) {
                if (stateID == StateIdentifier.None)
                    continue;
                
                _states.Add(stateID, CreateState(stateID));
            }
        }

        private IState CreateState(StateIdentifier stateIdentifier) {
            switch (stateIdentifier) {
                case StateIdentifier.Cruise:
                    return new CruiseState(_machineType, _parent);

                default:
                    return new DumbState(_machineType, _parent);
            }
        }

        public void UpdateStateActivation(StateIdentifier stateID, float activationLevel) {
            foreach (var state in _states.Values) {
                if (state.StateID == stateID) {
                    state.SetActivationValue(activationLevel);
                    return;
                }
            }
        }
        
        public void UpdatePerceptions(PerceptionData perceptionData) {
            _perceptionData = perceptionData;

            if (_triggerRange <= 0f) {
                _triggerRange = perceptionData.TriggerRange;
            } else {
                _perceptionData.TriggerRange = _triggerRange;
            }

            /*
            foreach (var state in _states.Values) {
                state.CalculateActivation(_perceptionData);
            }
            */
        }
    }
    
    public sealed class PerceptionData {
        private Vector3 _shipPosition;
        private Vector3 _targetPosition;
        private float _triggerRange;
        private IntRange _healthRange;
        private int _currentHealth;
        private FloatRange _xPlayArea;
        private FloatRange _yPlayArea;

        public Vector3 ShipPosition {
            get => _shipPosition;
            set => _shipPosition = value;
        }

        public Vector3 TargetPosition {
            get => _targetPosition;
            set => _targetPosition = value;
        }

        public float TriggerRange {
            get => _triggerRange;
            set => _triggerRange = value;
        }
        
        public float Distance => Vector3.Distance(_shipPosition, _targetPosition);

        public IntRange HealthRange {
            get { return _healthRange; }
            set { _healthRange = value; }
        }

        public int CurrentHealth {
            get { return _currentHealth; }
            set { _currentHealth = value; }
        }

        public FloatRange XPlayArea {
            get { return _xPlayArea; }
            set { _xPlayArea = value; }
        }

        public FloatRange YPlayArea {
            get { return _yPlayArea; }
            set { _yPlayArea = value; }
        }
    }

    public sealed class ReactionData {
        private Vector2 _axis;
        public Vector2 Axis { 
            get => _axis;
            set => _axis = value;
        }
    }
}