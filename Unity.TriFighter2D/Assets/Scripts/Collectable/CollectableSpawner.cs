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
                //.AddComponent<CollectablePickUp>()
                .Init(l_type, m_rewardAmount);
        }
        
        private void SpawnCollectable_V2() {
            // set spawn position
            Vector2 l_spawnPosition = (Vector2) transform.position + RNG.insideUnitCircle * m_spawnRadius;
            
            // set collectable type
            var l_type = m_collectableType == CollectableType.None 
                ? (CollectableType)RNG.Range(1, 3) 
                : m_collectableType;
            
            /*
             * what makes more sense?
             * 1. Instantiate the prefab and then set the sprite and amount
             * 2. Instantiate the prefab that already has a sprite and set the amount
             */
            // collectable prefab must have a sprite renderer
            var l_collectable = Instantiate(m_collectablePrefab.gameObject, l_spawnPosition, Quaternion.identity);

            switch (m_collectableType) {
                case CollectableType.Health:
                    l_collectable
                        .AddComponent<HealthPickup>();
                        //.Init(l_type, m_rewardAmount);
                    break;
                case CollectableType.Ammo:
                    break;
                case CollectableType.Experience:
                    break;
            }
            
            /*l_collectable
                .GetComponent<CollectablePickUp>()
                .Init(l_type, m_rewardAmount);*/
        }
    }
}