using Case.Game.Character;
using UnityEngine;

namespace Case.Game.Controller
{

    /// <summary>
    /// Zoom the camera when the player goes insane
    /// </summary>
    public class CameraController : MonoBehaviour
    {

        [SerializeField]
        private Animator _animator;

        #region Unity Methods

        private void OnEnable()
        {
            Player.PlayerStateChanged += OnPlayerStateChanged;
        }

        private void OnDisable()
        {
            Player.PlayerStateChanged -= OnPlayerStateChanged;
        }

        #endregion

        #region Controller Logic

        private void OnPlayerStateChanged(Player.PlayerState oldState, Player.PlayerState newState)
        {
            // Going Insane and Setoff makes the zoom
            _animator.SetBool("ZoomCamera", ((oldState == Player.PlayerState.Ready && newState == Player.PlayerState.None)
                || (oldState == Player.PlayerState.Insane && newState == Player.PlayerState.None)));
        }

        #endregion
    }
}