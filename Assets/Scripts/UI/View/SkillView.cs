using Case.Core;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Case.UI.View
{

    /// <summary>
    /// Representation of model data. Usually bounds model data to UI elements
    /// whenever model data changes View automatically updates its own representation
    /// but for the given study case I'll keep that as simple as possible,
    /// so auto update feature won't be implemented.
    /// </summary>
    public class SkillView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static event Action<int, float> ShowTooltip;
        public static event Action HideTooltip;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private Button _button;

        [SerializeField]
        private GameObject _checkmark;

        public Image Icon { get { return _icon; } }

        public Button button { get { return _button; } }

        public GameObject Checkmark { get { return _checkmark; } }

        public Signal signal { get; set; }

        public int ModelIndex { get; set; }

        public string Description { get; set; }

        #region Unity Methods

        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowTooltip?.Invoke(ModelIndex, eventData.position.x);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HideTooltip?.Invoke();
        }

        #endregion

        #region View Logic

        /// <summary>
        /// Emits a scriptable object base signal when the view is clicked.
        /// </summary>
        public void OnViewClicked()
        {
            _checkmark.SetActive(true);
            signal.value = ModelIndex;
            signal.Emit();
        }

        /// <summary>
        /// Disables the checkmark view element.
        /// </summary>
        public void Clear()
        {
            _checkmark.SetActive(false);
        }

        #endregion
    }
}