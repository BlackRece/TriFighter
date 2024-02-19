namespace BlackRece.TriFighter2D.Movement {
    using UnityEngine;

    [RequireComponent(typeof(MotionController))]
    public class ProjectileMovement : MonoBehaviour {
        private MotionController m_motionController;

        private float m_speed = 0f;
        private Vector2 m_direction = Vector2.zero;

        private bool m_isPaused;

        public bool IsPaused {
            get => m_isPaused;
            set => m_isPaused = value;
        }

        #region Unity Functions

        private void Awake() {
            m_motionController = GetComponent<MotionController>();
        }

        private void Update() {
            if (m_isPaused)
                return;

            m_motionController.Direction = m_direction;
        }

        #endregion

        public void Init(float a_speed, Vector2 a_direction) {
            m_isPaused = false;

            m_speed = a_speed;
            m_motionController.Speed = m_speed;
            
            m_direction = a_direction;
            m_motionController.Direction = m_direction;
        }
    }
}