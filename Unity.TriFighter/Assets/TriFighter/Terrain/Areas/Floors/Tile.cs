using System.Collections.Generic;

using UnityEngine;

namespace TriFighter.Terrain {
    public interface ITile {
        GameObject GetGameObject { get; }
        Transform GetTransform { get; }
        void SetParent(ITileMap parentTileMap);
        
        bool HasChildren { get; }

        Vector2Int GetMapPosition();
        Vector3 GetTopPosition();
        Vector3 GetWorldPosition(Vector2Int mapPosition);
        
        void FlagAsVisited();
    }

    [RequireComponent(typeof(Renderer))]
    public sealed class Tile : MonoBehaviour, ITile {
        private Vector3 _modelSize;
        
        public GameObject GetGameObject => gameObject;
        public Transform GetTransform => transform;

        public bool HasChildren => transform.childCount > 0;
        private bool _isVisited;
        private ITileMap _parentTileMap;

        private void Awake() {
            _modelSize = GetComponent<Renderer>().bounds.size;
        }

        public Vector3 GetWorldPosition(Vector2Int mapPosition) =>
            new Vector3(mapPosition.x * _modelSize.x, 0, mapPosition.y * _modelSize.z);

        public Vector2Int GetMapPosition() {
            var position = transform.position;
            return new Vector2Int(
                (int) (position.x / _modelSize.x),
                (int) (position.z / _modelSize.z));
        }

        public Vector3 GetTopPosition() {
            var position = transform.position;
            position.y += _modelSize.y / 2;
            return position;
        }
        
        public void SetParent(ITileMap parentTileMap) {
            _parentTileMap = parentTileMap;
        }

        public void FlagAsVisited() {
            if (_isVisited)
                return;
            
            _isVisited = true;

            if (!_parentTileMap.IsVisited)
                _parentTileMap.FlagAsVisited();
        }
    }
}