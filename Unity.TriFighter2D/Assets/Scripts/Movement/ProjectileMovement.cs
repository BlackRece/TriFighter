namespace BlackRece.TriFighter2D.Movement {
    using UnityEngine;

    [RequireComponent(typeof(MotionController))]
    public class ProjectileMovement : MonoBehaviour {
        private MotionController m_motionController;

        private Vector2 m_speed = Vector2.zero;
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
            if (IsPaused)
                return;

            m_motionController.Direction = m_direction;
        }

        #endregion

        public void Init(Vector2 a_speed, Vector2 a_direction) {
            m_isPaused = false;

            m_speed = a_speed;
            m_motionController.Speed = m_speed;
            
            m_direction = a_direction;
            m_motionController.Direction = m_direction;
        }
    }
}