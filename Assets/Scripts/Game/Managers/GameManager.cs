using UnityEngine;
using UnityEngine.SceneManagement;

namespace Case.Scene.Manager
{

    /// <summary>
    /// GameManager handles game scene related UI events and the configurations.
    /// </summary>
    public class GameManager : MonoBehaviour
    {

        #region Unity Methods

        private void Awake()
        {
            // We need to move objects that came from the LoadingScene to active scene (GameScene) with that way
            // when we go back then start a new game we'll prevent to make multiple copies of those items.
            var createdInLoading = GameObject.FindGameObjectsWithTag("CreatedInLoading");
            foreach (var item in createdInLoading)
            {
                SceneManager.MoveGameObjectToScene(item, SceneManager.GetActiveScene());
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnEscButtonClicked();
            }
        }

        #endregion

        #region Manager Logic

        public void OnEscButtonClicked()
        {
            SceneManager.LoadScene("MainScene");
        }

        #endregion

    }
}