using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Case.Scene.Manager
{

    /// <summary>
    /// We make all device initializations such as input manager, audio middleware and the device specific configurations in splash scene.
    /// Heavy object pool creations and Network connections are also good candidate to be done in splash scene 
    /// for this study case I'll just implement a pseudo initialization which will delay a second then load the main scene.
    /// </summary>
    public class SplashManager : MonoBehaviour
    {
        [SerializeField]
        private Image _progressBar;

        #region Unity Methods

        private async void Start()
        {
            await InitializeAsync();
        }

        #endregion

        #region Manager Logic

        /// <summary>
        /// Pseudo Initialization for Splash screen. This method will fill the progressbar in a second then load the Main scene asynchronously.
        /// </summary>
        private async Task InitializeAsync()
        {
            float total = 1000f;
            int delay = 10;

            // Simulate 1 second delay
            for (int i = 0; i <= total; i += delay)
            {
#if UNITY_WEBGL
                await this.Delay(delay / total);
#else
                await Task.Delay(delay);
#endif
                _progressBar.fillAmount = i / total;
            }

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
#if UNITY_WEBGL
                await this.Delay(delay / total);
#else
                await Task.Delay(delay);
#endif
            }
        }

        #endregion

    }
}