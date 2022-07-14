using UnityEditor;

using UnityEngine;

namespace TriFighter {
    public interface IBullet {
        bool IsActive { get; }
        GameObject Prefab { get; }

        void Launch(LaunchData launchData);
    }

    public class Bullet : MonoBehaviour, IBullet {
        public bool IsActive => gameObject.activeSelf;

        public GameObject Prefab => gameObject;

        private const bool DEBUG = true;
        
        private Vector3 _direction;
        private float _speed = 2f;
        private float _lifeTime;

        public void Launch(LaunchData launchData) {
            transform.position = launchData.origin;
            _direction = launchData.direction;
            _lifeTime = launchData.lifeTime;
            _speed = launchData.speed;
            
            gameObject.SetActive(true);
        }
        
        private void HideBullet() {
            if (IsActive) gameObject.SetActive(false);
        }

        private void Update() {
            if(!IsActive)
                return;
            
            transform.Translate(_direction * _speed * Time.deltaTime);

            _lifeTime -= Time.deltaTime;
            if(_lifeTime <= 0)
                HideBullet();
        }

        private void OnCollisionEnter(Collision other) {
            if(DEBUG)
                Debug.Log($"Bullet hit {other.gameObject.name}");
            
            if (other.gameObject.TryGetComponent(out IEnemy enemy)) 
                HideBullet();
        }
    }

    public sealed class LaunchData {
        public Vector3 origin;
        public Vector3 direction;
        public float lifeTime;
        public float speed;
    }
}