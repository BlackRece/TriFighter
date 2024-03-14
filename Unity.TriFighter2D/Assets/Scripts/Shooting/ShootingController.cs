namespace BlackRece.TriFighter2D.Shooting {
    using UnityEngine;

    using Health;
    using Events;
    using Utils;

    [RequireComponent(
        typeof(HealthManager),
        typeof(ObjectPooler))]
    public class ShootingController : MonoBehaviour {
        [SerializeField] private Transform m_bulletSpawnPoint;
        
        [SerializeField] private Vector2 m_bulletSpeed = Vector2.zero;
        [SerializeField] private float m_bulletDamage = 1f;
        [SerializeField] private float m_bulletLifeTime = 5f;
        
        [SerializeField] private float m_reloadCooldown = 2f;
        [SerializeField] private float m_shootCooldown = 0.5f;
        private float m_shootTimer;
        
        [SerializeField] private int m_maxClipSize = 10;
        private int m_currentAmmo;
        
        private ProjectileController.ProjectileMetaData m_projectileMetaData; 
        public ProjectileController.ProjectileMetaData ProjectileMetaData {
            get => m_projectileMetaData;
            set => m_projectileMetaData = value;
        }

        private ObjectPooler m_pooler;
        public ObjectPooler Pooler => m_pooler;
        
        private HealthManager.EntityTypes m_entityType = HealthManager.EntityTypes.None;
        
        private bool m_isReloading;
        private bool IsReloading {
            get => m_isReloading;
            set {
                m_isReloading = value;
                EventManager.InvokeEvent(EventIDs.OnReload, m_isReloading);
            }
        }

        private bool m_isPaused;

        public bool IsPaused {
            get => m_isPaused;
            set => m_isPaused = value;
        }
        
        public float AmmoRatio => (float)m_currentAmmo / m_maxClipSize;
        
        #region Unity Functions
        private void Awake() {
            if (m_bulletSpawnPoint == null)
                m_bulletSpawnPoint = transform; 
            
            if (TryGetComponent(out ObjectPooler l_pooler))
                m_pooler = l_pooler;
            
            if (TryGetComponent(out HealthManager l_healthManager))
                m_entityType = l_healthManager.EntityType;
        }

        private void Start() {
            IsPaused = false;
            m_currentAmmo = m_maxClipSize;
            m_isReloading = false;
            
            m_bulletSpeed.x = 5f;
            
            m_projectileMetaData = new ProjectileController.ProjectileMetaData {
                Pool = m_pooler.Pool,
                Speed = m_bulletSpeed,
                Damage = m_bulletDamage,
                LifeTime = m_bulletLifeTime,
                Direction = Vector2.right,
                EntityType = m_entityType
            };
            
            EventManager.InvokeEvent(EventIDs.OnAddAmmo, AmmoRatio);
        }

        private void Update() {
            if (IsPaused)
                return;

            if (m_shootTimer > 0f)
                m_shootTimer -= Time.deltaTime;

            if (m_entityType == HealthManager.EntityTypes.Player)
                PlayerUpdate();
            else
                Shoot();
        }

        #endregion  // Unity Functions
        
        private void PlayerUpdate() {
            /*
             * Reloading Mechanic:
             * if we are reloading and the shoot timer has ended, stop reloading
             * if we are reloading and the shoot timer has not ended, continue reloading and don't shoot
             * if we have no ammo and not already reloading, reload immediately
             * if we have ammo and not currently reloading, shoot
             * if we have ammo but a reload is requested, reload and don't shoot
             */
            if (IsReloading && m_shootTimer <= 0f) 
                IsReloading = false;
            
            if (Input.GetKeyDown(KeyCode.R) || m_currentAmmo <= 0)
                Reload();
            
            if(Input.GetButton("Fire1") && !IsReloading)
                Shoot();
            
        }

        private void Shoot() {
            if (m_shootTimer > 0f)
                return;
            
            m_shootTimer = m_shootCooldown;

            GameObject l_bullet = m_pooler.Pool.Get();
            l_bullet.transform.position = m_bulletSpawnPoint.position;
            
            var l_projectile = l_bullet.GetComponent<ProjectileController>();
            l_projectile.Fire(m_projectileMetaData);

            if (m_entityType == HealthManager.EntityTypes.Player) {
                m_currentAmmo--;

                EventManager.InvokeEvent(EventIDs.OnDelAmmo, 1);
            }
        }
        
        private void Reload() {
            if (m_currentAmmo == m_maxClipSize)
                return;

            m_shootTimer = m_reloadCooldown;
            m_currentAmmo = m_maxClipSize;

            if (m_entityType == HealthManager.EntityTypes.Player) 
                IsReloading = true;
        }
    }
}