using UnityEngine;

namespace TriFighter {
    public class CameraController : MonoBehaviour {
        [SerializeField] private static Vector3 offset = new Vector3(0, 0, -20f);

        private static Camera _cam;
        private PublisherSubscriber _eventQueue;

        public static Vector3 GetCursorPosition(Vector3 targetPosition) => 
            _cam.ScreenToWorldPoint(targetPosition - offset);

        public static void UpdatePlayerPosition(Vector3 position) =>
            _cam.transform.position = position + offset;

        private void Awake() {
            _cam = Camera.main;
            MovementController.PlayerMoved += TrackPlayerMovement;
            
            _eventQueue = new PublisherSubscriber();
        }

        private void TrackPlayerMovement(Vector3 position) => 
            _cam.transform.position = position + offset;
    }

    public sealed class CursorPositionEvent {
        public Vector3 CursorPostion { get; set; }
    }
}