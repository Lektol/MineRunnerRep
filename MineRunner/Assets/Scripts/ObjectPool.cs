using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Настройки пула")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 10; 

    public List<GameObject> pool {get; private set;}

    private void Awake()
    {
        InitPool();
    }

    private void InitPool()
    {
        pool = new List<GameObject>(poolSize);

        for (int i = 0; i < poolSize; i++)
        {
            CreateNewObject();
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }


    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                GameObject obj = pool[i];
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                //ActiveObjects.Add(obj);
                return obj;
            }
        }

        GameObject newObj = CreateNewObject();
        newObj.transform.position = position;
        newObj.transform.rotation = rotation;
        newObj.SetActive(true);
        //ActiveObjects.Add(newObj);
        return newObj;
    }

    public void DisableAll()
    {
        foreach (GameObject obj in pool)
        {
            obj.SetActive(false);
        }
    }

    public bool IsHereActiveObj()
    {
        foreach (GameObject obj in pool)
        {
            if(obj.activeSelf) return true;
        } 
        return false;
    }

    public int PoolSize() => poolSize;
}