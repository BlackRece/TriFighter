using System;

using BlackRece.Events;

using TriFighter.SceneManagement;

using UnityEngine;

namespace TriFighter {
    public sealed class GameManager : MonoBehaviour {
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private GlobalSettings _settings;
        [SerializeField] private VoidEvent _sceneReadyEvent;

        [SerializeField] private VoidListener _startGameListener;

        private SceneLoader _sceneLoader;
        
        private void Awake() {
            if(_settings == null)
                throw new NullReferenceException("No [Settings] available!");
            
            if (_loadingScreen == null)
                throw new NullReferenceException("No [Loading Screen] available!");
            
            if (!_loadingScreen.TryGetComponent(out SceneLoader sceneLoader))
                throw new NullReferenceException("No [Loading Screen] available!");

            _sceneLoader = sceneLoader;
            if(_loadingScreen.activeSelf)
                _loadingScreen.SetActive(false);
        }

        private void Start() {
            LoadScene(SceneLoader.SceneIndex.TitleScreen);
        }

        public void LoadScene(SceneLoader.SceneIndex sceneIndex) {
            _sceneLoader.LoadScene(sceneIndex);
            
            if(_sceneReadyEvent != null)
                _sceneReadyEvent.Raise();
        }
        
        public void LoadScene() {
            _sceneLoader.LoadScene(SceneLoader.SceneIndex.GamePlay);
            
            if(_sceneReadyEvent != null)
                _sceneReadyEvent.Raise();
        }
    }
}