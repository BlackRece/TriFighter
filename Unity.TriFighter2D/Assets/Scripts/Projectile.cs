using System;

using BlackRece.TriFighter2D.Movement;

namespace BlackRece.TriFighter2D.Shooting {
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour {
        private Rigidbody2D m_rigidbody2D;
        
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
            if (other.gameObject.TryGetComponent<MovementController>(out var l_player))
                return;
            
            Destroy(gameObject);
        }

        public void Fire(ProjectileMetaData p_metaData) {
            m_speed = p_metaData.speed;
            m_direction = p_metaData.direction;
            m_damage = p_metaData.damage;
            m_lifeTimer = m_lifeTimeMax;
        }
        
        public struct ProjectileMetaData {
            public float speed;
            public float damage;
            public Vector2 direction;
            public ProjectileMetaData(float speed, float damage, Vector2 direction) {
                this.speed = speed;
                this.damage = damage;
                this.direction = direction;
            }
        }
    }
}