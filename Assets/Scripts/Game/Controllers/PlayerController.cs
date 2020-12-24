
using Case.Core;
using Case.Game.Character;
using Case.Game.Skill;
using Case.Utility;
using UnityEngine;

namespace Case.Game.Controller
{

    /// <summary>
    /// Handle the player inputs and controls the player's behaviour.
    /// </summary>
    public class PlayerController : SingletonComponent<PlayerController>
    {

        [SerializeField]
        private Player _player;

        public Player @Player { get { return _player; } }

        public GameObject Clone { get; set; }

        #region Unity Methods

        private void Start()
        {
            _player.GetReady();
        }

        private void OnEnable()
        {
            Player.PlayerStateChanged += OnPlayerStateChanged;
        }

        private void OnDisable()
        {
            Player.PlayerStateChanged -= OnPlayerStateChanged;
            DestroyClone();
        }

        #endregion

        #region Controller Logic

        /// <summary>
        /// Sets multiple projectile logic to character when the skill button clicked.
        /// </summary>
        /// <param name="modelIndex"> View's model index. </param>
        public void OnMultipleProjectilesClicked(int modelIndex)
        {
            var strategy = new SupportWithMultipleProjectiles();
            strategy.Perform();
        }

        /// <summary>
        /// Sets echo logic to character when the skill button clicked.
        /// </summary>
        /// <param name="modelIndex"> View's model index. </param>
        public void OnEchoClicked(int modelIndex)
        {
            var strategy = new SupportWithEcho();
            strategy.Perform();
        }

        /// <summary>
        /// Sets faster attack logic to character when the skill button clicked.
        /// </summary>
        /// <param name="modelIndex"> View's model index. </param>
        public void OnFasterAttackClicked(int modelIndex)
        {
            var strategy = new SupportWithFasterAttack();
            strategy.Perform();
        }

        /// <summary>
        /// Sets faster projectile logic to character when the skill button clicked.
        /// </summary>
        /// <param name="modelIndex"> View's model index. </param>
        public void OnFasterProjectileClicked(int modelIndex)
        {
            var strategy = new SupportWithFasterProjectile();
            strategy.Perform();
        }

        /// <summary>
        /// Casts clone spell when the skill button clicked.
        /// </summary>
        /// <param name="modelIndex"> View's model index. </param>
        public void OnCloneClicked(int modelIndex)
        {
            var strategy = new KageBunshin();
            strategy.Perform();
        }

        /// <summary>
        /// Casts insane spell when the skill button clicked.
        /// </summary>
        /// <param name="modelIndex"> View's model index. </param>
        public void OnInsaneClicked(int modelIndex)
        {
            DestroyClone();
            _player.GoInsane();
        }

        /// <summary>
        /// Starts or stop attacking regarding to chracter's state
        /// </summary>
        /// <param name="oldState"> Old player state </param>
        /// <param name="newState"> New player state </param>
        private void OnPlayerStateChanged(Player.PlayerState oldState, Player.PlayerState newState)
        {
            if (newState == Player.PlayerState.Ready || newState == Player.PlayerState.Insane)
            {
                _player.StartAttacking();
            }
            else
            {
                _player.StopAttacking();
            }
        }

        /// <summary>
        /// Gives clones character back to pool.
        /// </summary>
        private void DestroyClone()
        {
            if (Clone != null)
            {
                ObjectPoolManager.Instance.Add(ObjectPoolManager.CHARACTER, Clone);
                Clone = null;
            }
        }

        #endregion

    }
}