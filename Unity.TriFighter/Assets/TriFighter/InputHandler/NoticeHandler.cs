using TMPro;

using UnityEngine;

namespace TriFighter {
    public sealed class NoticeHandler : MonoBehaviour {
        private TMP_Text _noticeDisplay = null;
        private static string _message = null;
        private static string _oldMessage = null;
        public static void UpdateNotice(string text) => _message = text;

        private void Awake() {
            _noticeDisplay = GetComponent<TextMeshPro>();
            _noticeDisplay.text = "Ready";
        }

        private void Update() {
            DisplayMessage();
        }

        private void DisplayMessage() {
            if (_oldMessage == _message) 
                return;
            
            _oldMessage = _message;
            _noticeDisplay.text = string.IsNullOrWhiteSpace(_message) ? "" : _message;
        }
    }
}