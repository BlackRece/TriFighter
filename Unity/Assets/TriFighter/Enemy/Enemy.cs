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

        private void Update() {
            //transform.Translate(Vector3.left * _speed * Time.deltaTime);

            // if (transform.position.x < TargetChecker.PlayArea.xMin) {
            //     gameObject.SetActive(false);
            //     Debug.Log("enemy has left playarea");
            // }
        }
    }
}