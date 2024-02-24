namespace BlackRece.TriFighter2D.Shooting {
    using UnityEngine;
    using UnityEngine.Pool;

    using Movement;
    using Health;

    [RequireComponent(typeof(ProjectileMovement))]
    public class Projectile : MonoBehaviour {
        private IObjectPool<GameObject> m_objectPool;
        private ProjectileMovement m_projectileMovement;
        private HealthManager.EntityTypes m_entityType;
        
        private float m_speed;
        private float m_damage;
        private Vector2 m_direction;
        
        
        [SerializeField] private float m_lifeTimeMax = 5f;
        private float m_lifeTimer;

        private bool m_isPaused;
        public bool IsPaused {
            get => m_isPaused;
            set => m_isPaused = value;
        }
        
        #region Unity Functions
        private void Awake() {
            m_projectileMovement = GetComponent<ProjectileMovement>();
        }

        private void Update() {
            if (m_isPaused)
                return;

            m_lifeTimer -= Time.deltaTime;
            if (m_lifeTimer <= 0f)
                m_objectPool.Release(gameObject);
        }

        private void OnTriggerStay2D(Collider2D other) {
            if (other.gameObject.TryGetComponent(out HealthManager l_otherHealthManager)) {
                if(m_entityType != l_otherHealthManager.EntityType)
                    l_otherHealthManager.TakeDamage(m_damage);
            }

            m_objectPool.Release(gameObject);
        }
        #endregion
        
        public void Fire(ProjectileMetaData p_metaData) {
            m_objectPool = p_metaData.Pool;
            
            // store the projectile's data
            m_speed = p_metaData.Speed;
            m_direction = p_metaData.Direction;
            m_damage = p_metaData.Damage;
            m_lifeTimer = m_lifeTimeMax;
            
            m_entityType = p_metaData.EntityType;
            
            // set m_projectileMovement's direction and speed
            m_projectileMovement.Init(m_speed, m_direction);
        }
        
        public struct ProjectileMetaData {
            public IObjectPool<GameObject> Pool;
            public float Speed;
            public float Damage;
            public Vector2 Direction;
            public HealthManager.EntityTypes EntityType;
        }
    }
}