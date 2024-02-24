namespace BlackRece.TriFighter2D.Utils {
    using UnityEngine;
    using UnityEngine.Pool;

    public class ObjectPooler : MonoBehaviour
    {
        private const bool CollectionCheck = true;
        
        [SerializeField] private GameObject m_prefab = null;
        
        [SerializeField] private int m_poolSize = 10;
        
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

        private void OnTakeFromPool(GameObject obj) { }
        
        private void OnReturnToPool(GameObject obj) => obj.SetActive(false);
        
        private void OnDestroyPooledItem(GameObject obj) { }

    }
}
