using System;

namespace BlackRece.TriFighter2D.Movement {

    using UnityEngine;

    public class PlayArea : MonoBehaviour {
        [SerializeField] private Transform m_min;
        [SerializeField] private Transform m_max;

        public bool Contains(Vector2 p_position) =>
            p_position.x >= m_min.position.x && p_position.x <= m_max.position.x && 
            p_position.y >= m_min.position.y && p_position.y <= m_max.position.y;
    }
    
    [Serializable]
    public static class BoundsChecker 
    {
        private static PlayArea m_playArea = null;
        
        public static bool Contains(Vector2 point)
        {
            if (m_playArea == null)
                m_playArea = GameObject.FindObjectOfType<PlayArea>();

            return m_playArea.Contains(point);
        }
    }
}