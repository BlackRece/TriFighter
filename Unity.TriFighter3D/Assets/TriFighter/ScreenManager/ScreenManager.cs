using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace TriFighter.ScreenManagement {
    [CreateAssetMenu(menuName = "TriFighter Objects/ScreenManager")]
    public sealed class ScreenManager : ScriptableObject {
        public enum ScreenIndex {
            SceneManager = 0,
            TitleScreen = 1,
            GamePlay = 2
        }

        public GameObject _loader { get; set; }

        private static ScreenIndex _previousScreenIndex;
        private float _screenProgress;
        private float _progressTarget;
        public float ScreenProgress => _screenProgress;

        public void LoadScreen(int nextSceneIndex) {
            //_coroutineRunner.StartCoroutine(LoadAsyncOperation_CO((ScreenIndex)nextSceneIndex));
        }
        
        private IEnumerator LoadAsyncOperation_CO(ScreenIndex nextScreenIndex) {
            var scenes = new List<AsyncOperation>();

            if (_previousScreenIndex != ScreenIndex.SceneManager)
                scenes.Add(SceneManager.UnloadSceneAsync((int)_previousScreenIndex));

            _previousScreenIndex = nextScreenIndex;
            
            if (nextScreenIndex != ScreenIndex.SceneManager)
                scenes.Add(SceneManager.LoadSceneAsync((int)nextScreenIndex, LoadSceneMode.Additive));

            var progress = 0f;

            while (progress < scenes.Count) {
                progress = 0f;
                foreach (var scene in scenes) {
                    progress += scene.progress;
                    yield return new WaitForEndOfFrame();
                }

                if (progress > 0)
                    progress /= 2f;

                //_progressBar.fillAmount = progress;
            }
        }

        public async void LoadScreenAsync(int nextScreenIndex) {
            var screen = SceneManager.LoadSceneAsync(nextScreenIndex);
            screen.allowSceneActivation = false;
            
            //show loader canvas

            do {
                await Task.Delay(100);
                _progressTarget = screen.progress;
            } while (screen.progress < 0.9f);
        }

        public void Update() {
            _screenProgress = Mathf.MoveTowards(
                _screenProgress,
                _progressTarget,
                3 * Time.deltaTime
            );
        }
    }
}