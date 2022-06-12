using System.Collections.Generic;

using TriFighter.Types;

using UnityEngine;

namespace TriFighter.Terrain {
    public interface IWallMap {
        void Init(Transform parent, FloatRange width);
        void AddBoundary(Vector3 wallPosition);
        void AddWalls(Vector3 wallPosition);
        void SetContainerPosition(Vector2Int pos);
        void TranslateBy();
    }
    
    public sealed class WallMap : ScriptableObject, IWallMap {
        private const int max = 10;
        private Transform _parent;
        private string _name;

        private FloatRange _width;
        private int _colCount;
        
        private Dictionary<int, IWall> _walls;
        private Vector2Int _size;
        
        private GameObject _container;
        public GameObject WallContainer => _container;

        public void Init(Transform parent, FloatRange width) {
            _parent = parent;
            _width = width;
            _colCount = 0;

            _name = $"Walls ({_size.x} : {_size.y})";

            _container = new GameObject(_name);
            _container.transform.SetParent(_parent);
        }
        
        private void OnEnable() {
            _walls = new Dictionary<int, IWall>();
        }

        public void AddWalls(Vector3 wallPosition) {
            var wall = GenerateBoundary();
            var index = _walls.Count;
            _walls.Add(index, wall);
            _colCount++;
        }

        public void AddBoundary(Vector3 wallPosition) {
            if (!_walls.ContainsKey(_colCount)) {
                AddWalls(wallPosition);
                return;
            }
            
            //if (_width.IsInRange(_walls[_colCount].GameObjectPosition.x))
            //    return;

        }
        
        public void SetContainerPosition(Vector2Int pos) {
            var offset = new Vector3(pos.x, 0, pos.y);

            _container.transform.position = offset;
            
            foreach (var wall in _walls.Values) {
                wall.GetGameObject.transform.position += offset;
            }
        }

        public void TranslateBy() {
            foreach (var wall in _walls) {
                var wallTransform = wall.Value.GetGameObject.transform;
                wallTransform.Translate(Vector3.left);

                var wallPosition = wallTransform.position;
                if (_width.IsInRange(wallPosition.x))
                    continue;
                
                wallPosition.x = _width.max;
                wallTransform.position = wallPosition;
            }
        }

        private IWall GenerateBoundary() {
            var wallGO = Instantiate(
                IoC.Resolve<IWall>().GetGameObject,
                _container.transform
            );

            return wallGO.GetComponent<IWall>();
        }
    }
}