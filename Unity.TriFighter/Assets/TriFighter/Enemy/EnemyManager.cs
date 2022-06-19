using UnityEngine;

namespace TriFighter {
    public sealed class EnemyManager : MonoBehaviour {
        [SerializeField] private bool DEBUG = false;
        [SerializeField] private int _enemyPoolSize = 20;
        [SerializeField] private GameObjectPooler _enemyPooler;
        
        [SerializeField] private float _startDelay = 1f;
        [SerializeField] private float _repeatDelay = 1f;

        private bool _spawned = false;
        
        private void Awake() {
            _enemyPooler.Init("Enemy Container", _enemyPoolSize);
            
            InvokeRepeating(nameof(Tick), _startDelay, _repeatDelay);
        }

        private Vector3 GetRandomSpawnPosition() {
            var worldRect = CameraController.PlayArea;

            return new Vector3(
                worldRect.xMax,
                Random.Range(worldRect.yMin, worldRect.yMax)
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
            
            Log("spawned enemy");
        }

        private void SpawnLineOfEnemies(int amountOfEnemies, float spacing = 2f) {
            var spawnPosition = GetRandomSpawnPosition();
            
            for (var i = 0; i < amountOfEnemies; i++) {
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