namespace BlackRece.TriFighter2D.Movement {

    using UnityEngine;
    using RNG = UnityEngine.Random;

    [RequireComponent(typeof(MotionController))]
    public class CollectableMovement : MonoBehaviour {
        [SerializeField] private Vector2 m_speed = Vector2.one;
        [SerializeField] private float m_wanderRadius = 10f;

        private Vector2 m_direction = Vector2.zero;
        private bool m_isPaused = false;
        private MotionController m_motionController;

        public bool IsPaused {
            get => m_isPaused;
            set => m_isPaused = value;
        }

        #region Unity Functions
        
        private void Awake() {
            m_motionController = GetComponent<MotionController>();
        }

        private void Start() {
            m_direction = RNG.insideUnitCircle;
            m_speed *= 5f;
            
            //m_motionController.Direction = m_direction;
            m_motionController.Speed = m_speed;
        }

        private void Update() {
            if (m_isPaused)
                return;
            
            m_motionController.Direction = GetDirection();
        }
        
        private void OnCollisionEnter2D(Collision2D other) {
            m_direction *= -1f;
        }
        
        #endregion
        
        private Vector2 GetDirection() {
            Vector2 l_position = transform.position;
            Vector2 l_targetCenter = l_position + m_direction * m_wanderRadius;
            Vector2 l_target = l_targetCenter + RNG.insideUnitCircle;
            m_direction = (l_target - l_position).normalized;
            return m_direction;
        }
    }
}