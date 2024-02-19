namespace BlackRece.TriFighter2D.Movement.EnemyMovement {

    using UnityEngine;
    
    using Movement;
    using Shooting;

    [RequireComponent(typeof(MotionController), typeof(Collider2D))]
    public class EnemyMovement : MonoBehaviour {
        private MotionController m_motionController;
        
        private float m_speed = 5f;
        private Vector2 m_direction = Vector2.up;

        private bool m_isPaused = false;
        public bool IsPaused {
            get => m_isPaused;
            set => m_isPaused = value;
        }
        
        private void Awake() {
            m_motionController = GetComponent<MotionController>();
        }
        
        private void Start() {
            m_motionController.Direction = m_direction;
            
            m_motionController.Speed = m_speed;
        }
        
        private void Update() {
            if (m_isPaused)
                return;
            
            m_motionController.Direction = m_direction;
        }

        private void OnCollisionEnter2D(Collision2D other) {
            m_direction *= -1f;

            if (other.gameObject.TryGetComponent<Projectile>(out var l_projectile)) {
                Destroy(l_projectile.gameObject);
            }
        }

    }
}