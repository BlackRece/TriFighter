using System;

using BlackRece.Events;

using TriFighter.SceneManagement;
using TriFighter.ScreenManagement;

using UnityEngine;

namespace TriFighter {
    public sealed class SceneController : MonoBehaviour {
        [SerializeField] private GameObject _loadingScreenPrefab;
        [SerializeField] private GlobalSettings _settings;
        [SerializeField] private VoidEvent _sceneReadyEvent;

        private LoadingCanvas _loadingCanvas;
        private GameObject _loadingScreen;

        public void LoadScene(int sceneindex) {
            gameObject.SetActive(true);
            _loadingCanvas.LoadScreen(sceneindex);
        }

        private void Awake() {
            if(_settings == null)
                throw new NullReferenceException("No [Settings] available!");
            
            if (_loadingScreenPrefab == null)
                throw new NullReferenceException("No [Loading Screen] available!");

            _loadingScreen = Instantiate(_loadingScreenPrefab, transform);
            
            if (!_loadingScreen.TryGetComponent(out LoadingCanvas sceneLoader))
                throw new NullReferenceException("No [Loading Screen] available!");

            _loadingCanvas = sceneLoader;
        }

        private void Start() {
            LoadScene((int)ScreenManager.ScreenIndex.TitleScreen);
        }
    }

}