using UnityEngine;

namespace TriFighter {
    public interface IBullet {
        bool IsActive { get; }
        GameObject Prefab { get; }
    }

    public class Bullet : MonoBehaviour, IBullet {
        public bool IsActive => gameObject.activeSelf;

        public GameObject Prefab => gameObject;

        private float _speed = 2f;

        private void HideBullet() => gameObject.SetActive(false);

        private void Update() {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);

            if (transform.position.x > TargetChecker.PlayArea.xMax) 
                HideBullet();
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.TryGetComponent(typeof (IEnemy), out var enemy)) 
                HideBullet();
        }
    }
}