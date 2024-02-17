namespace BlackRece.TriFighter2D.Shooting {
    using UnityEngine;

    using Health;

    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour {
        private Rigidbody2D m_rigidbody2D;
        private HealthManager.EntityTypes m_entityType;
        
        private bool m_isPaused;
        private float m_speed;
        private Vector2 m_direction;
        private float m_damage;
        [SerializeField] private float m_lifeTimeMax = 5f;
        private float m_lifeTimer;

        public bool IsPaused {
            get => m_isPaused;
            set => m_isPaused = value;
        }
        
        private void Awake() {
            m_rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Start() {
            IsPaused = false;
        }
        
        private void Update() {
            if (m_isPaused)
                return;

            m_lifeTimer -= Time.deltaTime;
            if (m_lifeTimer <= 0f)
                Destroy(gameObject);
        }

        private void FixedUpdate() {
            m_rigidbody2D.velocity = m_isPaused
                ? Vector2.zero
                : m_direction * m_speed;
        }

        private void OnTriggerStay2D(Collider2D other) {
            if (other.gameObject.TryGetComponent(out HealthManager l_otherHealthManager)) {
                if(m_entityType != l_otherHealthManager.EntityType)
                    l_otherHealthManager.TakeDamage(m_damage);
            }

            Destroy(gameObject);
        }

        public void Fire(ProjectileMetaData p_metaData) {
            m_speed = p_metaData.Speed;
            m_direction = p_metaData.Direction;
            m_damage = p_metaData.Damage;
            m_entityType = p_metaData.EntityType;
            
            m_lifeTimer = m_lifeTimeMax;
        }
        
        public struct ProjectileMetaData {
            public float Speed;
            public float Damage;
            public Vector2 Direction;
            public HealthManager.EntityTypes EntityType;
            public ProjectileMetaData(
                float a_speed,
                float a_damage,
                Vector2 a_direction,
                HealthManager.EntityTypes a_entityType) {
                Speed = a_speed;
                Damage = a_damage;
                Direction = a_direction;
                EntityType = a_entityType;
            }
        }
    }
}