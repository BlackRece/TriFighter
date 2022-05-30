using UnityEngine;

namespace TriFighter {
    
    public sealed class MouseHandler : MonoBehaviour {
        public static bool GetActiveState { get; private set; }

        private enum MouseButton {
            LeftClick = 0,
            RightClick,
            //MiddleClick
        }

        private void HandleMouseInput() {
            /*var layerId = _targetChecker.Target("Tile");
            _targetChecker.Check(layerId);
            
            if (!_targetChecker.Found)
                return;
            
            var targetTile = _targetChecker._raycastHitInfo.transform;
            var targetTileObject = targetTile.GetComponent<ITile>();

            if (targetTile != _lastTargetedTileTransform) {
                if (_lastTargetedTileTransform != null) {
                    var lastTileObject = _lastTargetedTileTransform.GetComponent<ITile>();
                    lastTileObject.ToggleHighlight();
                }

                targetTileObject.ToggleHighlight();
                
                _lastTargetedTileTransform = targetTile;
            }*/
            
            if (Input.GetMouseButton((int) MouseButton.LeftClick)) 
                GetActiveState = true;
            else 
                GetActiveState = false;

            /*if (Input.GetMouseButtonDown((int) MouseButton.RightClick)) {
                SelectTile(targetTileObject);
            }*/
        }

        private void Update() {
            HandleMouseInput();
        }
    }
}