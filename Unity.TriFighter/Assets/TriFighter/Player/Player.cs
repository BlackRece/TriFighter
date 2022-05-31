using UnityEngine;

namespace TriFighter {
    public sealed class Player : MonoBehaviour {
        private Renderer _renderer = null;

        [SerializeField] private float _rangeTolerance = 0.01f;
        [SerializeField] private float _speed = 1.0f;
        private Vector3 _targetPosition;

        [SerializeField] private Color _activeColor;
        private Color _inactiveColor;
        private Color ModelColor {
            get => _renderer.material.color;
            set => _renderer.material.color = value;
        }
        private bool _isHighlightActive = false;
        private float _distanceToTarget;
        
        private float _fireDelay;
        [SerializeField] private float _fireDelayLimit = 1f;
        [SerializeField] private GameObjectPooler _bulletPooler;

        public bool IsAtTargetPosition {
            get {
                var currentPosition = transform.position;
                currentPosition.z = 0f;
                
                _distanceToTarget = Vector3.Distance(currentPosition, _targetPosition);
                return _distanceToTarget < _rangeTolerance;
            }
        }

        private void Awake() {
            _renderer = GetComponent<Renderer>();
            _bulletPooler.Init("Player's Bullet Conatiner");
        }

        private void Start() {
            _inactiveColor = ModelColor;
            
            _targetPosition = Vector3.zero;
        }

        private void Update() {
            ActivateHighlight(MouseHandler.GetActiveState);

            if (MouseHandler.GetActiveState) {
                _targetPosition = GetTargetPosition();

                FireWeapon();
            }
            
            MoveToTargetPosition();
        }

        private void FireWeapon() {
            if (_fireDelay < 0) {
                SpawnBullet();
                _fireDelay = _fireDelayLimit;
            } else {
                _fireDelay -= Time.deltaTime;
            }
        }

        private void SpawnBullet() {
            var bullet = _bulletPooler.GetGameObject();
            bullet.transform.position = transform.position;
            bullet.gameObject.SetActive(true);
            
        }

        private Vector3 GetTargetPosition() {
            if (!MouseHandler.GetActiveState) 
                return _targetPosition;
            
            var nextPosition = CameraController.MouseWorldPosition;
            nextPosition.z = 0f;
            return nextPosition;
        }

        private void MoveToTargetPosition() {
            var currentPossition = transform.position;

            if (IsAtTargetPosition) {
                if (currentPossition != _targetPosition)
                    transform.position = _targetPosition;
                
                return;
            }

            var directionToTarget = (_targetPosition - currentPossition).normalized;
            directionToTarget.z = 0f;

            transform.Translate(directionToTarget * (_speed * Time.deltaTime));
        }

        private void ActivateHighlight(bool state) {
            if (_isHighlightActive == state) 
                return;

            _isHighlightActive = state;
            ModelColor = _isHighlightActive ? _activeColor : _inactiveColor;
        }
    }
}