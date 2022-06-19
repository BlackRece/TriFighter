using TriFighter.Types;

using UnityEngine;

namespace TriFighter.Terrain {
    public interface IArea {
        Rect RectSize { get; }

        void Update();
    }

    public sealed class Area : IArea {
        private Rect _rect;
        public Rect RectSize => _rect;

        private GameObject _container;
        public GameObject Container => _container;

        private IWallMap _topWallMap, _botWallMap;

        public Area(AreaDetail detail) {
            _rect = detail.Rect;

            CreateContainer(detail.Parent);
            CreateSpace();
        }
        
        public void Update() {
            _topWallMap.CycleBoundary();
            _botWallMap.CycleBoundary();
        }
        
        private void CreateContainer(Transform parent) {
            _container = new GameObject();
            _container.transform.SetParent(parent);
        }

        private void CreateSpace() {
            var topLine = new LinePath(
                new Vector2(_rect.xMax, _rect.yMin),
                new Vector2(_rect.xMin, _rect.yMin));

            var botLine = new LinePath(
                new Vector2(_rect.xMax, _rect.yMax),
                new Vector2(_rect.xMin, _rect.yMax));
            
            _topWallMap = ScriptableObject.CreateInstance<WallMap>();
            _topWallMap.Init("TopBoundary", _container.transform, topLine);
            
            _botWallMap = ScriptableObject.CreateInstance<WallMap>();
            _botWallMap.Init("BotBoundary", _container.transform, botLine);
        }

        
    }
}