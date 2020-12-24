using UnityEngine;
using UnityEngine.UI;

namespace Case.UI.View
{
    /// <summary>
    /// Representation of Skill model data. Multiple views can 
    /// bound to same model data and shows it in different forms.
    /// SkillView and SkillTooltipView shares the same data but shows it in different forms.
    /// </summary>
    public class SkillTooltipView : MonoBehaviour
    {
        [SerializeField]
        private Text _name;

        [SerializeField]
        private Text _description;

        [SerializeField]
        private Image _icon;

        public Text SkillName { get { return _name; } }

        public Text Description { get { return _description; } }

        public Image Icon { get { return _icon; } }

    }
}