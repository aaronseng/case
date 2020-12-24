using Case.Game.Skill;
using System;
using UnityEngine;

namespace Case.Game.Character
{
    /// <summary>
    /// Player Attack Script which updates the active ranged attack strategy
    /// </summary>
    public class PlayerAttack : MonoBehaviour
    {
        public static event Action Attack;

        public IRangedAttack RangedAttack { get; set; }

        #region Unity Methods

        private void Update()
        {
            if (RangedAttack.UpdateTimer(Time.deltaTime))
            {
                RangedAttack.Attack();
                Attack?.Invoke();
            }
        }

        #endregion
    }
}