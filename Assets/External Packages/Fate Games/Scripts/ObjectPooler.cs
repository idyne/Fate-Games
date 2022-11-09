using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames
{
    public static class ObjectPooler
    {
        private struct Pool
        {
            private Queue<GameObject> instances;
            private bool canActiveObjectsBeDequeued;

            public Pool(Queue<GameObject> instances, bool canActiveObjectsBeDequeued)
            {
                this.instances = instances;
                this.canActiveObjectsBeDequeued = canActiveObjectsBeDequeued;
            }

            public Queue<GameObject> Instances { get => instances; }
            public bool CanActiveObjectsBeDequeued { get => canActiveObjectsBeDequeued; }
        }

        private static Dictionary<string, Pool> poolDictionary;
        public static void CreatePools()
        {
            List<PoolData> pools = Resources.Load<PoolDataTable>("Fate Games/ScriptableObjects/PoolDataTables/Essential Pool Data").PoolData;
            List<PoolData> gamePools = Resources.Load<PoolDataTable>("Fate Games/ScriptableObjects/PoolDataTables/Game Pool Data").PoolData;
            pools.AddRange(gamePools);
            if (pools.Count == 0) return;
            poolDictionary = new Dictionary<string, Pool>();
            Transform container = new GameObject("Pool Container").transform;
            container.position = Vector3.up * 100;
            foreach (PoolData poolData in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();
                Transform poolObj = new GameObject(poolData.tag + " Pool").transform;
                poolObj.parent = container;
                poolObj.transform.localPosition = Vector3.zero;
                for (int i = 0; i < poolData.size; i++)
                {
                    GameObject obj = Object.Instantiate(poolData.prefab, poolObj);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                Pool pool = new Pool(objectPool, poolData.canActiveObjectsBeDequeued);
                poolDictionary.Add(poolData.tag, pool);
            }
        }
        public static GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
                return null;
            }
            GameObject objectToSpawn;
            Pool pool = poolDictionary[tag];
            objectToSpawn = pool.Instances.Dequeue();
            pool.Instances.Enqueue(objectToSpawn);
            if (!pool.CanActiveObjectsBeDequeued)
            {

                int i = pool.Instances.Count;
                while (i-- > 0 && objectToSpawn.activeSelf)
                {
                    objectToSpawn = pool.Instances.Dequeue();
                    pool.Instances.Enqueue(objectToSpawn);
                }
                if (objectToSpawn.activeSelf)
                {
                    Debug.LogError("All instances of \"" + tag + "\" are currently active!");
                    return null;
                }
            }
            DOTween.Kill(objectToSpawn);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.SetActive(true);
            IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
            if (pooledObj != null)
                pooledObj.OnObjectSpawn();
            return objectToSpawn;
        }
        [System.Serializable]
        public class PoolData
        {
            public string tag;
            public GameObject prefab;
            public int size;
            public bool canActiveObjectsBeDequeued = false;
        }
    }
}