using UnityEngine;

namespace TriFighter {
    public interface IWeaponController {
        void ActivateWeapon(bool activationState, BulletData bulletData);
        void Update();
    }

    public sealed class WeaponController : IWeaponController {
        private readonly IGameObjectPooler _bulletPool;
        private bool _isReadyToFire;
        private float _fireDelay;
        private readonly float _maxFireDelay;
        private Vector3 _origin;
        private float _speed;

        public WeaponController(GameObject bullet, string containerName, float maxFireDelay) {
            _maxFireDelay = maxFireDelay;
            _isReadyToFire = false;
            
            _bulletPool = ScriptableObject.CreateInstance<GameObjectPooler>();
            _bulletPool.Init(bullet, containerName);
        }
        public WeaponController(ProjectileData projectileData) {
            _maxFireDelay = projectileData.FireDelay;
            _isReadyToFire = false;
            
            _bulletPool = ScriptableObject.CreateInstance<GameObjectPooler>();
            _bulletPool.Init(projectileData.Prefab, projectileData.ContainerName);
            _speed = projectileData.Speed;

            _origin = projectileData.Origin.position;
        }

        public void Update() {
            UpdateFireDelay();
        }

        public void ActivateWeapon(bool activationState, BulletData bulletData) {
            if (!_isReadyToFire || !activationState) 
                return;
            
            _isReadyToFire = false;
            _fireDelay = _maxFireDelay;
            
            var bullet = _bulletPool.GetBullet();
            // bullet.Launch(new LaunchData {
            //     origin = bulletData.Origin,
            //     direction = bulletData.Direction,
            //     lifeTime = 10f,
            //     speed = bulletData.Speed,
            // });

            bullet.Launch(new LaunchData {
                origin = _origin,
                direction = Vector3.right,
                lifeTime = 10f,
                speed = _speed,
            });
        }

        private void UpdateFireDelay() {
            if (_isReadyToFire)
                return;

            if (_fireDelay > 0) {
                _fireDelay -= Time.deltaTime;
                return;
            }

            if(_fireDelay <= 0) 
                _fireDelay = 0f;
            
            if(!_isReadyToFire)
                _isReadyToFire = true;
        }
    }

    public sealed class ProjectileData {
        public Transform Origin { get; set; }
        public GameObject Prefab { get; set; }
        public string ContainerName { get; set; }
        public float FireDelay { get; set; }

        public float Speed { get; set; }
    }
    
    public sealed class BulletData {
        public Vector3 Origin { get; set; }
        public Vector3 Target { get; set; }
        
        public float Speed { get; set; }

        public Vector3 Direction => Vector3.right;
    }
}