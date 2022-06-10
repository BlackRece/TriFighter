using TriFighter.Terrain;

using UnityEngine;

namespace TriFighter {
    
    public class GeneralManager : MonoBehaviour {
        //[SerializeField] private EnemyManager _enemyManager = null;
        [SerializeField] private GameObject _enemyManager = null;
        
        private bool _tutorialComplete = false;
        private DependencyManager _dependencyResolver;

        private void Awake() {
            _dependencyResolver = new DependencyManager();
            IoC.Initialise(_dependencyResolver);
        }

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