using System;
using System.Collections.Generic;

using TriFighter.Types;

using UnityEngine;

namespace TriFighter.Terrain {
    public interface IWallMap {
        void Init(string containerName, Transform parent, LinePath path);
        void SetContainerPosition(Vector2Int pos);
        void CycleBoundary();
    }
    
    public sealed class WallMap : ScriptableObject, IWallMap {
        private Transform _parent;
        private string _name;

        private LinePath _boundaryPath;
        
        private Dictionary<int, IWall> _walls;
        private Vector2Int _size;
        
        private GameObject _container;
        public GameObject WallContainer => _container;

        public void Init(string containerName, Transform parent, LinePath path) {
            _parent = parent;
            _boundaryPath = path;

            _name = containerName;

            _container = new GameObject(_name);
            _container.transform.SetParent(_parent);
            
            _walls = new Dictionary<int, IWall>();
            for (var i = 0; i < (int)path.Distance; i++) {
                if (i % 2 == 0) continue;
                
                var wall = GenerateBoundary();
                var pos = new Vector3(path.Source.x - i, path.Source.y);
                wall.SetPosition(pos);
                _walls.Add(i, wall);
            }
        }

        public void SetContainerPosition(Vector2Int pos) {
            var offset = new Vector3(pos.x, 0, pos.y);

            _container.transform.position = offset;
            
            foreach (var wall in _walls.Values) {
                wall.GetGameObject.transform.position += offset;
            }
        }

        public void CycleBoundary() {
            foreach (var wall in _walls.Values) {
                wall.MoveLeft();
                
                if (!_boundaryPath.ContainsX(wall.GameObjectPosition.x)) 
                    wall.SetPosition(_boundaryPath.Source);
            }
        }

        private IWall GenerateBoundary() {
            return Instantiate(
                IoC.Resolve<IWall>().GetGameObject,
                _container.transform
            ).GetComponent<IWall>();
        }
    }
}