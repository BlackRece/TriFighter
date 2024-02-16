using System.Collections.Generic;

using UnityEngine;

namespace TriFighter.FuSM {
    public interface IStateLibrary { }

    [CreateAssetMenu(menuName = "TriFighter Objects/FuSM/New State Library")]
    public sealed class StateLibrary : ScriptableObject, IStateLibrary {
        [SerializeField] private FuSMMachineType _machineType;
        [SerializeField] private StateIdentifier[] _stateIDs;

        public FuSMMachineType MachineType => _machineType;

        public List<StateIdentifier> States => new List<StateIdentifier>(_stateIDs);
    }
}