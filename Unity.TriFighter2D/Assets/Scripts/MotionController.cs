namespace BlackRece.TriFighter2D.Movement {
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D))]
    public class MotionController : MonoBehaviour {
        private Rigidbody2D m_rigidbody2D = null;
        
        [SerializeField] private float m_speed = 5f;
        public float Speed {
            get => m_speed;
            set => m_speed = value;
        }
        
        private Vector2 m_direction = Vector2.zero;
        public Vector2 Direction {
            get => m_direction;
            set => m_direction = value;
        }
        
        private bool m_isPaused = false;
        public bool IsPaused {
            get => m_isPaused;
            set => m_isPaused = value;
        }
        
        private void Awake() {
            m_rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() {
            m_rigidbody2D.velocity = IsPaused
                ? Vector2.zero
                : m_direction * m_speed;
        }
    }
}