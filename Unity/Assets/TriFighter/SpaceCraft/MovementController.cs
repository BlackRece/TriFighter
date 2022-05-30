using System;

using UnityEngine;

namespace TriFighter {
    public interface IMovementController {
        Vector2 MoveSpeed { get; }
        void ProcessInput(Vector2 inputAxis);
        void AdjustRotation(Vector3 targetPosition);
    }
    
    public sealed class MovementController : IMovementController {
        private readonly Transform _transform;
        private readonly float _maxMoveSpeed;
        private Vector2 _moveSpeed;
        private readonly float _rotateSpeed;

        private ISubscriber _camEvents;
        public static event Action<Vector3> PlayerMoved = delegate(Vector3 position) { }; 

        public MovementController(Transform transform, float maxMoveSpeed, float rotateSpeed) {
            _transform = transform;
            _maxMoveSpeed = maxMoveSpeed;
            _rotateSpeed = rotateSpeed;
            _moveSpeed = new Vector2();
            
            //PubSub.RegisterSubscribe<MoveTargetEvent>(NotifyMove);
        }

        public Vector2 MoveSpeed {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }

        public void ProcessInput(Vector2 inputAxis) {
            AdjustSpeed(inputAxis);
            LimitSpeedToMax();
            MoveBy();

            //AdjustRotation();
        }

        public void AdjustRotation(Vector3 targetPosition) {
            var direction = targetPosition - _transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            var rotateStep = _rotateSpeed * 10 * Time.deltaTime;
            _transform.rotation = Quaternion.Slerp(_transform.rotation, rotation, rotateStep);
        }

        private void MoveBy() {
            var position = _transform.position;
            var step = (Vector3)_moveSpeed * Time.deltaTime;
            position += step;
            //PlayerMoved(position);
            //PubSub.Publish(new MoveTargetEvent{Position = position});
            _transform.position = position;
        }

        private void AdjustSpeed(Vector2 speed) {
            const float DragSpeed = 0.2f;

            float x = 0, y = 0;

            x = _moveSpeed.x + speed.x;
            y = _moveSpeed.y + speed.y;

            _moveSpeed.Set(x, y);
            
            if (_moveSpeed.x < 0) x = _moveSpeed.x + DragSpeed;
            if (_moveSpeed.x > 0) x = _moveSpeed.x - DragSpeed;

            if (_moveSpeed.y < 0) y = _moveSpeed.y + DragSpeed;
            if (_moveSpeed.y > 0) y = _moveSpeed.y - DragSpeed;

            if (_moveSpeed.x < DragSpeed && _moveSpeed.x > -DragSpeed) x = 0;
            if (_moveSpeed.y < DragSpeed && _moveSpeed.y > -DragSpeed) y = 0;
            
            _moveSpeed.Set(x, y);
        }

        private void LimitSpeedToMax() {
            if (_moveSpeed.x > +_maxMoveSpeed) _moveSpeed.x = +_maxMoveSpeed; 
            if (_moveSpeed.x < -_maxMoveSpeed) _moveSpeed.x = -_maxMoveSpeed; 
            
            if (_moveSpeed.y > +_maxMoveSpeed) _moveSpeed.y = +_maxMoveSpeed; 
            if (_moveSpeed.y < -_maxMoveSpeed) _moveSpeed.y = -_maxMoveSpeed;
        }

        private void NotifyMove(object publishEvent) {
            var noticeevent = publishEvent as MoveTargetEvent;
            
        } 
    }

    public sealed class MoveTargetEvent {
        public Vector3 Position { get; set; }
    }
}