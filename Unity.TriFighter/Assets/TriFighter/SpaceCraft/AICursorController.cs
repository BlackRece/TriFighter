using UnityEngine;

namespace TriFighter {
    [CreateAssetMenu(menuName = "New AI CursorController")]
    public sealed class AICursorController : ScriptableObject, ICursorController {
        private GameObject _cursorMarker;
        
        public void CreateMarker(string markerName, Transform parentTransform) {
            return;
            _cursorMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _cursorMarker.name = markerName;
            _cursorMarker.transform.SetParent(parentTransform);
        }

        public Vector3 Update(Vector3 target) {
            return target;
        }
    }
}