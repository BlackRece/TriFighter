using UnityEngine;

namespace TriFighter {
    public interface ICursorController {
        void CreateMarker(string markerName, Transform parentTransform);
        Vector3 UpdatePosition(Vector3 target);
    }

    public sealed class CursorController : ICursorController {
        private GameObject _cursorMarker = null;
        
        public void CreateMarker(string markerName, Transform parentTransform) {
            _cursorMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _cursorMarker.name = markerName;
            _cursorMarker.transform.SetParent(parentTransform);
        }
        
        public Vector3 UpdatePosition(Vector3 target) {
            var targetPositionInWorld = CameraController.GetCursorPosition(target);
            if(_cursorMarker != null)
                _cursorMarker.transform.SetPositionAndRotation(targetPositionInWorld, Quaternion.identity);
            return targetPositionInWorld;
        }
    }
}