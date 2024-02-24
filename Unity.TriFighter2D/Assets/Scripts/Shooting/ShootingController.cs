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
        
        [SerializeField] private float m_bulletSpeed = 5f;
        [SerializeField] private float m_bulletDamage = 1f;
        
        [SerializeField] private float m_shootCooldown = 0.5f;
        private float m_shootTimer;
        [SerializeField] private int m_maxClipSize = 30;
        private int m_currentAmmo;
        [SerializeField] private float m_reloadCooldown = 2f;
        
        private ObjectPooler m_pooler;
        
        private HealthManager.EntityTypes m_entityType = HealthManager.EntityTypes.None;
        
        private bool m_isPaused;

        public bool IsPaused {
            get => m_isPaused;
            set => m_isPaused = value;
        }
        
        public float AmmoRatio => (float)m_currentAmmo / m_maxClipSize;

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

        private void PlayerUpdate() {
            if(Input.GetButton("Fire1"))
                Shoot();
            
            if (Input.GetKeyDown(KeyCode.R))
                Reload();
        }

        private void Shoot() {
            if (m_shootTimer > 0f)
                return;
            
            if (m_currentAmmo <= 0) {
                Reload();
                return;
            }
            
            m_shootTimer = m_shootCooldown;

            GameObject l_bullet = m_pooler.Pool.Get();
            l_bullet.transform.position = m_bulletSpawnPoint.position;
            l_bullet.transform.parent = transform;
            l_bullet.SetActive(true);
            
            var l_projectile = l_bullet.GetComponent<Projectile>();
            var l_data = new Projectile.ProjectileMetaData {
                Pool = m_pooler.Pool,
                Speed = m_bulletSpeed,
                Damage = m_bulletDamage,
                Direction = m_entityType == HealthManager.EntityTypes.Player 
                    ? Vector2.right
                    : Vector2.left,
                EntityType =  m_entityType
            };
            l_projectile.Fire(l_data);

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
                EventManager.InvokeEvent(EventIDs.OnAddAmmo, AmmoRatio);
        }
    }
}