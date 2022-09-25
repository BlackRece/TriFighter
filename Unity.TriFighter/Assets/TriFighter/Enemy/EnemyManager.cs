using System;

using TriFighter.Types;

using UnityEngine;

using RND = UnityEngine.Random;

namespace TriFighter {
    public sealed class EnemyManager : MonoBehaviour {
        [SerializeField] private bool DEBUG = false;
        
        [SerializeField] private int _enemyPoolSize = 20;
        [SerializeField] private GameObjectPooler _enemyPooler;
        
        [SerializeField] private float _startDelay = 1f;
        [SerializeField] private float _repeatDelay = 1f;
        
        [SerializeField] private Transform _botLeft, _topRight, _botRight;

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
            if (_botLeft == null || _topRight == null || _botRight == null)
                throw new NullReferenceException("Not all [PlayArea Transform Markers] attached!");

            _enemyPooler.Init("Enemy Container", _enemyPoolSize);
            
            InvokeRepeating(nameof(Tick), _startDelay, _repeatDelay);
        }

        private Vector3 GetRandomSpawnPosition() {
            var enemyGO = _enemyPooler.GetGameObject();
            var enemyObj = enemyGO.GetComponent<IEnemy>();
            var enemySize = enemyObj.PrefabSize;
            var enemyHeight = enemySize.y;
            
            var yRange = new FloatRange(
                _botRight.position.y + enemyHeight,
                _topRight.position.y - enemyHeight
            );

            return new Vector3(
                _topRight.position.x,
                RND.Range(yRange.min, yRange.max)
            );
        }

        private void SpawnEnemy() {
            var enemy = _enemyPooler.GetGameObject();
            enemy.transform.position = GetRandomSpawnPosition();
            enemy.SetActive(true);
            
            Log("spawned enemy");
        }
        
        private void SpawnEnemy(Vector3 target) {
            var enemyObject = _enemyPooler.GetGameObject();
            enemyObject.transform.position = target;
            enemyObject.SetActive(true);

            //var enemyController = enemy.GetComponent<IEnemyController>();
            //var enemy = enemyObject.GetComponent<IEnemy>();
            
            var enemyShip = enemyObject.GetComponent<ShipController>();
            var enemyInputController = enemyShip.AIInputController;
            enemyInputController.SetMaxMoveSpeed(0.5f);
            enemyInputController.SetPlayArea(_botLeft, _topRight);
            enemyShip.SetShipToActive(true);
            
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