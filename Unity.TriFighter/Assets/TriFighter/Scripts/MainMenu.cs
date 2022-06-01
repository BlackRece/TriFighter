using BlackRece.Events;

using UnityEngine;

namespace TriFighter.UI {
    public class MainMenu : MonoBehaviour {
        [SerializeField] private VoidEvent _startGame;

        public void NewGame_Click() {
            _startGame?.Raise();
        }

        public void LoadGame_Click() { }
        public void Options_Click() { }
        public void QuitGame_Click() { }
    }
}