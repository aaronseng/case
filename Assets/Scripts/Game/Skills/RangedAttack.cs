using Case.Core;
using Case.Game.Props;
using System;
using UnityEngine;

namespace Case.Game.Skill
{

    /// <summary>
    /// Ranged Attack interface.
    /// </summary>
    public interface IRangedAttack
    {
        /// <summary>
        /// Event based observer pattern
        /// </summary>
        event Action ValueChanged;

        /// <summary>
        /// Returns the attack speed.
        /// </summary>
        float AttackSpeed { get; set; }

        /// <summary>
        /// Returns the projectile count.
        /// </summary>
        int ProjectileCount { get; set; }

        /// <summary>
        /// Return the projectile speed.
        /// </summary>
        float ProjectileSpeed { get; set; }

        /// <summary>
        /// Returns the Muzzle transformation of the Player which will be used
        /// for starting position for Projectiles.
        /// </summary>
        Transform MuzzleTransform { get; }

        /// <summary>
        /// Updates the RangedAttack timer.
        /// </summary>
        /// <param name="deltaTime">The completion time in seconds since the last frame.</param>
        /// <returns></returns>
        bool UpdateTimer(float deltaTime);

        void Attack();
    }

    /// <summary>
    /// Default RangedAttack script which can be further decorated.
    /// </summary>
    public class DefaultRangedAttack : IRangedAttack
    {
        public event Action ValueChanged;

        private const float SPREADING_ANGLE = 30f;

        #region Properties

        public float AttackSpeed
        {
            get { return _attackSpeed; }
            set
            {
                if (_attackSpeed != value)
                {
                    _attackSpeed = value;
                    ValueChanged?.Invoke();
                }
            }
        }

        public int ProjectileCount
        {
            get { return _projectileCount; }
            set
            {
                if (_projectileCount != value)
                {
                    _projectileCount = value;
                    ValueChanged?.Invoke();
                }
            }
        }

        public float ProjectileSpeed
        {
            get { return _projectileSpeed; }
            set
            {
                if (_projectileSpeed != value)
                {
                    _projectileSpeed = value;
                    ValueChanged?.Invoke();
                }
            }
        }

        public Transform MuzzleTransform { get; }

        #endregion

        private float _attackSpeed;
        private float _projectileSpeed;
        private int _projectileCount;

        private float _timer;

        /// <summary>
        /// Ctor of the DefaultRangedAttack Script.
        /// </summary>
        /// <param name="projectileCount"> Number of projectiles that can be fired by the DefaultRangedAttack script. </param>
        /// <param name="projectileSpeed"> Default projectile speed. </param>
        /// <param name="attackspeed"> Default attack speed. </param>
        /// <param name="muzzle"> Muzzle position where Projectiles to be fired. </param>
        public DefaultRangedAttack(int projectileCount, float projectileSpeed, float attackspeed, Transform muzzle)
        {
            _attackSpeed = attackspeed;
            _projectileSpeed = projectileSpeed;
            _projectileCount = projectileCount;

            MuzzleTransform = muzzle;
        }

        /// <summary>
        /// Attack logic.
        /// </summary>
        public void Attack()
        {
            var spread = _projectileCount > 1 ? SPREADING_ANGLE  * _projectileCount : 0;
            var step = _projectileCount > 1  ? spread / (_projectileCount - 1) : 0;
            var halfSpread = spread / 2;

            for (int i = 0; i < _projectileCount; ++i)
            {
                var projectile = ObjectPoolManager.Instance.Get(ObjectPoolManager.PROJECTILE, false);
                var movement = projectile.GetComponent<ProjectileMovement>();

                movement.MoveSpeed = _projectileSpeed;
                
                projectile.transform.position = MuzzleTransform.position;
                projectile.transform.rotation = MuzzleTransform.rotation * Quaternion.Euler(0, (halfSpread - (step * i)), 0);

                projectile.SetActive(true);
            }
        }

        public bool UpdateTimer(float deltaTime)
        {
            _timer += deltaTime;

            if (_timer >= AttackSpeed)
            {
                _timer = 0;
                return true;
            }
            return false;
        }

    }

    /// <summary>
    /// The Decorator script for the RangedAttack logic.
    /// </summary>
    public abstract class RangedAttackDecorator : IRangedAttack
    {
        public event Action ValueChanged;
        
        protected IRangedAttack _rangedAttack;

        #region Properties

        public float AttackSpeed 
        { 
            get {  return _rangedAttack.AttackSpeed; }
            set
            {
                if (_rangedAttack.AttackSpeed != value)
                {
                    _rangedAttack.AttackSpeed = value;
                    ValueChanged?.Invoke();
                }
            }
        }

        public int ProjectileCount 
        { 
            get {  return _rangedAttack.ProjectileCount; }
            set
            {
                if (_rangedAttack.ProjectileCount != value)
                {
                    _rangedAttack.ProjectileCount = value;
                    ValueChanged?.Invoke();
                }
            }
        }

        public float ProjectileSpeed 
        {
            get { return _rangedAttack.ProjectileSpeed; }
            set
            {
                if (_rangedAttack.ProjectileSpeed != value)
                {
                    _rangedAttack.ProjectileSpeed = value;
                    ValueChanged?.Invoke();
                }
            }
        }

        #endregion

        public Transform MuzzleTransform { get { return _rangedAttack.MuzzleTransform; } }

        public RangedAttackDecorator(IRangedAttack rangedAttack)
        {
            _rangedAttack = rangedAttack;
        }

        public virtual void Attack()
        {
            _rangedAttack.Attack();
        }

        public virtual bool UpdateTimer(float deltaTime)
        {
            return _rangedAttack.UpdateTimer(deltaTime);
        }
    }

    /// <summary>
    /// Decorates the RangedAttack that makes it sequentially attacks two times.
    /// </summary>
    public class EchoAttack : RangedAttackDecorator
    {
        private const float ECHO_SPEED = 0.15f;

        private float _echoElapsed = 0;
        private bool _canEcho = false;

        public EchoAttack(IRangedAttack rangedAttack) : base(rangedAttack)
        {
        }

        public override bool UpdateTimer(float deltaTime)
        {
            _echoElapsed += deltaTime;

            if (base.UpdateTimer(deltaTime))
            {
                _canEcho = true;
                _echoElapsed = 0;
                return true;
            }
            else if(_canEcho && _echoElapsed >= ECHO_SPEED)
            {
                _canEcho = false;
                _echoElapsed = 0;
                return true;
            }
            return false;
        }
    }

}