﻿using UnityEngine;

namespace Case.Utility
{

    /// <summary>
    /// Classes derived from this class become singleton.
    /// </summary>
    public abstract class SingletonComponent<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T _instance = null;

        /// <summary>
        /// Returns the singleton instance if it's already created.
        /// If we can't find an already created Singleton instance returns 'null'.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }
                return _instance;
            }
        }

        #region Unity Methods

        protected virtual void Awake()
        {
            // if we try to create second instance of a Singleton component throw an exception.
            if (Instance != null && Instance != this)
            {
                throw new System.Exception("Singleton instance is already exists.");
            }
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }

        #endregion

    }
}