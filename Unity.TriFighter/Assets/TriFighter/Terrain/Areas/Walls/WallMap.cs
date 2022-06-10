using System.Collections.Generic;

using TriFighter.Types;

using UnityEngine;
using UnityEngine.Assertions;

namespace TriFighter.Terrain {
    public interface IWallMap {
        void Init(Transform parent);
        void AddWalls(List<Vector3> wallPositions);
        void CreateWalls(List<ITile> tiles);
        void CreateBoundary(Vector2Int position);
        void CreateOpeningInBoundary(List<Vector2Int> boundaryOpenings);
        void SetPosition(Vector2Int pos);
    }
    
    public sealed class WallMap : ScriptableObject, IWallMap {
        private Transform _parent;
        private string _name;
        
        private Dictionary<Vector2Int, IWall> _walls;
        private Vector2Int _size;
        
        private GameObject _container;
        public GameObject WallContainer => _container;

        public void Init(Transform parent) {
            _parent = parent;

            _walls = new Dictionary<Vector2Int, IWall>();
            _name = $"Walls ({_size.x} : {_size.y})";

            _container = new GameObject(_name);
            _container.transform.SetParent(_parent);
        }
        
        private void OnEnable() {
            _walls = new Dictionary<Vector2Int, IWall>();
        }

        public void CreateWalls(List<ITile> tiles) {
            foreach (var tile in tiles) {
                
                var mapPos = tile.GetMapPosition();
                if (_walls.ContainsKey(mapPos)) continue;
                
                var wall = GenerateWall(_container.transform);
                
                var worldPos = tile.GetTopPosition();
                worldPos.y += GetGameObjectHeight(wall.GetGameObject) / 2;
                wall.GetGameObject.transform.position = worldPos;
                
                _walls.Add(mapPos, wall);
            }
        }

        public void AddWalls(List<Vector3> wallPositions) {
            foreach (var position in wallPositions) {
                var wall = GenerateBoundary();
                wall.GetGameObject.transform.position = position;
                var mapPos = new Vector2Int((int)position.x, (int)position.y); 
                _walls.Add(mapPos, wall);
            }
        }

        public void CreateBoundary(Vector2Int position) {
            var size = new IntSize(_size.x, _size.y);
            var mid = size.Center();

            var width = new IntRange(position.x - mid.x, position.x + mid.x);
            var height = new IntRange(position.y - mid.y, position.y + mid.y);

            for (var y = height.min; y <= height.max; y++) {
                var northPos = new Vector2Int(width.min, height.max);
                var southPos = new Vector2Int(width.max, height.max);
                
                var northWall = GenerateBoundary(northPos);
                var southWall = GenerateBoundary(southPos);
                
                AddBoundary(northPos, northWall);
                AddBoundary(southPos, southWall);
            }
        }

        private void AddBoundary(Vector2Int position, IWall boundary) {
            if (_walls.ContainsKey(position))
                return;
            
            _walls.Add(position, boundary);
        }

        public void CreateOpeningInBoundary(List<Vector2Int> boundaryOpenings) {
            foreach (var opening in boundaryOpenings) {
                if (_walls.ContainsKey(opening)) 
                    _walls[opening].Hide();
            }
        }

        public void SetPosition(Vector2Int pos) {
            var offset = new Vector3(pos.x, 0, pos.y);

            _container.transform.position = offset;
            
            foreach (var wall in _walls.Values) {
                wall.GetGameObject.transform.position += offset;
            }
        }

        private IWall GenerateWall(Transform parentTransform) {
            var wallGO = Instantiate(
                IoC.Resolve<IWall>().GetGameObject,
                _container.transform
            );
            
            return wallGO.GetComponent<IWall>();
        }

        private IWall GenerateBoundary(Vector2Int mapPosition) {
            var wallGO = Instantiate(
                IoC.Resolve<IWall>().GetGameObject,
                _container.transform
            );

            return wallGO.GetComponent<IWall>();
        }

        private IWall GenerateBoundary() {
            var wallGO = Instantiate(
                IoC.Resolve<IWall>().GetGameObject,
                _container.transform
            );

            return wallGO.GetComponent<IWall>();
        }
        

        private float GetGameObjectHeight(GameObject gameObject) => 
                gameObject.GetComponent<Renderer>().bounds.size.y;
        
    }
}