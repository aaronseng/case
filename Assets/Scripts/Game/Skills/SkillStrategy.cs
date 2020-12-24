using Case.Core;
using Case.Game.Character;
using Case.Game.Controller;
using System.Diagnostics.Tracing;
using UnityEngine;

namespace Case.Game.Skill
{
    /// <summary>
    /// Skill Strategy interface.
    /// </summary>
    public interface ISkillStrategy
    {
        void Perform();
    }

    /// <summary>
    /// Skill class. 
    /// </summary>
    public class Skill
    {
        public ISkillStrategy Strategy { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="strategy">The strategy (skill logic) of the corresponding skill.</param>
        public Skill(ISkillStrategy strategy)
        {
            Strategy = strategy;
        }

        public void Perform()
        {
            Strategy.Perform();
        }
    }

    /// <summary>
    /// Modifies the current ranged attack logic. Fires [n] arrows at the same time.
    /// </summary>
    public class SupportWithMultipleProjectiles : ISkillStrategy
    {
        public const int DEFAULT_MODIFIER = 3;

        private readonly int _modifier;

        public SupportWithMultipleProjectiles(int numberOfProjectiles = DEFAULT_MODIFIER)
        {
            _modifier = numberOfProjectiles;
        }

        public void Perform()
        {
            PlayerController.Instance.Player.Attack.RangedAttack.ProjectileCount = _modifier;
        }
    }

    /// <summary>
    /// Decorates the current ranged attack logic with Echo Support.
    /// </summary>
    public class SupportWithEcho : ISkillStrategy
    {
        public void Perform()
        {
            PlayerController.Instance.Player.Attack.RangedAttack = new EchoAttack(PlayerController.Instance.Player.Attack.RangedAttack);
        }
    }

    /// <summary>
    /// Modifies the current ranged attack speed with the modifier.
    /// </summary>
    public class SupportWithFasterAttack : ISkillStrategy
    {
        public const float DEFAULT_MODIFIER = 2f;

        private readonly float _modifier;

        public SupportWithFasterAttack(float attackSpeedModifier = DEFAULT_MODIFIER)
        {
            _modifier = attackSpeedModifier;
        }

        public void Perform()
        {
            PlayerController.Instance.Player.Attack.RangedAttack.AttackSpeed /= _modifier;
        }
    }

    /// <summary>
    /// Modifies the current projectile speed with the modifier.
    /// </summary>
    public class SupportWithFasterProjectile : ISkillStrategy
    {
        public const int DEFAULT_MODIFIER = 2;

        private readonly float _modifier;

        public SupportWithFasterProjectile(float projectileSpeedModifier = DEFAULT_MODIFIER)
        {
            _modifier = projectileSpeedModifier;
        }

        public void Perform()
        {
            PlayerController.Instance.Player.Attack.RangedAttack.ProjectileSpeed *= _modifier;
        }
    }

    /// <summary>
    /// Creates a shadow clone of the player.
    /// </summary>
    public class KageBunshin : ISkillStrategy
    {
        private readonly Vector2 MIN = new Vector2(-7f, 20f);
        private readonly Vector2 MAX = new Vector2(7f, 34f);

        public void Perform()
        {
            var realRangedAttack = PlayerController.Instance.Player.Attack.RangedAttack;
            var clone = ObjectPoolManager.Instance.Get(ObjectPoolManager.CHARACTER);
            PlayerController.Instance.Clone = clone;
            clone.transform.position = new Vector3(Random.Range(MIN.x, MAX.x), clone.transform.position.y, Random.Range(MIN.y, MAX.y));
            var clonedPlayer = clone.GetComponent<PlayerClone>();
            clonedPlayer.SubscribeToReal(realRangedAttack);
            
            // Deep copy parent's ranged attack properties so we can have independent attack characteristcs.
            clonedPlayer.Attack.RangedAttack = new DefaultRangedAttack(realRangedAttack.ProjectileCount, realRangedAttack.ProjectileSpeed, realRangedAttack.AttackSpeed, clonedPlayer.Attack.RangedAttack.MuzzleTransform);
            clonedPlayer.enabled = true;
        }
    }
}