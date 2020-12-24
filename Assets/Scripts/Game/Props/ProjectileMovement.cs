using Case.Core;
using UnityEngine;

namespace Case.Game.Props
{

    /// <summary>
    /// Simple object movement script. When the object reaches the target position
    /// active object puts itself back to object pool.
    /// </summary>
    public class ProjectileMovement : MonoBehaviour
    {

        [SerializeField]
        private float _maxDistance;

        [SerializeField]
        private float _moveSpeed;

        #region Properties

        public float MoveSpeed
        {
            get
            {
                return _moveSpeed;
            }
            set
            {
                _moveSpeed = value;
            }
        }

        public float MaxDistance
        {
            get
            {
                return _maxDistance;
            }
            set
            {
                _maxDistance = value;
            }
        }

        #endregion

        private Vector3 _target;

        #region #Unity Methods

        private void OnEnable()
        {
            // Finds the target position regarding to player's transformation.
            _target = transform.position + Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * Vector3.forward * _maxDistance;
        }

        private void Update()
        {
            if (transform.position == _target)
            {
                ObjectPoolManager.Instance.Add(ObjectPoolManager.PROJECTILE, gameObject);
            }

            transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * _moveSpeed);
        }

        #endregion

    }
}