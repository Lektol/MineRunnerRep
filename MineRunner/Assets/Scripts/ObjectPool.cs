using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Настройки пула")]
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _poolSize = 10; 

    public List<GameObject> Pool {get; private set;}
    public GameObject PrefabObj => _prefab;

    private void Awake()
    {
        InitPool(_poolSize);
    }

    public void SetPrefab(GameObject gameObjectPref)
    {
        _prefab = gameObjectPref;
    }

    public void InitPool(int poolSize)
    {
        if(_prefab == null)
        {
            Debug.LogWarning("На пулл объекта:" + gameObject.name + " нету объекта префаба");
            return;
        }
        _poolSize = poolSize;
        Pool = new List<GameObject>(_poolSize);

        for (int i = 0; i < poolSize; i++)
        {
            CreateNewObject();
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(_prefab, transform);
        obj.SetActive(false);
        Pool.Add(obj);
        return obj;
    }


    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < Pool.Count; i++)
        {
            if (!Pool[i].activeInHierarchy)
            {
                GameObject obj = Pool[i];
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
        foreach (GameObject obj in Pool)
        {
            obj.SetActive(false);
        }
    }

    public bool IsHereActiveObj()
    {
        foreach (GameObject obj in Pool)
        {
            if(obj.activeSelf) return true;
        } 
        return false;
    }

    public int PoolSize() => _poolSize;
}