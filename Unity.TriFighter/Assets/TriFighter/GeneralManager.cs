using UnityEngine;

namespace TriFighter {
    
    public class GeneralManager : MonoBehaviour {
        [SerializeField] private EnemyManager _enemyManager = null;
        private bool _tutorialComplete = false;

        private void Update() {
            if (!_tutorialComplete) {
                DisplayTutorialMessage();
            }
        }
        
        private void DisplayTutorialMessage(){
            if (!MouseHandler.GetActiveState) {
                NoticeHandler.UpdateNotice("Click and hold to move.");
            } else {
                NoticeHandler.UpdateNotice(null);
                _tutorialComplete = true;
            }
        }
    }
}