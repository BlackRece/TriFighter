namespace BlackRece.TriFighter2D.Movement {

    using UnityEngine;
    
    using Events;

    [RequireComponent(typeof(MotionController))]
    public class MovementController : MonoBehaviour {
        private MotionController m_motionController;

        //private bool m_isPlayer = true;
        
        [SerializeField] private Vector2 m_speed = Vector2.one;
        
        private Vector2 m_direction = Vector2.zero;
        public Vector2 Direction { get => m_direction; set => m_direction = value; }
        
        private MovementMetaData m_movementData;
        public MovementMetaData MovementData {
            get => m_movementData;
            set {
                m_movementData = value;
                
                //m_isPlayer = m_movementData.IsPlayer;
                
                m_speed = m_movementData.Speed;
                m_direction = m_movementData.Direction;
            }
        }
        
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
            m_motionController.Speed = m_speed;
        }
        
        public struct MovementMetaData {
            //public bool IsPlayer;
            public Vector2 Direction;
            public Vector2 Speed;
        }
    }
}