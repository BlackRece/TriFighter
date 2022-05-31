using System.Collections.Generic;

using UnityEngine;

namespace TriFighter {
    
    public interface IInputController {
        Vector2 Axis { get; }
        Vector3 MousePosition { get; }
        bool MouseButtonLeft { get; }
        
        ISubscriber EventQueue { get; }

        void Update();
    }
    
    public sealed class InputController : IInputController {
        private struct InputAxis {
            public const string HORIZONTAL = "Horizontal";
            public const string VERTICAL = "Vertical";
        }

        public enum InputMouseButtons {
            None = -1,
            Left = 0,
            Right = 1,
            Middle = 2
        }

        private Vector2 _axis;
        public Vector2 Axis => _axis;

        private Vector3 _mousePosition;
        public Vector3 MousePosition => _mousePosition;

        private bool _mouseButtonLeft;
        public bool MouseButtonLeft => _mouseButtonLeft;
        
        private Dictionary<InputMouseButtons, bool> _mouseButtons;
        public Dictionary<InputMouseButtons, bool> MouseButtons => _mouseButtons;

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

            _mouseButtonLeft = Input.GetMouseButton((int)InputMouseButtons.Left);
        }
    }
}