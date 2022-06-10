﻿using UnityEngine;

namespace TriFighter {
    public interface ITargetChecker {
        bool Found { get; }
        LayerMask Target(string layerToTarget);
        void Check(LayerMask layerIdToTarget = new LayerMask());
    }

    public interface ICameraController {
        
    }

    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour, ICameraController, ITargetChecker {
        [SerializeField] private static Vector3 offset = new Vector3(0, 0, -20f);

        private static Camera _cam;
        
        private RaycastHit _raycastHitInfo;
        private static Rect _playArea = default;
        public static Rect PlayArea {
            get {
                if (_playArea == default) {
                    _playArea = GetPlayArea();
                }
                return _playArea;
            }
        }

        public bool Found { get; private set; }
        
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
            var topRight = _cam.ViewportToWorldPoint(new Vector3(1, 1));
            var botleft = _cam.ViewportToWorldPoint(new Vector3(0, 0));

            return new Rect(
                botleft.x,
                botleft.y,
                topRight.x - botleft.x,
                topRight.y - botleft.y
            );
        }
        
        private Ray MouseRay(Camera cam) => _cam.ScreenPointToRay(Input.mousePosition);

        private void TrackPlayerMovement(Vector3 position) => 
            _cam.transform.position = position + offset;

        public LayerMask Target(string layerToTarget) => LayerMask.GetMask(layerToTarget);

        public void Check(LayerMask layerIdToTarget = new LayerMask()) =>
            Found = Physics.Raycast(
                MouseRay(_cam),
                out _raycastHitInfo,
                _cam.farClipPlane,
                layerIdToTarget,
                QueryTriggerInteraction.Ignore
            );
        
        public static Vector3 GetCursorPosition(Vector3 targetPosition) => 
            _cam.ScreenToWorldPoint(targetPosition - offset);

        public static void UpdatePlayerPosition(Vector3 position) =>
            _cam.transform.position = position + offset;

        private void Awake() {
            _cam = Camera.main;
        }
    }

    public sealed class CursorPositionEvent {
        public Vector3 CursorPostion { get; set; }
    }
}