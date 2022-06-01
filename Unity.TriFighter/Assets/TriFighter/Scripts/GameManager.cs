using System;

using BlackRece.Events;

using TriFighter.SceneManagement;

using UnityEngine;

namespace TriFighter {
    public sealed class GameManager : MonoBehaviour {
        [SerializeField] private GameObject _loadingScreen = null;
        [SerializeField] private GlobalSettings _settings = null;
        [SerializeField] private VoidEvent _sceneReady;

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
            _sceneLoader.LoadScene(SceneLoader.SceneIndex.TitleScreen);
            
            if(_sceneReady != null)
                _sceneReady.Raise();
        }

    }
}