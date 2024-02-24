namespace BlackRece.TriFighter2D.SkyBox {

    using System.Collections.Generic;
    using UnityEngine;

    public class ScrollingSkyBox : MonoBehaviour {
        [SerializeField] private float m_speed = 0.5f;
        [SerializeField] private float m_spacing = 39.98f;
        private List<GameObject> m_skyBoxGOs;
        private List<Bounds> m_skyBoxSizes;

        private bool m_isPaused;

        public bool IsPaused {
            get => m_isPaused;
            set => m_isPaused = value;
        }
        
        private void Awake() {
            if(transform.childCount == 0)
                Debug.LogError("No skybox game objects found!");
        }

        private void Start() {
            IsPaused = false;

            m_skyBoxGOs = new List<GameObject>();
            m_skyBoxSizes = new List<Bounds>();
            
            var l_spacing = 0f;
            
            foreach (Transform l_child in transform) {
                m_skyBoxGOs.Add(l_child.gameObject);
                
                Bounds l_bgSize = l_child.GetComponent<SpriteRenderer>().bounds;
                m_skyBoxSizes.Add(l_bgSize);
                
                l_child.position = new Vector3(l_spacing, 0f, 0f);
                l_spacing += l_bgSize.size.x;
            }
        }

        private void Update() {
            if (IsPaused)
                return;
            
            for(var i = 0; i < m_skyBoxGOs.Count; i++) {
                m_skyBoxGOs[i].transform.Translate(Vector2.left * (m_speed * Time.deltaTime));
                if (m_skyBoxGOs[i].transform.position.x < -m_spacing) {
                    m_skyBoxGOs[i].transform.position = 
                        Vector3.right * (m_skyBoxSizes[i].size.x * (m_skyBoxGOs.Count - 1));
                }
            }
        }
    }
}