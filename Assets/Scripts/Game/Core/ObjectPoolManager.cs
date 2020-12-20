using Case.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Case.Core
{

    /// <summary>
    /// Object Pool Manager.
    /// </summary>
    public class ObjectPoolManager : SingletonComponent<ObjectPoolManager>
    {

        /// <summary>
        /// Helper class for keeping pool configs for the object pool and exposing it to Unity inspector.
        /// </summary>
        [Serializable]
        public class Pool
        {
            public string key;
            public GameObject gameObject;
            public int count;
        }

        [SerializeField]
        private List<Pool> _pools = new List<Pool>();

        /// <summary>
        /// When Progress reaches to 1.0 all default objects pools are initialized and ready to use.
        /// </summary>
        public float Progress { get; private set; }

        public bool IsReady { get { return Progress == 1.0f; } }

        private Dictionary<string, Queue<GameObject>> _objPools = new Dictionary<string, Queue<GameObject>>();

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            // Keep the PoolObjectManager for the GameScene.
            DontDestroyOnLoad(this.gameObject);

            // While initializing pools with default size for each given pair it's nice to not block the Unity's main thread,
            // since we can't create GameObjects in a worker thread, so I'll be using Coroutine instead of async-await.
            StartCoroutine(InitializePools());
        }

        #endregion

        #region ObjectPoolManager Logic

        public GameObject Get(string pool)
        {
            var item = _objPools[pool].Dequeue();
            item.SetActive(true);
            
            return item;
        }

        public bool Add(string pool, GameObject item)
        {
            bool result = _objPools.ContainsKey(pool);
            if (result)
            {
                _objPools[pool].Enqueue(item);
            }

            return result;
        }


        /// <summary>
        /// Initialize Objects Pools with the Default Size.
        /// </summary>
        private IEnumerator InitializePools()
        {
            float objectsToBeCreated = 0f;
            int createdCount = 0;

            foreach (var pool in _pools)
            {
                objectsToBeCreated += pool.count;
            }

            foreach (var pool in _pools)
            {
                _objPools[pool.key] = new Queue<GameObject>();
                var objPool = _objPools[pool.key];

                for(int i = 0; i < pool.count; ++i)
                {
                    GameObject item = Instantiate(pool.gameObject);
                    item.SetActive(false);
#if UNITY_EDITOR
                    // Changing GameObject parents are expensive I only want to do that in editor mode.
                    item.transform.parent = transform;
#endif
                    objPool.Enqueue(item);
                    
                    createdCount++;
                    if (i % 10 == 0)
                    {
                        // Let the main thread update after 10 objects initialized.
                        // Calculate the Progress so UI can give feedback to player.
                        Progress = createdCount / objectsToBeCreated;
                        yield return null;
                    }
                }
                Progress = createdCount / objectsToBeCreated;
            }
        }

        #endregion region

    }
}