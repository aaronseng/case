using Case.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Case.UI.Model
{
    /// <summary>
    /// It is the game's dynamic skill data structure, 
    /// Usually it directly manages the data and represents it to consumers,
    /// Informs consumers when its data changed, but for the given study case
    /// I'll keep that as simple as possible, so inform feature won't be implemented.
    /// </summary>
    [CreateAssetMenu(fileName = "SkillSetModel", menuName = "Model/Skill Set")]
    public class SkillSetModel : ScriptableObject
    {

        /// <summary>
        /// Skill model for the view.
        /// </summary>
        [Serializable]
        public class Skill
        {
            public string name;
            public bool enabled;
            public Sprite sprite;
            public string description;
            public Signal signal;

            public Button button { get; set; }
        }

        public List<Skill> items = new List<Skill>();

    }
}