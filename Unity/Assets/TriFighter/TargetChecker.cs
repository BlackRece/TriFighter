using BlackRece.Events;

using UnityEngine;

namespace TriFighter {
    public interface ITargetChecker {
        bool Found { get; }
        LayerMask Target(string layerToTarget);
        void Check(LayerMask layerIdToTarget = new LayerMask());
    }
    
    [RequireComponent(typeof(Camera))]
    public sealed class TargetChecker : MonoBehaviour, ITargetChecker {
        [SerializeField] private RectEvent _worldBoundaryEvent = null;
        private static readonly Camera _mainCamera = Camera.main;
        private static Camera _cam => _mainCamera;

        private Ray MouseRay(Camera cam) => _cam.ScreenPointToRay(Input.mousePosition);
        
        public bool Found { get; private set; }
        public RaycastHit _raycastHitInfo;
        private static Rect _playArea = default;
        public static Rect PlayArea {
            get {
                if (_playArea == default) {
                    _playArea = GetPlayArea();
                }
                return _playArea;
            }
        }

        public static Vector3 MouseViewportPosition => _cam.ScreenToViewportPoint(Input.mousePosition);
        public static Vector3 MouseWorldPosition => _cam.ScreenToWorldPoint(Input.mousePosition);

        private static Rect _viewportRect { get; set; } = default;
        public static Rect GetViewportRect {
            get {
                if (_viewportRect == default) {
                    _viewportRect = _cam.rect;
                }
                
                return _viewportRect;
            }
        }

        private static Rect GetPlayArea() {
            // viewport reminder:
            // bottom-left  = 0,0
            // top-right    = 1,1
            var topRight = _cam.ViewportToWorldPoint(new Vector3(1, 1));
            var botleft = _cam.ViewportToWorldPoint(new Vector3(0, 0));

            return new Rect(
                botleft.x,
                botleft.y,
                topRight.x - botleft.x,
                topRight.y - botleft.y
            );
        }

        public LayerMask Target(string layerToTarget) => LayerMask.GetMask(layerToTarget);
        public void Check(LayerMask layerIdToTarget = new LayerMask()) =>
            Found = Physics.Raycast(MouseRay(_cam), out _raycastHitInfo, _cam.farClipPlane, layerIdToTarget, QueryTriggerInteraction.Ignore);
    }
}