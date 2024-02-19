namespace BlackRece.TriFighter2D.Collectables {

    using UnityEngine;
    using RNG = UnityEngine.Random;

    // NOTE: Obviously, if the object that this is attached to is disabled or
    // removed from the scene, the spawner will stop working.

    public class CollectableSpawner : MonoBehaviour {
        [SerializeField] private CollectablePickUp m_collectablePrefab = null;
        
        [SerializeField] private int m_spawnAmount = 5;
        [SerializeField] private float m_spawnRate = 1f;
        [SerializeField] private float m_spawnRadius = 1f;
        private float m_spawnTimer = 0f;
        
        [SerializeField] private CollectableType m_collectableType = CollectableType.None;
        [SerializeField] private int m_rewardAmount = 0;
        [SerializeField] private bool m_repeatSpawn = false;

        
        private bool m_isPaused = false;
        public bool IsPaused {
            get => m_isPaused;
            set => m_isPaused = value;
        }

        private void Update() {
            if (m_isPaused)
                return;

            m_spawnTimer += Time.deltaTime;

            if (m_spawnTimer >= m_spawnRate && m_spawnAmount > 0 && m_repeatSpawn) {
                m_spawnTimer = 0f;
                m_spawnAmount--;
                
                SpawnCollectable();
            }
            
            if(m_spawnAmount <= 0)
                m_repeatSpawn = false;
        }
        
        private void SpawnCollectable() {
            Vector2 l_spawnPosition = (Vector2) transform.position + RNG.insideUnitCircle * m_spawnRadius;
            
            var l_type = m_collectableType == CollectableType.None 
                ? (CollectableType)RNG.Range(1, 3) 
                : m_collectableType;
            
            var l_collectable = Instantiate(m_collectablePrefab.gameObject, l_spawnPosition, Quaternion.identity);
            l_collectable
                .GetComponent<CollectablePickUp>()
                .Init(l_type, m_rewardAmount);
        }
    }
}