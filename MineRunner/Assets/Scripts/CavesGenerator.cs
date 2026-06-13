using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavesGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] CavesPrefabs;
    private List<GameObject> Caves = new List<GameObject>();
    [SerializeField] private int countCaves; 
    [SerializeField] private Vector3 startPose;
    [SerializeField] private float speedDevider; 
    private float currentSpeed = 0;

    void Start()
    {
        ResetLevel();
    }

    void OnEnable()
    {
        EventManager.OnStartGame += ChangeSpeedToMax;
        EventManager.OnLoseGame += ResetLevel;
    }

    void OnDisable()
    {
        EventManager.OnStartGame -= ChangeSpeedToMax;
        EventManager.OnLoseGame -= ResetLevel;
    }

    void Update()
    {
        if(currentSpeed == 0) return; 

        foreach(GameObject cave in Caves)
        {
            cave.transform.position -= new Vector3(currentSpeed / speedDevider * Time.deltaTime, 0, 0);
        }

        if(Caves[0].transform.position.x < -70)
        {
            Destroy(Caves[0]);
            Caves.RemoveAt(0);
            CreateNewCave();
        }
    }

    void CreateNewCave()
    {
        Vector3 pos = Caves.Count > 0 ? Caves[Caves.Count-1].transform.position + new Vector3(72,0,0) : startPose;
        int index = Random.Range(0, CavesPrefabs.Length);
        GameObject newCave = Instantiate(CavesPrefabs[index], pos, Quaternion.identity);
        newCave.transform.SetParent(transform);
        Caves.Add(newCave);
    }

    void ResetLevel()
    {
        currentSpeed = 0;
        while(Caves.Count > 0)
        {
            Destroy(Caves[0]);
            Caves.RemoveAt(0);
        }
        for(int i = 0; i < countCaves; i++) CreateNewCave();
    }

    void ChangeSpeedToMax()
    {
        currentSpeed = RoadGenerator.Instance.maxSpeed;
    }
}
