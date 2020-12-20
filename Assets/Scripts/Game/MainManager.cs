using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Case.Scene.Manager
{
    public class MainManager : MonoBehaviour
    {
        #region Manager Logic

        /// <summary>
        /// Event Handler for the Start button clicked event. 
        /// We load the Loading Scene which handles the game play related data before starting the game itself.
        /// </summary>
        public async void OnStartClicked()
        {
            await loadLoadingScene();
        }

        /// <summary>
        /// Quit the Game.
        /// </summary>
        public void OnQuitClicked()
        {
            Application.Quit();
        }

        /// <summary>
        /// Loads the Loading Scene asynchronously.
        /// </summary>
        /// <returns></returns>
        private async Task loadLoadingScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LoadingScene");

            // Check every 10ms if Loading scene is loaded.
            while (!asyncLoad.isDone)
            {
                await Task.Delay(System.TimeSpan.FromMilliseconds(10));
            }
        }

        #endregion

    }
}