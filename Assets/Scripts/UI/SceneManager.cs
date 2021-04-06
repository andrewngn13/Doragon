using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using Doragon.Logging;

namespace Doragon.UI
{
    [RequireComponent(typeof(Canvas))]
    /// <summary> Instanced singleton on application startup that allows async scene transition alongside animated loading UI.
    public class SceneManager : MonoBehaviour
    {
        private bool instanced, loading;
        private Canvas loadingPrefabCanvas;
        private AnimateLoading animateLoader;
        [SerializeField] private GameObject LoadingPrefab;
        private void Awake()
        {
            if (!instanced)
            {
                instanced = true;
                loadingPrefabCanvas = GetComponent<Canvas>();
                loadingPrefabCanvas.enabled = false;
                animateLoader = loadingPrefabCanvas.GetComponent<AnimateLoading>();
                DontDestroyOnLoad(this);
                DontDestroyOnLoad(loadingPrefabCanvas.gameObject);
            }
            else
            {
                Destroy(this);
            }
        }

        /// <summary> Loads singular scene asynchronously and displays UI loading prefab during load
        public async void LoadSceneAsync(string sceneName)
        {
            if (loading) return;
            loading = true;
            Application.backgroundLoadingPriority = ThreadPriority.Low;
            loadingPrefabCanvas.enabled = true;
            await animateLoader.Animate();
            try
            {
                await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            }
            catch
            {
                DLogger.Log("Scene load failed");
            }

            await animateLoader.StopAnimating();
            loadingPrefabCanvas.enabled = false;
            loading = false;
            Application.backgroundLoadingPriority = ThreadPriority.Normal;
        }
    }
}