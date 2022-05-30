using System.Collections.Generic;

using UnityEngine;

namespace TriFighter {
    interface IGameObjectPooler {
        void Init(string containerName, int initialAmount = 10);
        GameObject GetGameObject();
    }
    
    [CreateAssetMenu(fileName = "GameObjectPooler", menuName = "New GameObject Pooler" /*, order = 0*/)]
    public class GameObjectPooler : ScriptableObject, IGameObjectPooler {
        [SerializeField] private GameObject _prefab;
        private GameObject _container;
        private List<GameObject> _pool;
        
        private void OnDestroy() {
            Destroy(_container);
        }

        public void Init(string containerName, int initialAmount = 10) {
            _container = new GameObject();
            _pool = new List<GameObject>();
            
            _container.name = containerName;

            for (var i = 0; i < initialAmount; i++) {
                _pool.Add(CreateGameObjectInstance());
            }
        }

        public GameObject GetGameObject() {
            foreach (var inactiveObject in _pool) 
                if (!inactiveObject.activeSelf) 
                    return inactiveObject;

            var cleanGameObject = CreateGameObjectInstance();
            _pool.Add(cleanGameObject);
            
            return cleanGameObject;
        }

        private GameObject CreateGameObjectInstance() {
            var instance = Instantiate(_prefab, _container.transform);
            instance.SetActive(false);
            return instance;
        }
    }
}