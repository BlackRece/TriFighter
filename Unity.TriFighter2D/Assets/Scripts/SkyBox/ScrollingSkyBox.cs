namespace BlackRece.TriFighter2D.SkyBox {

    using System.Collections.Generic;
    using UnityEngine;

    public class ScrollingSkyBox : MonoBehaviour {
        [SerializeField] private float m_speed = 0.5f;
        [SerializeField] private float m_spacing = 39.98f;
        private List<GameObject> m_skyBox;
        

        private void Awake() {
            if(transform.childCount == 0)
                Debug.LogError("No skybox game objects found!");
        }

        private void Start() {
            m_skyBox = new List<GameObject>();
            var l_spacing = 0f;
            foreach (Transform l_child in transform) {
                l_child.position = new Vector3(l_spacing, 0f, 0f);
                m_skyBox.Add(l_child.gameObject);
                l_spacing += m_spacing;
            }
        }

        private void Update() {
            foreach (GameObject l_bg in m_skyBox) {
                l_bg.transform.Translate(Vector2.left * (m_speed * Time.deltaTime));
                var l_pos = l_bg.transform.position;
                if (l_pos.x < -m_spacing) {
                    l_bg.transform.position = Vector3.right * (m_spacing * (m_skyBox.Count - 1));
                }
            }
        }
    }
}