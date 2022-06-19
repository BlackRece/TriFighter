using TriFighter.Types;

using UnityEngine;

namespace TriFighter {
    [CreateAssetMenu(menuName = "TriFighter Objects/AI Controllers/New AI InputController")]
    public sealed class AIInputController : ScriptableObject, IInputController {
        public FloatRange PlayArea => _playArea;
        public Vector2 Axis => _axis;
        public float MaxMoveSpeed => _maxMoveSpeed;

        public Vector3 MousePosition => _mousePosition;
        public bool MouseButtonLeft => _mouseButtonLeft;
        public ISubscriber EventQueue { get; private set; }

        private readonly FloatRange _playArea = new FloatRange(-100, +100);
        private Vector2 _axis;
        private Vector3 _mousePosition;
        private bool _mouseButtonLeft;
        private float _maxMoveSpeed;

        public void Update() {
            _axis.Set(
                Random.Range(-1f,1f),
                Random.Range(-1f,1f)
            );
            
            _mousePosition = Vector3.left;
        }
    }
}