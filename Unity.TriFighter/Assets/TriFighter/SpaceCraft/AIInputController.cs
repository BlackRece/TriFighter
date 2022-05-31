using UnityEngine;

namespace TriFighter {
    [CreateAssetMenu(menuName = "New AI InputController")]
    public sealed class AIInputController : ScriptableObject, IInputController {
        private Vector2 _axis;
        public Vector2 Axis => _axis;

        private Vector3 _mousePosition;
        public Vector3 MousePosition => _mousePosition;

        private bool _mouseButtonLeft;
        public bool MouseButtonLeft => _mouseButtonLeft;

        public ISubscriber EventQueue { get; private set; }

        public void Update() {
            _axis.Set(
                Random.Range(-1f,1f),
                Random.Range(-1f,1f)
            );
            
            _mousePosition = Vector3.left;
        }
    }
}