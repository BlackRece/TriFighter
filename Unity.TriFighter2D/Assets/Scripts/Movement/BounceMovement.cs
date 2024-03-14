namespace BlackRece.TriFighter2D.Movement {
    using UnityEngine;

    using Events;
    
    [RequireComponent(typeof(MovementController))]
    public class BounceMovement : MonoBehaviour {
        private MovementController m_movementController;
        
        private void Awake() {
            m_movementController = GetComponent<MovementController>();
        }

        private void Update() {
            RaycastHit2D l_hitDown = Physics2D.Raycast(transform.position, Vector2.down);
            if (l_hitDown.collider != null)
                m_movementController.Direction = Vector2.up;
            
            RaycastHit2D l_hitUp = Physics2D.Raycast(transform.position, Vector2.up);
            if (l_hitUp.collider != null) 
                m_movementController.Direction = Vector2.down;
            
        }

        private void OnBecameInvisible() {
            EventManager.InvokeEvent(EventIDs.OnEnemyOutOfBounds, gameObject);
        }
    }
}