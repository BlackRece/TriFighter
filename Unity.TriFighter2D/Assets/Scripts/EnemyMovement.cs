namespace BlackRece.TriFighter2D.EnemyMovement {

    using UnityEngine;
    using Shooting;

    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMovement : MonoBehaviour {
        private Rigidbody2D m_rigidbody2D;
        private float m_speed = 5f;
        private Vector2 m_direction;

        private bool m_isPaused;
        public bool IsPaused {
            get => m_isPaused;
            set => m_isPaused = value;
        }
        
        private void Awake() {
            m_rigidbody2D = GetComponent<Rigidbody2D>();
        }
        
        private void Start() {
            m_isPaused = false;
            m_direction = Vector2.up;
        }

        private void FixedUpdate() {
            m_rigidbody2D.velocity = m_isPaused
                ? Vector2.zero
                : m_direction * m_speed;
        }

        private void OnCollisionEnter2D(Collision2D other) {
            m_direction *= -1f;

            if (other.gameObject.TryGetComponent<Projectile>(out var l_projectile)) {
                Destroy(l_projectile.gameObject);
            }
        }

    }
}