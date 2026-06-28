using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavesGenerator : MonoBehaviour
{
    //[SerializeField] private GameObject[] CavesPrefabs;
    private GameObject LastCave;
    private ObjectPool objectPool;
    [SerializeField] private Vector3 startPose;
    [SerializeField] private float speedDevider; 
    private float currentSpeed = 0;

    void Start()
    {
        objectPool = GetComponent<ObjectPool>();
        ResetLevel();
    }

    void OnEnable()
    {
        EventManager.OnStartGame += ChangeSpeedToMax;
        EventManager.OnResetGame += ResetLevel;
        EventManager.OnLoseGame += StopLevel;
    }

    void OnDisable()
    {
        EventManager.OnStartGame -= ChangeSpeedToMax;
        EventManager.OnResetGame -= ResetLevel;
        EventManager.OnLoseGame -= StopLevel;
    }

    void Update()
    {
        if(currentSpeed == 0) return; 

        foreach(GameObject cave in objectPool.pool)
        {
            cave.transform.position -= new Vector3(currentSpeed / speedDevider * Time.deltaTime, 0, 0);
        }

        foreach(GameObject cave in objectPool.pool)
        {
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
        Vector3 pos = objectPool.IsHereActiveObj() ? LastCave.transform.position + new Vector3(72,0,0) : startPose;
        GameObject newCave = objectPool.GetObject(pos, Quaternion.identity);
        LastCave = newCave;
    }

    void ResetLevel()
    {
        objectPool.DisableAll();
        for(int i = 0; i < objectPool.PoolSize(); i++) CreateNewCave();
    }

    void StopLevel()
    {
        currentSpeed = 0;
    }

    void ChangeSpeedToMax()
    {
        currentSpeed = RoadGenerator.Instance.maxSpeed;
    }
}
