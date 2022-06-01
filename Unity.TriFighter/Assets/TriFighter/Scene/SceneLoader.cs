using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TriFighter.SceneManagement {
    public class SceneLoader : MonoBehaviour {
        public enum SceneIndex {
            SceneManager = 0,
            TitleScreen = 1,
            GamePlay = 2
        }

        [SerializeField] private Image _progressBar;
        private SceneIndex _previousSceneIndex;

        public void LoadScene(SceneIndex nextSceneIndex) {
            gameObject.SetActive(true);
            
            StartCoroutine(LoadAsyncOperation_CO(nextSceneIndex));
            
            gameObject.SetActive(false);
        }
        
        private void Start() {
            _previousSceneIndex = SceneIndex.SceneManager;

            //StartCoroutine(LoadAsyncOperation_CO(SceneIndex.TitleScreen));
            //StartCoroutine(LoadAsyncOperation_CO(SceneIndex.GamePlay));
            //LoadScene(SceneIndex.GamePlay);
            LoadScene(SceneIndex.TitleScreen);
        }
        
        private IEnumerator LoadAsyncOperation_CO(SceneIndex nextSceneIndex) {
            var scenes = new List<AsyncOperation>();

            if (_previousSceneIndex != SceneIndex.SceneManager)
                scenes.Add(SceneManager.UnloadSceneAsync((int)_previousSceneIndex));

            _previousSceneIndex = nextSceneIndex;
            
            if (nextSceneIndex != SceneIndex.SceneManager)
                SceneManager.LoadSceneAsync((int)nextSceneIndex, LoadSceneMode.Additive);

            var progress = 0f;

            while (progress < scenes.Count) {
                progress = 0f;
                foreach (var scene in scenes) {
                    progress += scene.progress;
                    yield return new WaitForEndOfFrame();
                }

                if (progress > 0)
                    progress /= 2f;

                _progressBar.fillAmount = progress;
            }
        }
    }
}