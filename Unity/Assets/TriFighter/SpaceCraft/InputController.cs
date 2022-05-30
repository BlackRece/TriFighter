using UnityEngine;

namespace TriFighter {
    
    public interface IInputController {
        Vector2 Axis { get; }
        Vector3 MousePosition { get; }
        ISubscriber EventQueue { get; }

        void Update();
    }
    
    public sealed class InputController : IInputController {
        private struct InputAxis {
            public const string HORIZONTAL = "Horizontal";
            public const string VERTICAL = "Vertical";
        }

        private Vector2 _axis;
        public Vector2 Axis => _axis;

        private Vector3 _mousePosition;
        public Vector3 MousePosition => _mousePosition;
        
        public ISubscriber EventQueue { get; private set; }

        public InputController() {
            EventQueue = new PublisherSubscriber();
        }

        public void Update() {
            _axis.Set(
                Input.GetAxis(InputAxis.HORIZONTAL),
                Input.GetAxis(InputAxis.VERTICAL)
            );

            _mousePosition = Input.mousePosition;
        }
    }
}