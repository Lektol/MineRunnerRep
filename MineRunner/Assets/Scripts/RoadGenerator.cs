using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public static RoadGenerator Instance { get; private set;}
    [SerializeField] private GameObject[] _roadPrefabsLevel1;
    [SerializeField] private GameObject[] _roadPrefabsLevel2;
    [SerializeField] private float _secToMedium = 100;
    [SerializeField] private GameObject[] _roadPrefabsLevel3;
    [SerializeField] private float _secToHard = 100;
    public enum LevelDifficulty 
    {
        Easy = 1,
        Medium = 2,
        Hard = 3
    }
    public LevelDifficulty levelDifficulty = LevelDifficulty.Easy;
    private List<GameObject> _roads = new List<GameObject>();
    public float MaxSpeed = 10;
    private float _currentSpeed = 0;
    [SerializeField] private int _maxRoadCount;
    [SerializeField] private Vector3 _startPose;

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
    }

    void OnEnable()
    {
        EventManager.OnStartGame += StartLevel;
        EventManager.OnLoseGame += StopLevel;
        EventManager.OnResetGame += ResetLevel;
        EventManager.OnRebirth += StartLevel;
    }

    void OnDisable()
    {
        EventManager.OnStartGame -= StartLevel;
        EventManager.OnLoseGame -= StopLevel;
        EventManager.OnResetGame -= ResetLevel;
        EventManager.OnRebirth -= StartLevel;
    }

    void Update()
    {
        if(_currentSpeed == 0) return; 

        foreach(GameObject road in _roads)
        {
            road.transform.position -= new Vector3(_currentSpeed * Time.deltaTime, 0, 0);
        }

        if(_roads[0].transform.position.x < -70)
        {
            Destroy(_roads[0]);
            _roads.RemoveAt(0);
            CreateNewRoad();
        }
    }

    void CreateNewRoad(bool isFirstRoad = false)
    {
        GameObject[] RoadPrefabs = null;
        switch (levelDifficulty)
        {
            case LevelDifficulty.Easy:
                RoadPrefabs = _roadPrefabsLevel1;
                break;
            case LevelDifficulty.Medium:
                RoadPrefabs = _roadPrefabsLevel2;
                break;
            case LevelDifficulty.Hard:
                RoadPrefabs = _roadPrefabsLevel3;
                break;
        }
        Vector3 pos = _roads.Count > 0 ? _roads[_roads.Count-1].transform.position + new Vector3(56,0,0) : _startPose;
        int index = isFirstRoad ? 0 : Random.Range(0, RoadPrefabs.Length);

        GameObject newRoad = Instantiate(RoadPrefabs[index], pos, Quaternion.identity);
        newRoad.transform.SetParent(transform);
        _roads.Add(newRoad);
    }

    void ResetLevel()
    {
        _currentSpeed = 0;
        while(_roads.Count > 0)
        {
            Destroy(_roads[0]);
            _roads.RemoveAt(0);
        }
        for(int i = 0; i < _maxRoadCount; i++)
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
        _currentSpeed = 0;
        StopAllCoroutines();
    }

    void StartLevel()
    {
        _currentSpeed = MaxSpeed;
        StartCoroutine(ChangeLevelDifficulty());
    }

    IEnumerator ChangeLevelDifficulty()
    {
        yield return new WaitForSeconds(_secToMedium);
        levelDifficulty = LevelDifficulty.Medium;
        yield return new WaitForSeconds(_secToHard);
        levelDifficulty = LevelDifficulty.Hard;
    }
}
