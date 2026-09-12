using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavesGenerator : MonoBehaviour
{
    //[SerializeField] private GameObject[] CavesPrefabs;
    private GameObject _lastCave;
    private ObjectPool _objectPool;
    [SerializeField] private Vector3 _startPose;
    [SerializeField] private float _speedDevider; 
    private float _currentSpeed = 0;

    void Start()
    {
        _objectPool = GetComponent<ObjectPool>();
        ResetLevel();
    }

    void OnEnable()
    {
        EventManager.OnStartGame += ChangeSpeedToMax;
        EventManager.OnResetGame += ResetLevel;
        EventManager.OnLoseGame += StopLevel;
        EventManager.OnRebirth += ChangeSpeedToMax;
    }

    void OnDisable()
    {
        EventManager.OnStartGame -= ChangeSpeedToMax;
        EventManager.OnResetGame -= ResetLevel;
        EventManager.OnLoseGame -= StopLevel;
        EventManager.OnRebirth -= ChangeSpeedToMax;
    }

    void Update()
    {
        if(_currentSpeed == 0) return; 

        foreach(GameObject cave in _objectPool.pool)
        {
            cave.transform.position -= new Vector3(_currentSpeed / _speedDevider * Time.deltaTime, 0, 0);
            if(cave.transform.position.x < -90)
            {
                cave.SetActive(false);
                CreateNewCave(); 
            }
        }

    }

    void CreateNewCave()
    {
        //objectPool.IsHereActiveObj();
        Vector3 pos = _objectPool.IsHereActiveObj() ? _lastCave.transform.position + new Vector3(72,0,0) : _startPose;
        GameObject newCave = _objectPool.GetObject(pos, Quaternion.identity);
        _lastCave = newCave;
    }

    void ResetLevel()
    {
        StopLevel();
        _objectPool.DisableAll();
        for(int i = 0; i < _objectPool.PoolSize(); i++) CreateNewCave();
    }

    void StopLevel()
    {
        _currentSpeed = 0;
    }

    void ChangeSpeedToMax()
    {
        _currentSpeed = RoadGenerator.Instance.MaxSpeed;
    }
}
