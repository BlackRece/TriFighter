using TriFighter.Types;

using UnityEngine;

namespace TriFighter.Terrain {
    public interface IArea {
        Rect RectSize { get; }

        void AddBoundaryWalls();
        void MoveBoundary();
        void Update();
    }

    public sealed class Area : IArea {
        private Rect _rect;
        public Rect RectSize => _rect;

        private GameObject _container;
        public GameObject Container => _container;

        private IWallMap _topWallMap, _botWallMap;

        private readonly Vector3 _top;
        private readonly Vector3 _bot;

        public Area(AreaDetail detail) {
            _rect = detail.Rect;
            
            _top = new Vector3(_rect.xMax, _rect.yMin);
            _bot = new Vector3(_rect.xMax, _rect.yMax);

            CreateContainer(detail.Parent);
            CreateSpace();
        }

        private void CreateContainer(Transform parent) {
            _container = new GameObject();
            _container.transform.SetParent(parent);
        }

        private void CreateSpace() {
            var width = new FloatRange((int)_rect.xMin, (int)_rect.xMax);
            
            _topWallMap = IoC.Resolve<IWallMap>();
            _topWallMap.Init(_container.transform, width);
            
            _botWallMap = IoC.Resolve<IWallMap>();
            _botWallMap.Init(_container.transform, width);
        }

        public void AddBoundaryWalls() {
            _topWallMap.AddBoundary(_top);
            _botWallMap.AddBoundary(_bot);
        }
        
        public void MoveBoundary() {
            _topWallMap.TranslateBy();
            _botWallMap.TranslateBy();
        }

        public void Update() {
            /*
             * move walls to the left by 1
             * any on the left-most edge move to right-most edge
             * if not enough boundary blocks, add some 
             */
            
            MoveBoundary();
            AddBoundaryWalls();
        }
    }
}