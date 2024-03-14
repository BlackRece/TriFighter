using System;

namespace BlackRece.TriFighter2D.Movement.EnemyMovement {

    using UnityEngine;
    using UnityEngine.Tilemaps;
    
    using Events;
    using Movement;
    
    [Obsolete ("This class is obsolete. Use the MovementController class instead.")]
    [RequireComponent(
        typeof(MotionController), 
        typeof(Collider2D))]
    public class EnemyMovement : MonoBehaviour {
        private MotionController m_motionController;
        
        private Vector2 m_speed = Vector2.one;
        private Vector2 m_direction = Vector2.up;

        private MovementMetaData m_movementData;
        public MovementMetaData MovementData {
            get => m_movementData;
            set {
                m_movementData = value;
                
                m_speed = m_movementData.Speed;
                m_direction = m_movementData.Direction;
            }
        }

        private bool m_isPaused = false;
        public bool IsPaused {
            get => m_isPaused;
            set => m_isPaused = value;
        }
        
        // private void Awake() {
        //     m_motionController = GetComponent<MotionController>();
        // }
        //
        // private void Start() {
        //     // TODO: decide how movement will be handled. FuSM? Steering Behaviors?
        //     m_motionController.Direction = m_direction;
        //     
        //     m_motionController.Speed = m_speed;
        // }
        //
        // private void Update() {
        //     if (m_isPaused)
        //         return;
        //     
        //     
        //     
        //     m_motionController.Direction = m_direction;
        // }

        private void OnCollisionEnter2D(Collision2D other) {
            m_direction *= -1f;

            // swap between up and down on collision with a tilemap
            if (other.collider is TilemapCollider2D) {
                
            }

            // if (other.gameObject.TryGetComponent<Projectile>(out var l_projectile)) {
            //     Destroy(l_projectile.gameObject);
            // }
        }

        // private void OnBecameInvisible() {
        //     EventManager.InvokeEvent(EventIDs.OnEnemyOutOfBounds, gameObject);
        // }

        public struct MovementMetaData {
            public Vector2 Direction;
            public Vector2 Speed;
        }
    }
}