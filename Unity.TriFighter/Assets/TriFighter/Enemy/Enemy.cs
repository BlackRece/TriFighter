using UnityEngine;

namespace TriFighter {
    public interface IEnemy {
        bool IsActive { get; }
        GameObject Prefab { get; }
    }
    
    public class Enemy : MonoBehaviour, IEnemy {
        public bool IsActive => gameObject.activeSelf;

        public GameObject Prefab => gameObject;
        private float _speed = 1;
    }
}