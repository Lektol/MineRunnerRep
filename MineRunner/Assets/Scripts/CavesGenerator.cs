using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavesGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] CavesPrefabs;
    private List<GameObject> Caves = new List<GameObject>();
    [SerializeField] private int countCaves; 
    [SerializeField] private float speedDevider; 
    private float currentSpeed = 0;

    void Start()
    {
        RestartLevel();
    }

    void OnEnable()
    {
        EventManager.OnStartGame += ChangeSpeedToMax;
        EventManager.OnLooseGame += RestartLevel;
    }

    void OnDisable()
    {
        EventManager.OnStartGame -= ChangeSpeedToMax;
        EventManager.OnLooseGame -= RestartLevel;
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
        Vector3 pos = new Vector3(0f, -50f, 0f);
        if(Caves.Count > 0) { pos = Caves[Caves.Count-1].transform.position + new Vector3(72,0,0); }
        int index;
        index = Random.Range(0, CavesPrefabs.Length);
        GameObject newCave = Instantiate(CavesPrefabs[index], pos, Quaternion.identity);
        newCave.transform.SetParent(transform);
        Caves.Add(newCave);
    }

    void RestartLevel()
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
