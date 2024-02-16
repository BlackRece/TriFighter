namespace BlackRece.TriFighter2D.Movement {

    using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementController : MonoBehaviour {
        [SerializeField] private float m_speed = 5f;
        
        private Rigidbody2D m_rigidbody2D;
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
            m_direction = Vector2.zero;
        }

        private void Update() {
            m_direction.x = Input.GetAxis("Horizontal");
            m_direction.y = Input.GetAxis("Vertical");
        }

        private void FixedUpdate() {
            m_rigidbody2D.velocity = IsPaused
                ? Vector2.zero
                : m_direction * m_speed;
        }
    }
}