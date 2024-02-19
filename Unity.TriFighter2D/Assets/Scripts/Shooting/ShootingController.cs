using BlackRece.TriFighter2D.Events;

namespace BlackRece.TriFighter2D.Shooting {

    using UnityEngine;
    using System.Collections.Generic;

    using Health;

    public class ShootingController : MonoBehaviour {
        [SerializeField] private GameObject m_bulletPrefab = null;
        private List<GameObject> m_bullets;
        [SerializeField] private Transform m_bulletSpawnPoint = null;
        [SerializeField] private float m_bulletSpeed = 5f;
        [SerializeField] private float m_bulletDamage = 1f;
        [SerializeField] private float m_shootCooldown = 0.5f;
        private float m_shootTimer;
        [SerializeField] private int m_maxClipSize = 30;
        private int m_currentAmmo = 0;
        [SerializeField] private float m_reloadCooldown = 2f;
        public float AmmoRatio => (float)m_currentAmmo / m_maxClipSize;
        
        private bool m_isPaused = false;

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
            m_currentAmmo = m_maxClipSize;
            EventManager.InvokeEvent(EventIDs.OnAddAmmo, AmmoRatio);
        }

        private void Update() {
            if (m_isPaused)
                return;

            if (m_shootTimer > 0f)
                m_shootTimer -= Time.deltaTime;

            if (Input.GetButton("Fire1") && m_shootTimer <= 0f)
                Shoot();
            
            if (Input.GetKeyDown(KeyCode.R))
                Reload();
        }

        private void Shoot() {
            if (m_currentAmmo <= 0) {
                Reload();
                return;
            }
            
            m_currentAmmo--;
            m_shootTimer = m_shootCooldown;

            var l_bullet = Instantiate(m_bulletPrefab, m_bulletSpawnPoint.position, Quaternion.identity);
            var l_projectile = l_bullet.GetComponent<Projectile>();
            var l_data = new Projectile.ProjectileMetaData {
                Speed = m_bulletSpeed,
                Damage = m_bulletDamage,
                Direction = Vector2.right,
                EntityType = HealthManager.EntityTypes.Player
            };
            l_projectile.Fire(l_data);

            EventManager.InvokeEvent(EventIDs.OnDelAmmo, 1);
            
            //TODO: bullet pooling
            //m_bullets.Add(l_bullet);
        }
        
        private void Reload() {
            if (m_currentAmmo == m_maxClipSize)
                return;

            m_shootTimer = m_reloadCooldown;
            m_currentAmmo = m_maxClipSize;
            
            EventManager.InvokeEvent(EventIDs.OnAddAmmo, AmmoRatio);
        }
    }
}