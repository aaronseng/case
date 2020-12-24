using Case.Game.Skill;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Case.Game.Character
{

    /// <summary>
    /// Player script that controls the Player States, informs the other
    /// components about State changes. It's also used to control Animator state machine.
    /// </summary>
    public class Player : MonoBehaviour
    {
        public enum PlayerState
        {
            None, Idle, Ready, Insane, Dead
        }

        public static event Action<PlayerState, PlayerState> PlayerStateChanged;

        [SerializeField]
        private Transform _spawnPoint;

        [SerializeField]
        private Transform _targetPoint;

        [SerializeField]
        private PlayerMovement _movement;

        [SerializeField]
        private PlayerAttack _attack;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private Transform _muzzle;

        public PlayerAttack Attack { get { return _attack; } }

        public Transform Muzzle { get { return _muzzle; } }

        private readonly List<string> _readyAnimations = new List<string>()
        {
            "Walking", "Skipping", "Flying", "Crawling"
        };

        private readonly List<string> _insaneAnimations = new List<string>()
        {
            "Laughing", "AirGuitar", "MoonWalk"
        };

        private readonly List<Vector3> _insaneCoords = new List<Vector3>()
        {
            new Vector3(-3.6f , 0.5f, 5.5f), new Vector3(-14.2f , 0.5f, 25.2f), new Vector3(1.7f, 0.5f, 42.2f), new Vector3(13.5f, 0.5f, 25.8f)
        };

        private PlayerState _characterState = PlayerState.None;

        private int _insaneIndex;

        #region Unity Methods

        private void Awake()
        {
            // Set the default ranged attack
            _attack.RangedAttack = new DefaultRangedAttack(1, 100f, 2f, _muzzle);
        }

        private void OnEnable()
        {
            PlayerStateChanged += OnPlayerStateChanged;
        }

        private void OnDisable()
        {
            PlayerStateChanged -= OnPlayerStateChanged;
        }

        #endregion

        #region Character Logic

        /// <summary>
        /// Brings the player starting position with a random animation.
        /// </summary>
        public void GetReady()
        {
            PlayerStateChanged?.Invoke(_characterState, PlayerState.None);

            var index = Random.Range(0, _readyAnimations.Count);
            _animator.SetBool(_readyAnimations[index], true);

            _movement.MoveTo(_spawnPoint.position, _targetPoint.position).OnCompleted(() =>
            {
                PlayerStateChanged?.Invoke(_characterState, PlayerState.Ready);
                _animator.SetBool(_readyAnimations[index], false);
            });
        }

        /// <summary>
        /// Bonus feature makes the player go wild.
        /// </summary>
        public void GoInsane()
        {
            PlayerStateChanged?.Invoke(_characterState, PlayerState.None);
            _insaneIndex = Random.Range(0, _insaneAnimations.Count);
            _animator.SetBool(_insaneAnimations[_insaneIndex], true);
        }

        /// <summary>
        /// Insane State Logic
        /// </summary>
        /// <returns></returns>
        private async Task Insane()
        {
            _attack.RangedAttack.ProjectileCount = 7;
            _attack.RangedAttack.AttackSpeed = 0.5f;
            PlayerStateChanged?.Invoke(_characterState, PlayerState.Insane);

            _animator.SetBool("Skipping", true);

            _movement.StartSpin();
            
            var coords = new List<Vector3>(_insaneCoords.ToArray());

            // Take points in a random order for every time when player go insane.
            for (int i = 0; i < 4; ++i)
            {
                var isCompleted = false;
                var idx = Random.Range(0, coords.Count);
                _movement.MoveTo(coords[idx], false).OnCompleted(() => {
                    isCompleted = true;
                });
                
                // Wait until player reaches the the destination point
                while (!isCompleted)
                {
                    await Task.Delay(10);
                }

                // Remove the point so we don't select the same point again
                coords.RemoveAt(idx);
            }

            _movement.StopSpin();
            _animator.SetBool("Skipping", false);
            PlayerStateChanged?.Invoke(_characterState, PlayerState.None);

            // Leave the screen then reset
            _attack.RangedAttack = new DefaultRangedAttack(1, 100f, 2f, _muzzle);
            _animator.SetBool("Setoff", true);
            await Task.Delay(2000);

            _movement.Setoff();
            await Task.Delay(3000);
            _animator.SetBool("Setoff", false);
            GetReady();
        }

        /// <summary>
        /// Let the player attacks.
        /// </summary>
        public void StartAttacking()
        {
            _attack.enabled = true;
        }

        /// <summary>
        /// Stops the Player attack.
        /// </summary>
        public void StopAttacking()
        {
            _attack.enabled = false;
        }

        /// <summary>
        /// Triggered by Animator state machine, let us know when the Insane animation completed.
        /// </summary>
        public async void OnInsaneAnimationCompleted()
        {
            _animator.SetBool(_insaneAnimations[_insaneIndex], false);
            await Insane();
        }

        /// <summary>
        /// Updates its own current State.
        /// </summary>
        /// <param name="oldState">Old character state</param>
        /// <param name="newState">New Character state</param>
        private void OnPlayerStateChanged(PlayerState oldState, PlayerState newState)
        {
            _characterState = newState;
        }

        #endregion
    }
}