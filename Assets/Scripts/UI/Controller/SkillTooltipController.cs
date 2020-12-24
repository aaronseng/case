using Case.UI.Model;
using Case.UI.View;
using System;
using UnityEngine;

namespace Case.UI.Controller
{
    /// <summary>
    /// Unlike SkillSetController instead of creating its own views SkillTooltipController uses
    /// already created view from Unity scene which is SkillTooltip.
    /// It will be used for accepting inputs from Unity EventSystem to show, hide and change the content.
    /// </summary>
    public class SkillTooltipController : MonoBehaviour
    {

        [SerializeField]
        private SkillSetModel _model;

        [SerializeField]
        private SkillTooltipView _view;

        [SerializeField]
        private RectTransform _tooltip;

        [SerializeField]
        private RectTransform _skillBar;

        private Vector2 _tooltipPos = new Vector2();

        #region Unity Methods

        private void OnEnable()
        {
            SkillView.ShowTooltip += OnShowTooltip;
            SkillView.HideTooltip += OnHideTooltip;
        }

        private void OnDisable()
        {
            SkillView.ShowTooltip -= OnShowTooltip;
            SkillView.HideTooltip -= OnHideTooltip;
        }

        #endregion

        #region Controller Logic

        private void OnShowTooltip(int index, float posX)
        {
            // Load model data to Tooltip view
            var data = _model.items[index];
            _view.SkillName.text = data.name;
            _view.Description.text = data.description;
            _view.Icon.sprite = data.sprite;

            _tooltipPos.x = posX - _tooltip.sizeDelta.x / 2;
            _tooltipPos.y = _skillBar.rect.height;
            _tooltip.anchoredPosition = _tooltipPos;
            _tooltip.gameObject.SetActive(true);
        }

        private void OnHideTooltip()
        {
            // Hide the Tooltip
            _tooltip.gameObject.SetActive(false);
        }

        #endregion

    }
}