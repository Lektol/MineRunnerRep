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


    public bool TryGetObject(out GameObject obj, Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < Pool.Count; i++)
        {
            if (!Pool[i].activeInHierarchy)
            {
                obj = Pool[i];
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                //ActiveObjects.Add(obj);
                return true;
            }
        }
        obj = null;
        return false;
    }

    public void DisableAll()
    {
        foreach (GameObject obj in Pool)
        {
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// Возвращает true - если всё объекты сейчас активны, т.е. используются пулом и на данный момент он "пуст".Из него нечего достать.
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
        foreach (GameObject obj in Pool)
        {
            if(!obj.activeSelf) return false;
        } 
        return true;
    }

    public int PoolSize() => _poolSize;
}