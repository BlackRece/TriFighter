using BlackRece.Events;

using TriFighter.ScreenManagement;

using UnityEngine;

namespace TriFighter.UI {
    public class MainMenu : MonoBehaviour {
        [SerializeField] private IntEvent _startGame;

        public void NewGame_Click() {
            if (_startGame != null) 
                _startGame.Raise((int)ScreenManager.ScreenIndex.GamePlay);
        }

        public void LoadGame_Click() { }
        public void Options_Click() { }
        public void QuitGame_Click() { }
    }
}
