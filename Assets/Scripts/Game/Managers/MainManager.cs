using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Case.Scene.Manager
{

    /// <summary>
    /// MainManager just handles Main menu button events.
    /// </summary>
    public class MainManager : MonoBehaviour
    {
        #region Manager Logic

        /// <summary>
        /// Event Handler for the Start button clicked event. 
        /// We load the Loading Scene which handles the game play related data before starting the game itself.
        /// </summary>
        public async void OnStartClicked()
        {
            await loadLoadingSceneAsync();
        }

        /// <summary>
        /// Quits the game.
        /// </summary>
        public void OnQuitClicked()
        {
            Application.Quit();
        }

        /// <summary>
        /// Loads the LoadingScene asynchronously.
        /// </summary>
        private async Task loadLoadingSceneAsync()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LoadingScene");

            // Check every 10ms if Loading scene is loaded.
            while (!asyncLoad.isDone)
            {
                await Task.Delay(10);
            }
        }

        #endregion

    }
}