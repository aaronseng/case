using System;
using System.Collections;
using UnityEngine;

namespace Case.Game.Character
{

    #region DOTWEENER LIKE CALLBACK SUPPORT

    /// <summary>
    /// Very simple DOTween like Callback support for the character movement.
    /// I implemented this for a demonstration purpose.
    /// </summary>
    public class MovementResult
    {
        public Action Abort { get; set; }
        public Action Completed { get; set; }
    }

    /// <summary>
    /// Extension methods
    /// </summary>
    public static class MovementResultExtensions
    {
        public static MovementResult OnAbort(this MovementResult result, Action callback)
        {
            result.Abort = callback;
            return result;
        }

        public static MovementResult OnCompleted(this MovementResult result, Action callback)
        {
            result.Completed = callback;
            return result;
        }
    }

    #endregion

    /// <summary>
    /// Simple character movement script.
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {

        [SerializeField]
        private float _moveSpeed = 20f;

        [SerializeField]
        private float _turnSpeed = 20f;

        private Vector3 _target;
        private IEnumerator _moveRoutine;
        private MovementResult _result;
        private IEnumerator _spinRoutine;

        #region Movement Logic

        /// <summary>
        /// Translates the game object from current position to target position.
        /// </summary>
        /// <param name="to"> Target position</param>
        /// <returns> MovementResult simple callback container. </returns>
        public MovementResult MoveTo(Vector3 to, bool rotateTowards = true)
        {
            if (_moveRoutine != null)
            {
                StopCoroutine(_moveRoutine);
                _result.Abort?.Invoke();
            }
            _target = to;
            _result = new MovementResult();
            _moveRoutine = Move(_result, rotateTowards);

            StartCoroutine(_moveRoutine);

            return _result;
        }

        /// <summary>
        /// Translates the game object from desired position to target position.
        /// </summary>
        /// <param name="from"> Desired position </param>
        /// <param name="to"> Target position </param>
        /// <returns> MovementResult simple callback container. </returns>
        public MovementResult MoveTo(Vector3 from, Vector3 to, bool rotateTowards = true)
        {
            transform.position = from;
            return MoveTo(to, rotateTowards);
        }

        /// <summary>
        /// Player objects start spining around itself.
        /// </summary>
        public void StartSpin()
        {
            _spinRoutine = Spin();
            StartCoroutine(_spinRoutine);
        }

        /// <summary>
        /// Stops the spin.
        /// </summary>
        public void StopSpin()
        {
            StopCoroutine(_spinRoutine);
        }

        /// <summary>
        /// Leaves the screen in 2 seconds.
        /// </summary>
        public void Setoff()
        {
            StartCoroutine(MoveUp(2f));
        }

        private IEnumerator MoveUp(float seconds)
        {
            float elapsedTime = 0;
            Vector3 startingPos = transform.position;
            Vector3 target = transform.position;
            target.y += 30f;
            while (elapsedTime < seconds)
            {
                transform.position = Vector3.Lerp(startingPos, target, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// Rotates the Player object around itself with random speed.
        /// </summary>
        private IEnumerator Spin()
        {
            while (true)
            {
                transform.Rotate(0, UnityEngine.Random.Range(360, 720) * Time.deltaTime, 0);
                yield return null;
            }
        }

        /// <summary>
        /// Translates the game objects
        /// </summary>
        /// <param name="result"> Callback for movement completed </param>
        private IEnumerator Move(MovementResult result, bool rotateTowards)
        {
            while (_target != transform.position)
            {
                var direction = (_target - transform.position).normalized;
                transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * _moveSpeed);
                if (rotateTowards)
                {
                    RotateTowards(direction);
                }
                yield return null;
            }
            _moveRoutine = null;
            result.Completed?.Invoke();
        }

        /// <summary>
        /// Rotates the game object towards target
        /// </summary>
        /// <param name="direction"> Towards direction </param>
        private void RotateTowards(Vector3 direction)
        {
            if (direction == Vector3.zero)
            {
                return;
            }

            Quaternion target = Quaternion.LookRotation(direction);
            Quaternion desiredRotation = Quaternion.Slerp(transform.rotation, target, _turnSpeed * Time.deltaTime);

            Vector3 euler = desiredRotation.eulerAngles;
            euler.z = 0;
            euler.x = 0;
            desiredRotation = Quaternion.Euler(euler);

            transform.rotation = desiredRotation;
        }

        #endregion
    }
}