namespace BlackRece.TriFighter2D.Shooting {

    using UnityEngine;
    using System.Collections.Generic;
    
    using Health;

    public class EnemyShooting : MonoBehaviour {
        [SerializeField] private GameObject m_bulletPrefab = null;
        private List<GameObject> m_bullets;
        [SerializeField] private Transform m_bulletSpawnPoint = null;
        [SerializeField] private float m_bulletSpeed = 5f;
        [SerializeField] private float m_shootCooldown = 0.5f;
        
        private float m_shootTimer;
        private bool m_isPaused;

        public bool IsPaused {
            get => m_isPaused;
            set => m_isPaused = value;
        }

        private void Awake() {
            m_bullets = new List<GameObject>();
            if (m_bulletPrefab == null)
                Debug.LogError("Bullet prefab is not set!");
        }

        private void Start() {
            IsPaused = false;
            m_bulletSpawnPoint = transform;
        }

        private void Update() {
            if (IsPaused)
                return;

            if (m_shootTimer > 0f)
                m_shootTimer -= Time.deltaTime;

            if (m_shootTimer <= 0f)
                Shoot();
        }

        private void Shoot() {
            m_shootTimer = m_shootCooldown;

            var l_bullet = 
                Instantiate(m_bulletPrefab, m_bulletSpawnPoint.position, Quaternion.identity);
            var l_projectile = l_bullet.GetComponent<Projectile>();
            var l_data = new Projectile.ProjectileMetaData {
                Speed = m_bulletSpeed,
                Damage = 1f,
                Direction = Vector2.left,
                EntityType = HealthManager.EntityTypes.Enemy
            };
            l_projectile.Fire(l_data);

            //TODO: bullet pooling
            //m_bullets.Add(l_bullet);
        }
    }
}