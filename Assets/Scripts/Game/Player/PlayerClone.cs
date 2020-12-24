using Case.Game.Skill;
using UnityEngine;

namespace Case.Game.Character
{
    /// <summary>
    /// Player clone Script which updates the active RangedAttack strategy for the cloned player.
    /// </summary>
    public class PlayerClone : MonoBehaviour
    {
        [SerializeField]
        private PlayerAttack _attack;

        public PlayerAttack Attack { get { return _attack; } set { _attack = value; } }

        private IRangedAttack _realRangedAttack;

        #region Unity Methods

        private void OnEnable()
        {
            PlayerAttack.Attack += OnAttack;
        }

        private void OnDisable()
        {
            PlayerAttack.Attack -= OnAttack;
            _realRangedAttack.ValueChanged -= OnRangedAttackUpdated;
        }

        #endregion

        /// <summary>
        /// Event based Observer
        /// </summary>
        /// <param name="rangedAttack"> Observee </param>
        public void SubscribeToReal(IRangedAttack rangedAttack)
        {
            _realRangedAttack = rangedAttack;
            _realRangedAttack.ValueChanged += OnRangedAttackUpdated;
        }

        /// <summary>
        /// Subscribed to real player's Attack event.
        /// </summary>
        private void OnAttack()
        {
            _attack.RangedAttack.Attack();
        }

        /// <summary>
        /// Update clone's ranged attack whenever parent's ranged attack properties updated.
        /// </summary>
        private void OnRangedAttackUpdated()
        {
            _attack.RangedAttack = new DefaultRangedAttack(_realRangedAttack.ProjectileCount, _realRangedAttack.ProjectileSpeed, _realRangedAttack.AttackSpeed, _attack.RangedAttack.MuzzleTransform);
        }

    }
}