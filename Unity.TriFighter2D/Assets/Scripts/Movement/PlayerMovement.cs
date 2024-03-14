namespace BlackRece.TriFighter2D.Movement {
    using UnityEngine;
    
    [RequireComponent(typeof(MovementController))]
    public class PlayerMovement : MonoBehaviour {
        private MovementController m_movementController;
        
        private void Awake() {
            m_movementController = GetComponent<MovementController>();
        }
        
        private void Update() {
            m_movementController.Direction = new Vector2(
                Input.GetAxis("Horizontal"), 
                Input.GetAxis("Vertical"));
        }
    }
}