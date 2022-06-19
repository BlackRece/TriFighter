using System;

using UnityEngine;

using Random = UnityEngine.Random;

namespace TriFighter {
    public sealed class EnemyManager : MonoBehaviour {
        [SerializeField] private bool DEBUG = false;
        
        [SerializeField] private int _enemyPoolSize = 20;
        [SerializeField] private GameObjectPooler _enemyPooler;
        
        [SerializeField] private float _startDelay = 1f;
        [SerializeField] private float _repeatDelay = 1f;
        
        [SerializeField] private Transform _topRight, _botRight;

        private bool _spawned = false;

        public void OnPlayerPositionBroadCast(Vector3 position) {
            var activeEnemies = _enemyPooler.GetActiveObjects();
            foreach (var enemyGO in activeEnemies) {
                if (!enemyGO.TryGetComponent(out AIInputController enemyAI))
                    continue;

                enemyAI.UpdateTargetPosition(position);
            }
        }
        
        private void Awake() {
            if (_topRight == null || _botRight == null)
                throw new NullReferenceException("Not all [PlayArea Transform Markers] attached!");

            _enemyPooler.Init("Enemy Container", _enemyPoolSize);
            
            InvokeRepeating(nameof(Tick), _startDelay, _repeatDelay);
        }

        private Vector3 GetRandomSpawnPosition() {
            return new Vector3(
                _topRight.position.x,
                Random.Range(_topRight.position.y + 1, _botRight.position.y - 1)
            );
        }

        private void SpawnEnemy() {
            var enemy = _enemyPooler.GetGameObject();
            enemy.transform.position = GetRandomSpawnPosition();
            enemy.SetActive(true);
            
            Log("spawned enemy");
        }
        
        private void SpawnEnemy(Vector3 target) {
            var enemy = _enemyPooler.GetGameObject();
            enemy.transform.position = target;
            enemy.SetActive(true);

            var enemyAI = enemy.GetComponent<ShipController>();
            enemyAI.AIInputController.SetMaxMoveSpeed(0.5f);
            
            Log("spawned enemy");
        }

        private void SpawnLineOfEnemies(int amountOfEnemies, float spacing = 2f) {
            for (var i = 0; i < amountOfEnemies; i++) {
                var spawnPosition = GetRandomSpawnPosition();
            
                var offsetPosition = new Vector3(
                    spawnPosition.x + spacing * i,
                    spawnPosition.y
                );

                SpawnEnemy(offsetPosition);
            }
            
            Log("spawned line of enemies");
        }

        private void Tick() {
            if (!_spawned) {
                //SpawnEnemy();
                SpawnLineOfEnemies(5);
                _spawned = true;
            }
        }
        
        private void Log(string msg) {
            if(DEBUG) Debug.Log(msg);
        }
    }
}