using Case.Core;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Case.Scene.Manager
{

    /// <summary>
    /// We make all light-weight object pool creations, loading terrains, multiplayer network connections in LoadingScene.
    /// Also game user interface preparations are good candidate to be done in LoadingScene as well.
    /// For this study case I'll just implement light-weight object pool initialization in LoadingScene.
    /// </summary>
    public class LoadingManager : MonoBehaviour
    {
        [SerializeField]
        private Image _progressBar;

        private bool isGameLoading = false;

        #region Unity Methods

        private async void Update()
        {
            // Update the progressbar with the current object pool progress.
            _progressBar.fillAmount = ObjectPoolManager.Instance.Progress;

            // Load the game scene when object pool is ready.
            if (ObjectPoolManager.Instance.IsReady && !isGameLoading)
            {
                isGameLoading = true;
                await loadGameSceneAsync();
            }
        }

        #endregion

        #region Manager Logic

        /// <summary>
        /// Loads the GameScene asynchronously.
        /// </summary>
        private async Task loadGameSceneAsync()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                await Task.Delay(System.TimeSpan.FromMilliseconds(10));
            }
        }

        #endregion

    }
}