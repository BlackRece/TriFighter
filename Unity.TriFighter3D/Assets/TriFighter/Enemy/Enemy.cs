using TriFighter.FuSM;

using UnityEngine;

namespace TriFighter {
    public interface IEnemy {
        bool IsActive { get; }
        GameObject Prefab { get; }
        Vector3 PrefabSize { get; }
        FuSMMachineType MachineType { get; }

    }
    
    public sealed class Enemy : MonoBehaviour, IEnemy {
        public bool IsActive => gameObject.activeSelf;

        public GameObject Prefab => gameObject;
        public Vector3 PrefabSize => gameObject
            .GetComponentInChildren<Renderer>()
            .bounds.size;

        public FuSMMachineType MachineType => _machineType;

        private float _speed = 1;
        private FuSMMachineType _machineType;
        private IAIControl _aiControl;

    }

}