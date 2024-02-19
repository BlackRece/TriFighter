namespace BlackRece.TriFighter2D.Movement {

    using UnityEngine;

    [CreateAssetMenu(fileName = "Boundary", menuName = "TriFighter2D/Boundary", order = 0)]
    public class Boundary : ScriptableObject {
        public static Transform m_min;
        public static Transform m_max;
        
        public static bool Contains(Vector2 p_position) =>
            p_position.x >= m_min.position.x && p_position.x <= m_max.position.x && 
            p_position.y >= m_min.position.y && p_position.y <= m_max.position.y;
    }
}