using Case.UI.Model;
using Case.UI.View;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Case.UI.Controller
{

    /// <summary>
    /// Creates visual representation of the model data using view delegates.
    /// Usually accepts input and converts it to commands for the model or view updates
    /// but for the given study case I'll keep that as simple as possible.
    /// </summary>
    public class SkillSetController : MonoBehaviour
    {
        [SerializeField]
        private SkillSetModel _model;

        [SerializeField]
        private GameObject _viewDelegate;

        [SerializeField]
        private HorizontalLayoutGroup _list;

        #region Unity Methods

        private void Awake()
        {
            StartCoroutine(InitializeViews());
        }

        #endregion

        #region Controller Logic

        /// <summary>
        /// It's not recommended to block Unity's main thread so, I'll initialize
        /// views in a coroutine (even if we only have 6 items in our model and it's gonna blocked anyway because
        /// I'll let the main thread go for every 10 objects), also if the View objects are heavy we better
        /// initialize them lazily (Lazy Initialization Pattern) but for the given study case I'll use eager initialization.
        /// </summary>
        private IEnumerator InitializeViews()
        {
            for (int i = 0; i < _model.items.Count; ++i)
            {
                var data = _model.items[i];
                var view = Instantiate(_viewDelegate, _list.transform)?.GetComponent<SkillView>();
                view.name = data.name;
                view.enabled = data.enabled;
                view.Description = data.description;
                view.Icon.sprite = data.sprite;
                view.signal = data.signal;
                view.ModelIndex = i;

                // Set created toggle to Model item.
                data.button = view.button;

                // Let the main thread update after 10 objects initialized.
                if (i % 10 == 0)
                {
                    yield return null;
                }
            }
        }

        #endregion

    }
}