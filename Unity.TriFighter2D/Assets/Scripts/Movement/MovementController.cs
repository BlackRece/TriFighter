namespace BlackRece.TriFighter2D.Movement {

    using UnityEngine;

    [RequireComponent(typeof(MotionController))]
    public class MovementController : MonoBehaviour {
        [SerializeField] private float m_speed = 5f;
        
        private Vector2 m_direction = Vector2.zero;
        private bool m_isPaused = false;
        private MotionController m_motionController;

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
            
            m_direction.x = Input.GetAxis("Horizontal");
            m_direction.y = Input.GetAxis("Vertical");
            m_motionController.Direction = m_direction;
        }
    }
}