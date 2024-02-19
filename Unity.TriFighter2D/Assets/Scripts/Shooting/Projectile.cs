namespace BlackRece.TriFighter2D.Shooting {
    using UnityEngine;

    using Movement;
    using Health;

    [RequireComponent(typeof(ProjectileMovement))]
    public class Projectile : MonoBehaviour {
        private ProjectileMovement m_projectileMovement;
        private HealthManager.EntityTypes m_entityType;
        
        private float m_speed = 0f;
        private Vector2 m_direction = Vector2.zero;
        
        private float m_damage = 0f;
        
        [SerializeField] private float m_lifeTimeMax = 5f;
        private float m_lifeTimer;

        private bool m_isPaused = false;
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
                Destroy(gameObject);
        }

        private void OnTriggerStay2D(Collider2D other) {
            if (other.gameObject.TryGetComponent(out HealthManager l_otherHealthManager)) {
                if(m_entityType != l_otherHealthManager.EntityType)
                    l_otherHealthManager.TakeDamage(m_damage);
            }

            Destroy(gameObject);
        }
        #endregion
        
        public void Fire(ProjectileMetaData p_metaData) {
            // store the projectile's data
            m_speed = p_metaData.Speed;
            
            m_direction = p_metaData.Direction;
            m_damage = p_metaData.Damage;
            
            m_entityType = p_metaData.EntityType;
            
            m_lifeTimer = m_lifeTimeMax;
            
            // set m_projectileMovement's direction and speed
            m_projectileMovement.Init(m_speed, m_direction);
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