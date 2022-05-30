using UnityEngine;

namespace TriFighter {
    public sealed class EnemyManager : MonoBehaviour {
        [SerializeField] private int _enemyPoolSize = 20;
        [SerializeField] private GameObjectPooler _enemyPooler;

        private bool _spawned = false;
        
        private float _startDelay = 1f;
        private float _repeatDelay = 1f;

        private void Awake() {
            _enemyPooler.Init("Enemy Container", _enemyPoolSize);
            
            InvokeRepeating(nameof(Tick), _startDelay, _repeatDelay);
        }

        private Vector3 GetRandomSpawnPosition() {
            var worldRect = TargetChecker.PlayArea;

            return new Vector3(
                worldRect.xMax,
                Random.Range(worldRect.yMin, worldRect.yMax)
            );
        }

        private void SpawnEnemy() {
            var enemy = _enemyPooler.GetGameObject();
            enemy.transform.position = GetRandomSpawnPosition();
            enemy.SetActive(true);
            
            Debug.Log("spawned enemy");
        }

        private void SpawnLineOfEnemies(int amountOfEnemies, float spacing = 2f) {
            var spawnPosition = GetRandomSpawnPosition();
            
            for (var i = 0; i < amountOfEnemies; i++) {
                var offsetPosition = new Vector3(
                    spawnPosition.x + spacing * i,
                    spawnPosition.y
                );

                var enemy = _enemyPooler.GetGameObject();
                enemy.transform.position = offsetPosition;
                enemy.SetActive(true);
            }
            
            Debug.Log("spawned line of enemies");
        }

        private void Tick() {
            if (!_spawned) {
                //SpawnEnemy();
                SpawnLineOfEnemies(5);
                _spawned = true;
            }
        }
    }
}