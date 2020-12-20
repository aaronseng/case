using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Case.Splash
{

    // We make all device initializations such as input manager, audio middleware and the device specific configurations in splash scene.
    // Heavy object pool creations and Network connections are also good candidate to be done in splash scene 
    // for this study case we'll just implement a pseudo initialization which will delay a second then load the Main scene.

    public class SplashManager : MonoBehaviour
    {
        [SerializeField]
        private Image _progressBar;

        #region Unity Methods

        private async void Start()
        {
            await Initialize();
        }

        #endregion

        #region Manager Logic

        /// <summary>
        /// Pseudo Initialization for Splash screen. This method will fill the progressbar in a second then load the Main scene asynchronously.
        /// </summary>
        /// <returns></returns>
        private async Task Initialize()
        {
            float total = 1000f;
            int delay = 10;
            
            // Simulate 1 second delay
            for(int i = 0; i <= total; i+= delay)
            {
                await Task.Delay(System.TimeSpan.FromMilliseconds(delay));
                _progressBar.fillAmount = i / total;
            }

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                await Task.Delay(System.TimeSpan.FromMilliseconds(delay));
            }
        }

        #endregion

    }
}