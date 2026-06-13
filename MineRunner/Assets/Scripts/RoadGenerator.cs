using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public static RoadGenerator Instance { get; private set;}
    [SerializeField] private GameObject[] RoadPrefabsLevel1;
    [SerializeField] private GameObject[] RoadPrefabsLevel2;
    [SerializeField] private float secToMedium = 100;
    [SerializeField] private GameObject[] RoadPrefabsLevel3;
    [SerializeField] private float secToHard = 100;
    public enum LevelDifficulty 
    {
        Easy = 1,
        Medium = 2,
        Hard = 3
    }
    public LevelDifficulty levelDifficulty = LevelDifficulty.Easy;
    private List<GameObject> Roads = new List<GameObject>();
    public float maxSpeed = 10;
    private float currentSpeed = 0;
    [SerializeField] private int maxRoadCount;
    [SerializeField] private Vector3 startPose;

    void Awake()
    {
        if(Instance != null)
        {
           Destroy(gameObject); 
           return;
        } 
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ResetLevel();
        EventManager.OnStartGameInvoke();
    }

    void OnEnable()
    {
        EventManager.OnStartGame += StartLevel;
        EventManager.OnLooseGame += StopLevel;
        EventManager.OnRestartGame += ResetLevel;
    }

    void OnDisable()
    {
        EventManager.OnStartGame -= StartLevel;
        EventManager.OnLooseGame -= StopLevel;
        EventManager.OnRestartGame -= ResetLevel;
    }

    void Update()
    {
        if(currentSpeed == 0) return; 

        foreach(GameObject road in Roads)
        {
            road.transform.position -= new Vector3(currentSpeed * Time.deltaTime, 0, 0);
        }

        if(Roads[0].transform.position.x < -70)
        {
            Destroy(Roads[0]);
            Roads.RemoveAt(0);
            CreateNewRoad();
        }
    }

    void CreateNewRoad(bool isFirstRoad = false)
    {
        GameObject[] RoadPrefabs = null;
        switch (levelDifficulty)
        {
            case LevelDifficulty.Easy:
                RoadPrefabs = RoadPrefabsLevel1;
                break;
            case LevelDifficulty.Medium:
                RoadPrefabs = RoadPrefabsLevel2;
                break;
            case LevelDifficulty.Hard:
                RoadPrefabs = RoadPrefabsLevel3;
                break;
        }
        Vector3 pos = Roads.Count > 0 ? Roads[Roads.Count-1].transform.position + new Vector3(56,0,0) : startPose;
        int index = isFirstRoad ? 0 : Random.Range(0, RoadPrefabs.Length);
        
        GameObject newRoad = Instantiate(RoadPrefabs[index], pos, Quaternion.identity);
        newRoad.transform.SetParent(transform);
        Roads.Add(newRoad);
    }

    void ResetLevel()
    {
        currentSpeed = 0;
        while(Roads.Count > 0)
        {
            Destroy(Roads[0]);
            Roads.RemoveAt(0);
        }
        for(int i = 0; i < maxRoadCount; i++)
        {
            if(i < 3)
            {
                CreateNewRoad(true);
            }
            else
            {
                CreateNewRoad();
            }
        }
        levelDifficulty = LevelDifficulty.Easy;
        StopAllCoroutines();
    }

    void StopLevel()
    {
        currentSpeed = 0;
        StopAllCoroutines();
    }

    void StartLevel()
    {
        currentSpeed = maxSpeed;
        StartCoroutine(ChangeLevelDifficulty());
    }

    IEnumerator ChangeLevelDifficulty()
    {
        yield return new WaitForSeconds(secToMedium);
        levelDifficulty = LevelDifficulty.Medium;
        yield return new WaitForSeconds(secToHard);
        levelDifficulty = LevelDifficulty.Hard;
    }
}
