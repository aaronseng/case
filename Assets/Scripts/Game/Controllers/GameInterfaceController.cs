using Case.Game.Character;
using Case.UI.Model;
using Case.UI.View;
using UnityEngine;

namespace Case.Game.Controller
{

    /// <summary>
    /// Controls the interface related logic. That module doesn't handle the inputs !
    /// </summary>
    public class GameInterfaceController : MonoBehaviour
    {
        private const string ANIM_SHOWINTERFACE = "ShowGameInterface";

        [SerializeField]
        private Animator _skillBarAnimator;

        [SerializeField]
        private SkillSetModel _skillSetmodel;

        private int _activeSupportCount = 0;

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

        public void OnMultipleProjectilesClicked(int modelIndex)
        {
            _skillSetmodel.items[modelIndex].button.interactable = false;
            CheckActiveButtons();
        }

        public void OnEchoClicked(int modelIndex)
        {
            _skillSetmodel.items[modelIndex].button.interactable = false;
            CheckActiveButtons();
        }

        public void OnFasterAttackClicked(int modelIndex)
        {
            _skillSetmodel.items[modelIndex].button.interactable = false;
            CheckActiveButtons();
        }

        public void OnFasterProjectileClicked(int modelIndex)
        {
            _skillSetmodel.items[modelIndex].button.interactable = false;
            CheckActiveButtons();
        }

        public void OnCloneClicked(int modelIndex)
        {
            _skillSetmodel.items[modelIndex].button.interactable = false;
            CheckActiveButtons();
        }

        /// <summary>
        /// Checks the Active Skill Button states, if there is 3 active supports disable rest of the buttons, except Insane button.
        /// </summary>
        private void CheckActiveButtons()
        {
            _activeSupportCount++;
            if (_activeSupportCount == 3)
            {
                // (We can also add an another field to Model data for specializing skills, for the simplicity
                // I'll just keep the last element as Insane skill for this study case.)
                for (int i = 0; i < _skillSetmodel.items.Count - 1; i++)
                {
                    _skillSetmodel.items[i].button.interactable = false;
                }
            }
        }

        private void OnPlayerStateChanged(Player.PlayerState oldState, Player.PlayerState newState)
        {
            if (newState == Player.PlayerState.Ready)
            {
                Clear();
                _skillBarAnimator.SetBool(ANIM_SHOWINTERFACE, true);
            }
            else if (newState == Player.PlayerState.Insane || newState == Player.PlayerState.None)
            {
                _skillBarAnimator.SetBool(ANIM_SHOWINTERFACE, false);
            }
        }

        private void Clear()
        {
            _activeSupportCount = 0;
            for (int i = 0; i < _skillSetmodel.items.Count; i++)
            {
                _skillSetmodel.items[i].button.gameObject.GetComponent<SkillView>()?.Clear();
                _skillSetmodel.items[i].button.interactable = true;
            }
        }

        #endregion

    }
}