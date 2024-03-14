namespace BlackRece.TriFighter2D.Utils {
    using UnityEngine;
    using UnityEngine.Pool;

    using Managers;
    
    public class ObjectPooler : MonoBehaviour
    {
        private const bool CollectionCheck = false;
        
        [SerializeField] private GameObject m_prefab = null;
        
        [SerializeField] private int m_poolSize = 10;

        [SerializeField] private string m_poolName = "ObjectPool";
        public string PoolName => m_poolName;
        
        IObjectPool<GameObject> m_objectPool;
        public IObjectPool<GameObject> Pool => m_objectPool;
        
        private void Awake() {
            m_objectPool = new ObjectPool<GameObject>(
                CreatePooledItem,
                OnTakeFromPool,
                OnReturnToPool,
                OnDestroyPooledItem,
                CollectionCheck,
                m_poolSize);
        }
        
        private GameObject CreatePooledItem() => Instantiate(m_prefab);

        private void OnTakeFromPool(GameObject obj) => obj.SetActive(true);
        
        private void OnReturnToPool(GameObject obj) => obj.SetActive(false);
        
        private void OnDestroyPooledItem(GameObject obj) => Destroy(obj);

    }
}
