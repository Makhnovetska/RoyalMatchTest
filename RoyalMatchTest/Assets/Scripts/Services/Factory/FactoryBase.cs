using System.Collections.Generic;
using UnityEngine;

public class FactoryBase<T> : MonoBehaviour
    where T : MonoBehaviour
{
    [SerializeField] private T prefabToSpawn;
    
    private List<T> spawnedInstances = new List<T>();

    public List<T> Spawn(int count, Transform parentToSpawn = null)
    {
        List<T> spawned = new List<T>();
        for (int i = 0; i < count; i++)
        {
            T instanceSpawned = SpawnInstance(parentToSpawn);
            instanceSpawned.gameObject.SetActive(true);
            spawned.Add(instanceSpawned);   
        }
        
        return spawned;
    }
    
    public void DespawnAll()
    {
        foreach (T instance in spawnedInstances)
        {
            instance.gameObject.SetActive(false);
            instance.gameObject.transform.SetParent(transform);
        }
    }
    
    private T SpawnInstance(Transform parentToSpawn = null)
    {
        foreach (T instance in spawnedInstances)
        {
            if (!instance.gameObject.activeInHierarchy)
            {
                instance.transform.SetParent(parentToSpawn);
                return instance;
            }
        }
        
        T newItem = Instantiate(prefabToSpawn, parentToSpawn);
        spawnedInstances.Add(newItem);
        return newItem;
    }
}
