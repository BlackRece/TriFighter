using System;
using System.Collections.Generic;

using TriFighter.Types;

using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

namespace TriFighter.Terrain {
    public interface ITerrainController {
        Vector2Int Position { get; }
        
        void Init(Transform parentTransform, IntSize startingRoomSize);
        
    }

    [CreateAssetMenu(menuName = "TriFighter Objects/New Terrain Controller")]
    public sealed class TerrainController : ScriptableObject, ITerrainController {
        [SerializeField] private int MAX_TILE_GROUPS = 100;

        [SerializeField] private int MAX_PATH_LENGTH = 10;
        
        [SerializeField] private int MAX_ROOM_SIZE = 50;
        [SerializeField] private int MIN_ROOM_SIZE = 10;
        
        [SerializeField] private IntRange ROOM_LIMIT = new IntRange(5, 10);
        [SerializeField] private IntRange ROOM_WIDTH = new IntRange(10, 50);
        [SerializeField] private IntRange ROOM_HEIGHT = new IntRange(10, 50);
        
        [SerializeField] private IntRange PATH_LENGTH = new IntRange(3, 10);
        
        private Dictionary<Vector2Int, ITile> _tiles;
        private List<IArea> _areas;
        private Transform _parentTransform;
        private IntSize _startingRoomSize;
        private int _numberOfRooms;

        private Vector3 _playerSpawnPosition;
        private Vector2Int _currentPos;
        private Rect _boundary;
        public Vector2Int Position => _currentPos;
        
        public event UnityAction<Vector3> TerrainComplete;

        private void OnEnable() {
            _tiles = new Dictionary<Vector2Int, ITile>();
            _areas = new List<IArea>();
        }
        
        public void Init(Transform parentTransform, IntSize startingRoomSize) {
            _parentTransform = parentTransform;
            
            _startingRoomSize = startingRoomSize;
            _numberOfRooms = ROOM_LIMIT.Random();
            
            _currentPos = new Vector2Int();
            _playerSpawnPosition = new Vector3(_currentPos.x, 1f, _currentPos.y);
        }

        public void Init(Transform parentTransform, Rect spaceBoundary) {
            _parentTransform = parentTransform;

            var offscreenBuffer = spaceBoundary.width / 4;
            var boundaryWidth = new FloatRange(
                spaceBoundary.xMin - offscreenBuffer,
                spaceBoundary.xMax + offscreenBuffer
            );

            _boundary = new Rect(
                boundaryWidth.min,
                spaceBoundary.yMin,
                boundaryWidth.max,
                spaceBoundary.yMax
            );
        }
        
        public void CreateSpaceMap() {
            /*
             *
             * remove generated rooms
             * 
             */

            CreateSpawnSpace();
        }

        private void CreateSpawnSpace() {
            var space = CreateSpaceArea();
        }
        
        private IArea CreateSpaceArea() {
            var area = new Area(
                new AreaDetail {
                    Rect = _boundary,
                    Parent = _parentTransform
                }
            );

            return area;
        }

    }
}