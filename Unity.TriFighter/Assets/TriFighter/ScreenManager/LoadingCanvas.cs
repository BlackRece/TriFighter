using System;

using TriFighter.ScreenManagement;

using UnityEngine;
using UnityEngine.UI;

namespace TriFighter.SceneManagement {
    public class LoadingCanvas : MonoBehaviour {
        [SerializeField] private GameObject _loadScreen;
        [SerializeField] private GameObject _splashScreen;
        [SerializeField] private Image _progressBar;
        [SerializeField] private float _splashDelay;
        [SerializeField] private ScreenManager _screenManager;
        
        private bool _isShowingSplash => _splashScreen.activeSelf;
        private bool _isShowlingLoading => _loadScreen.activeSelf;

        private float _progress;
        private bool _isReady;

        public void LoadScreen(int nextScreenIndex) {
            _loadScreen.SetActive(true);

            //_screenManager.LoadScreen(nextScreenIndex);
            _screenManager.LoadScreenAsync(nextScreenIndex);
        }

        public void OnScreenLoadProgress(float ratioLoaded) {
            _progress = ratioLoaded;
        }

        private void Awake() {
            _isReady = false;
            _progress = 0f;

            if (_screenManager == null)
                throw new NullReferenceException("No [ScreenManager] assigned!");
        }

        private void Start() {
            _loadScreen.SetActive(false);
            _splashDelay = _splashDelay < 2.0f
                ? 2.0f
                : _splashDelay;
        }

        private void Update() {
            if (_isShowingSplash) {
                UpdateSplashDisplay();
                return;
            }

            if (_isShowlingLoading) {
                UpdateLoadingDisplay();
                return;
            }

            if(_isReady)
                gameObject.SetActive(false);
        }

        private void UpdateLoadingDisplay() {
            if (_progress >= 1.0f) {
                _loadScreen.SetActive(false);
            }
            
            _screenManager.Update();
            LoadScreen((int)ScreenManager.ScreenIndex.TitleScreen);
            _progress = _screenManager.ScreenProgress;
        }

        private void UpdateSplashDisplay() {
            if (_splashDelay > 0) {
                _splashDelay -= Time.deltaTime;
                return;
            }
            
            _splashScreen.SetActive(false);
        }
    }
}
