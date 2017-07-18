using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    [System.Serializable]
    public class PoolDescriptor
    {
        public Poolable m_Prefab;
        public int m_Amount;
    }

    public class PoolManager : MonoBehaviour
    {
        #region Singleton
        private static PoolManager instance;
        public static PoolManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<PoolManager>();
                }

                return instance;
            }
        }
        #endregion

        public List<PoolDescriptor> m_PoolDescriptors;

        private Dictionary<Poolable, List<Poolable>> m_Pools;
        private Dictionary<Poolable, Transform> m_PoolParents;

        private void Awake()
        {
            InitPools();
        }

        private void InitPools()
        {
            m_Pools = new Dictionary<Poolable, List<Poolable>>();
            m_PoolParents = new Dictionary<Poolable, Transform>();
            foreach (var p in m_PoolDescriptors)
            {
                var poolParent = new GameObject(p.m_Prefab.name);
                poolParent.transform.parent = transform;
                m_Pools[p.m_Prefab] = new List<Poolable>();
                m_PoolParents[p.m_Prefab] = poolParent.transform;

                foreach (var obj in CreatePoolable(p.m_Prefab, poolParent.transform, p.m_Amount))
                {
                    m_Pools[p.m_Prefab].Add(obj);
                }
            }
        }

        private IEnumerable<Poolable> CreatePoolable(Poolable prefab, Transform poolParent, int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                var go = Instantiate(prefab, poolParent);
                go.InitPool(poolParent);
                yield return go;
            }
        }

        public GameObject InstantiatePool(Poolable prefab, Transform parent = null)
        {
            var pool = m_Pools[prefab];
            if (pool == null)
            {
                throw new Exception("Pool does not exist: " + prefab.name);
            }

            var poolable = pool.DefaultIfEmpty(null).FirstOrDefault(p => !p.Active);
            if (poolable == null)
            {
                poolable = CreatePoolable(prefab, m_PoolParents[prefab]).FirstOrDefault();
                pool.Add(poolable);
            }

            poolable.Activate(parent);
            return poolable.gameObject;
        }
    }
}
