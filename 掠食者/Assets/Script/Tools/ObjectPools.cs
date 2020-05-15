using System.Collections.Generic;
using UnityEngine;

public class ObjectPools : Singleton<ObjectPools>
{
    [System.Serializable]
    public class Pool
    {
        public string name;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        GetPools();
    }

    protected virtual void GetPools()
    {
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.name = pool.name;
                obj.transform.SetParent(this.transform);
                obj.transform.localPosition = Vector3.zero;
                obj.SetActive(false);
                objPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.name, objPool);
        }
    }

    public GameObject DamagePopup(string name, bool isCritical, int damageAmount, Vector3 position, float damageDirectionX)
    {
        if (poolDictionary == null || !poolDictionary.ContainsKey(name))
            return null;

        GameObject objectToSpawn = poolDictionary[name].Dequeue();

        IDamageGenerator pooledObj = objectToSpawn.GetComponent<IDamageGenerator>();

        if (pooledObj != null)
        {
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            pooledObj.SetupDamage(isCritical, damageAmount);
        }

        poolDictionary[name].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
