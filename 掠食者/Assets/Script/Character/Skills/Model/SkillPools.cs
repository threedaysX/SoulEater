using System.Collections.Generic;
using UnityEngine;

public class SkillPools : Singleton<SkillPools>
{
    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public int size;
    }

    public List<Pool> skillPools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called before the first frame update
    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        GetSkillPools();
    }

    private void GetSkillPools()
    {
        foreach (Pool pool in skillPools)
        {
            Queue<GameObject> skillObjPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject skillObj = Instantiate(pool.prefab);
                skillObj.name = pool.prefab.name;
                skillObj.transform.SetParent(this.transform);
                skillObj.SetActive(false);
                skillObjPool.Enqueue(skillObj);
            }

            poolDictionary.Add(pool.prefab.name, skillObjPool);
        }
    }

    public GameObject SpawnSkillFromPool(Character caster, Skill skill, Vector3 position, Quaternion rotation)
    {      
        string skillName = skill.prefab.name;
        if (!poolDictionary.ContainsKey(skillName))
            return null;

        GameObject objectToSpawn = poolDictionary[skillName].Dequeue();

        ISkillGenerator pooledObj = objectToSpawn.GetComponent<ISkillGenerator>();

        if (pooledObj != null)
        {
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            pooledObj.GenerateSkill(caster, skill);
        }

        poolDictionary[skillName].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
