namespace BlackRece.TriFighter2D.Managers {
    using System.Collections.Generic;
    
    using UnityEngine;

    using Events;
    using Health;
    using Movement;
    using Shooting;
    using Utils;
    
    using RNG = UnityEngine.Random;

    [RequireComponent(typeof(ObjectPooler))]
    public class EnemyManager : MonoBehaviour
    {
        private GameObject m_player;
        
        /* Constants for the pool IDs
         * quick and dirty method of having consistent pool IDs
         * since Unity doesn't allow for string type enums */
        private const string MinionID = "Minion"; 
        private const string BossID = "Boss"; 
        [SerializeField] private List<string> m_poolIDs = new() { MinionID, BossID };
        
        private Dictionary<string, ObjectPooler> m_pools;
        
        [SerializeField] private float m_baseMinionHealth = 10f;

        private int m_minionCount;
        private int m_minionLimit = 1;
        [SerializeField] private float m_minionSpawnDelay = 1f;
        private float m_minionSpawnTimer;
        
        private bool m_isPaused;

        public bool IsPaused {
            get => m_isPaused;
            set => m_isPaused = value;
        }
        
        private void Awake() {
            // i hate this and it will break when we get fancy
            // adding the player to the current scene
            m_player = GameObject.FindGameObjectWithTag("Player");
            
            m_minionCount = 0;
            
            m_pools = new Dictionary<string, ObjectPooler>();
            var l_pools = GetComponents<ObjectPooler>();
            foreach (var l_pool in  l_pools) {
                if (m_poolIDs.Contains(l_pool.PoolName))
                    m_pools.Add(l_pool.PoolName, l_pool);
                else
                    Debug.LogWarning(
                        $"The ObjectPooler component's name {l_pool.PoolName}" +
                        "does not exist within the name: {l_poolID}");
            }
            
            EventManager.AddEvent<Vector3>(EventIDs.OnSpawnMinion);
            EventManager.AddEvent<Vector3>(EventIDs.OnSpawnBoss);
            EventManager.AddEvent<GameObject>(EventIDs.OnMinionDeath);
            EventManager.AddEvent<GameObject>(EventIDs.OnBossDeath);
            EventManager.AddEvent<GameObject>(EventIDs.OnEnemyOutOfBounds);
        }
        
        private void OnEnable() {
            //EventManager.AddListener<Vector3>(EventIDs.OnSpawnEnemy, SpawnEnemy);
            //EventManager.AddListener<Vector3>(EventIDs.OnDespawnEnemy, DespawnEnemy);
            EventManager.AddListener<GameObject>(EventIDs.OnMinionDeath, KillMinion);
            EventManager.AddListener<GameObject>(EventIDs.OnBossDeath, KillBoss);
            EventManager.AddListener<GameObject>(EventIDs.OnEnemyOutOfBounds, OnEnemyOutOfBounds);
        }

        private void OnDisable() {
            //EventManager.RemoveListener<Vector3>(EventIDs.OnSpawnEnemy, SpawnEnemy);
            //EventManager.RemoveListener<Vector3>(EventIDs.OnDespawnEnemy, DespawnEnemy);
            EventManager.RemoveListener<GameObject>(EventIDs.OnMinionDeath, KillMinion);
            EventManager.RemoveListener<GameObject>(EventIDs.OnBossDeath, KillBoss);
        }

        private void Update() {
            if (m_isPaused)
                return;
            
            if(Input.GetKeyDown(KeyCode.Alpha1))
                SpawnMinion(transform.position);

            // want to implement a type of wave system. spawn minions up to a limit, wait for them to die, then spawn an increased number of minions and repeat
            if (m_minionCount < m_minionLimit)
                if (m_minionSpawnDelay <= 0f)
                    SpawnMinion(transform.position);
                else
                    m_minionSpawnDelay -= Time.deltaTime;
            else if (m_minionCount == 0)
                m_minionLimit++;
        }

        private void SpawnMinion(Vector3 a_position) {
            // get a minion from the pool
            GameObject l_minion = m_pools[MinionID].Pool.Get();
            l_minion.transform.position = a_position;
            
            /***
             * enemies fire in the wrong direction
             * enemies don't move
             *
             * collision response handled in separate class
             ***/
            
            // set minion's parameters
            // health
            if (l_minion.TryGetComponent(out HealthManager l_healthManager)) {
                l_healthManager.Init(HealthManager.EntityTypes.Minion);
                l_healthManager.CurrentHealth = m_baseMinionHealth;
            }
            
            // movement
            if (l_minion.TryGetComponent(out MovementController l_movementController)) {
                Vector2 l_direction = (RNG.value * 10) % 2 == 0
                    ? Vector2.up
                    : Vector2.down;
                var l_movementData = new MovementController.MovementMetaData {
                    //IsPlayer = false,
                    Direction = Vector2.left + l_direction,
                    Speed = new Vector2(1f, 5f)
                }; 
                l_movementController.MovementData = l_movementData;
                l_minion.AddComponent(typeof(BounceMovement));
            }

            // shooting
            if (l_minion.TryGetComponent(out ShootingController l_shootingController)) {
                // TODO: get shooting parameters from a scriptable object
                l_shootingController.ProjectileMetaData = new ProjectileController.ProjectileMetaData {
                    Pool = l_shootingController.Pooler.Pool,
                    Speed = new Vector2(10f, 0f),
                    Damage = 1f,
                    LifeTime = 10f,
                    Direction = Vector2.left,
                    EntityType = HealthManager.EntityTypes.Minion
                };
            }
            
            // keep track of the minion count and reset the spawn delay
            m_minionCount++;
            m_minionSpawnDelay = m_minionSpawnTimer;
            
            // TODO: setup collectables for minions
            
            EventManager.InvokeEvent(EventIDs.OnSpawnMinion, a_position);
        }
        
        private void SpawnBoss(Vector3 a_position) {
            GameObject l_boss = m_pools[BossID].Pool.Get();
            l_boss.transform.position = a_position;
            l_boss.SetActive(true);
            
            // set boss's parameters
            var l_enemy =  l_boss.GetComponent<HealthManager>();
            l_enemy.Init(HealthManager.EntityTypes.Boss);
            
            // TODO: setup collectables for bosses
            
            EventManager.InvokeEvent(EventIDs.OnSpawnBoss, a_position);
        }
        
        private void KillMinion(GameObject a_enemy) {
            m_pools[MinionID].Pool.Release(a_enemy);
            m_minionCount--;
        }

        private void KillBoss(GameObject a_enemy) => 
            m_pools[BossID].Pool.Release(a_enemy);

        private void OnEnemyOutOfBounds(GameObject a_enemy) => 
            a_enemy.transform.position = transform.position;
    }
}
