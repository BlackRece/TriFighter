using System.Collections.Generic;

using UnityEngine;

namespace TriFighter.Terrain {
    public interface IArea {
        Rect RectSize { get; }
    }

    public sealed class Area : IArea {
        private Rect _rect;
        public Rect RectSize => _rect;
        
        private GameObject _container;
        public GameObject Container => _container;

        private IWallMap _wallMap;
        public IWallMap WallMap => _wallMap;

        private bool _isVisited;

        public Area(AreaDetail detail) {
            _rect = detail.Rect;

            CreateContainer(detail.Parent);
            
            _isVisited = false;
        }

        private void CreateContainer(Transform parent) {
            _container = new GameObject();
            _container.transform.SetParent(parent);
        }
        
        public void CreateSpace(Vector2Int position) {
            _wallMap = IoC.Resolve<IWallMap>();
            _wallMap.Init(_container.transform);
        }

        public void CycleSpace() {
            _wallMap.AddWalls(new List<Vector3>());
        }
    }
}