namespace BlackRece.TriFighter2D.UI.MainMenu {
    using System.Collections;

    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    public static class ScenePaths {
        public const string MainMenu = "Scenes/Menu";
        public const string GameScene = "Scenes/Game";
    }
    
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Slider m_loadingSlider = null;

        private void Awake() {
            if(m_loadingSlider == null)
                Debug.LogWarning("Loading Slider is not set in the inspector!");
        }

        private void Start()
        {
            m_loadingSlider.gameObject.SetActive(false);
        }
        
        public void OnPlayButtonClicked()
        {
            m_loadingSlider.gameObject.SetActive(true);
            m_loadingSlider.value = 0f;
            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene() {
            AsyncOperation l_async = SceneManager.LoadSceneAsync(ScenePaths.GameScene);
            l_async.allowSceneActivation = false;
            while (!l_async.isDone) {
                m_loadingSlider.value = l_async.progress;
                if (l_async.progress >= 0.9f) {
                    m_loadingSlider.value = 1f;
                    l_async.allowSceneActivation = true;
                }
                yield return null;
            }
        }
    }
}
