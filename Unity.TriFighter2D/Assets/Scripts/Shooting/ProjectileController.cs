namespace BlackRece.TriFighter2D.Shooting {
    using UnityEngine;
    using UnityEngine.Pool;

    using Movement;
    using Health;

    [RequireComponent(typeof(MotionController))]
    public class ProjectileController : MonoBehaviour {
        private MotionController m_motionController;
        private IObjectPool<GameObject> m_objectPool;
        
        private Vector2 m_speed = Vector2.zero;
        private Vector2 m_direction = Vector2.zero;
        private float m_damage;
        private HealthManager.EntityTypes m_entityType;
        
        [SerializeField] private float m_lifeTimeMax = 5f;
        private float m_lifeTimer;

        private bool m_isPaused;
        public bool IsPaused {
            get => m_isPaused;
            set => m_isPaused = value;
        }
        
        #region Unity Functions
        private void Awake() {
            m_motionController = GetComponent<MotionController>();
        }

        private void Update() {
            if (IsPaused)
                return;

            m_lifeTimer -= Time.deltaTime;
            if (m_lifeTimer <= 0f)
                KillProjectile();
            
            // shouldn't need to update the direction or speed since set in the fire method.
            m_motionController.Direction = m_direction;
            m_motionController.Speed = m_speed;
        }

        private void OnTriggerStay2D(Collider2D other) {
            if (other.gameObject.TryGetComponent(out HealthManager l_otherHealthManager)) {
                if(m_entityType != l_otherHealthManager.EntityType)
                    l_otherHealthManager.TakeDamage(m_damage);
            }

            KillProjectile();
        }
        
        #endregion  // Unity Functions
        
        public void Fire(ProjectileMetaData p_metaData) {
            m_objectPool = p_metaData.Pool;
            
            // store the projectile's data
            m_speed = p_metaData.Speed;
            m_direction = p_metaData.Direction;
            m_damage = p_metaData.Damage;
            m_lifeTimer = p_metaData.LifeTime <= 0f
                ? m_lifeTimeMax
                : p_metaData.LifeTime;
            
            m_entityType = p_metaData.EntityType;
            
            // set movement controller's data
            m_motionController.Speed = m_speed;
            m_motionController.Direction = m_direction;
        }
        
        public void KillProjectile() => 
            m_objectPool.Release(gameObject);

        public struct ProjectileMetaData {
            public IObjectPool<GameObject> Pool;
            public Vector2 Speed;
            public float Damage;
            public float LifeTime;
            public HealthManager.EntityTypes EntityType;
            public Vector2 Direction;
            
            public ProjectileMetaData(IObjectPool<GameObject> Pool, Vector2 Speed, float Damage, float LifeTime, HealthManager.EntityTypes EntityType, Vector2 Direction) {
                this.Pool = Pool;
                this.Speed = Speed;
                this.Damage = Damage;
                this.LifeTime = LifeTime;
                this.EntityType = EntityType;
                this.Direction = Direction;
            }
        }
    }
}