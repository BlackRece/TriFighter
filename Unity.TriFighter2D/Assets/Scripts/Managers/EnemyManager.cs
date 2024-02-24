namespace BlackRece.TriFighter2D.Managers
{
    using System.Collections.Generic;
    
    using UnityEngine;

    using Events;

    public class EnemyManager : MonoBehaviour
    {
        /*
         * Purpose: to contain a pool of enemies and manage their spawning and despawning
         */
        [SerializeField] private GameObject m_enemyPrefab = null;
        [SerializeField] private int m_poolSize = 10;
        private List<GameObject> m_enemyPool;
        
        private void Awake() {
            m_enemyPool = new List<GameObject>();
            for (var i = 0; i < m_poolSize; i++) {
                var l_enemy = Instantiate(m_enemyPrefab, transform);
                l_enemy.SetActive(false);
                m_enemyPool.Add(l_enemy);
            }
            
            EventManager.AddEvent<Vector3>(EventIDs.OnSpawnEnemy);
            //EventManager.AddEvent<Vector3>(EventIDs.OnDespawnEnemy);
            EventManager.AddEvent<GameObject>(EventIDs.OnDeath);
        }
        
        private void OnEnable() {
            //EventManager.AddListener<Vector3>(EventIDs.OnSpawnEnemy, SpawnEnemy);
            //EventManager.AddListener<Vector3>(EventIDs.OnDespawnEnemy, DespawnEnemy);
            EventManager.AddListener<GameObject>(EventIDs.OnDeath, DespawnEnemy);
        }
        
        private void OnDisable() {
            //EventManager.RemoveListener<Vector3>(EventIDs.OnSpawnEnemy, SpawnEnemy);
            //EventManager.RemoveListener<Vector3>(EventIDs.OnDespawnEnemy, DespawnEnemy);
            EventManager.RemoveListener<GameObject>(EventIDs.OnDeath, DespawnEnemy);
        }
        
        private void SpawnEnemy(Vector3 a_position) {
            var l_enemy = GetEnemy();
            l_enemy.transform.position = a_position;
            l_enemy.SetActive(true);
        }
        
        private void DespawnEnemy(GameObject a_enemy) {
            a_enemy.SetActive(false);
        }

        private GameObject GetEnemy() {
            foreach (GameObject l_enemy in m_enemyPool) {
                if(!l_enemy.activeInHierarchy)
                    return l_enemy;
            }
            
            AddEnemies();
            return GetEnemy();
        }

        private void AddEnemies() {
            for (var i = 0; i < m_poolSize; i++) {
                GameObject l_enemy = Instantiate(m_enemyPrefab, transform);
                l_enemy.SetActive(false);
                m_enemyPool.Add(l_enemy);
            }
        }
    }
}
