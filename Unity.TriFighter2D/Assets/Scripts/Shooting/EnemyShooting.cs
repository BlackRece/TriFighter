namespace BlackRece.TriFighter2D.Shooting {

    using System;
    using System.Collections.Generic;
    
    using UnityEngine;
    
    using Health;

    [Obsolete ("This class is deprecated, use ShootingController instead.")]
    public class EnemyShooting : MonoBehaviour {
        [SerializeField] private GameObject m_bulletPrefab = null;
        [SerializeField] private Transform m_bulletSpawnPoint = null;
        [SerializeField] private Vector2 m_bulletSpeed = new Vector2(5f, 0f);
        [SerializeField] private float m_shootCooldown = 0.5f;
        
        private float m_shootTimer;
        private bool m_isPaused;

        public bool IsPaused {
            get => m_isPaused;
            set => m_isPaused = value;
        }

        private void Awake() {
            new List<GameObject>();
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
            var l_projectile = l_bullet.GetComponent<ProjectileController>();
            var l_data = new ProjectileController.ProjectileMetaData {
                Speed = m_bulletSpeed,
                Damage = 1f,
                Direction = Vector2.left,
                EntityType = HealthManager.EntityTypes.Minion
            };
            l_projectile.Fire(l_data);

            //TODO: bullet pooling
            //m_bullets.Add(l_bullet);
        }
    }
}