namespace BlackRece.TriFighter2D.Collectables {

    using UnityEngine;

    public class CollectableSpinner : MonoBehaviour {
        [SerializeField] private float m_speed = 5f;
        private GameObject m_sprite = null;
        
        private bool m_isPaused;
        public bool IsPaused {
            get => m_isPaused;
            set => m_isPaused = value;
        }
        
        private void Awake() {
            // try and get the attached sprite game object
            m_sprite = transform.childCount > 0 
                ? transform.GetChild(0).gameObject 
                : null;
            
            if(m_sprite == null)
                this.enabled = false;
        }

        void Update() {
            if(m_isPaused)
                return;
            
            m_sprite.transform.Rotate(Vector3.forward, m_speed * Time.deltaTime);
        }
    }
}