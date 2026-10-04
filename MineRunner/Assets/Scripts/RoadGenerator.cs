using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public static RoadGenerator Instance { get; private set; }
    [SerializeField] private GameObject[] _roadPrefabs;
    private ObjectPool[] _roadsPools;
    private List<GameObject> _roads = new List<GameObject>();
    public float MaxSpeed = 10;
    private float _currentSpeed = 0;
    [SerializeField] private int _maxRoadCount;
    [SerializeField] private Vector3 _startPose;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _roadsPools = new ObjectPool[_roadPrefabs.Length];

        for (int i = 0; i < _roadsPools.Length; i++)
        {
            GameObject poolObject = new GameObject($"RoadPool_{i}");
            poolObject.transform.SetParent(transform);

            ObjectPool pool = poolObject.AddComponent<ObjectPool>();

            pool.SetPrefab(_roadPrefabs[i]);
            pool.InitPool();

            _roadsPools[i] = pool;
        }
    }

    private void Start()
    {
        ResetLevel();
    }

    // private void OnEnable()
    // {
    //     EventManager.OnStartGame += StartLevel;
    //     EventManager.OnLoseGame += StopLevel;
    //     EventManager.OnResetGame += ResetLevel;
    //     EventManager.OnRebirth += StartLevel;
    // }

    // private void OnDisable()
    // {
    //     EventManager.OnStartGame -= StartLevel;
    //     EventManager.OnLoseGame -= StopLevel;
    //     EventManager.OnResetGame -= ResetLevel;
    //     EventManager.OnRebirth -= StartLevel;
    // }

    private void Update()
    {
        if (_currentSpeed == 0) return;

        foreach (GameObject road in _roads)
        {
            road.transform.position -= new Vector3(_currentSpeed * Time.deltaTime, 0, 0);
        }

        if (_roads[0].transform.position.x < -70)
        {
            _roads[0].SetActive(false);
            _roads.RemoveAt(0);
            CreateNewRoad();
        }
    }

    private void CreateNewRoad(bool setStartRoad = false)
    {
        int index = setStartRoad ? 0 : Random.Range(1, _roadsPools.Length);
        var roadSegmentLenght = _roadsPools[index].PrefabObj.GetComponent<RoadSegment>().Length;
        Vector3 pos = _roads.Count > 0 ? _roads[_roads.Count - 1].transform.position + Vector3.right * roadSegmentLenght : _startPose;

        //GameObject newRoad = Instantiate(_roadPrefabs[index], pos, Quaternion.identity);
        GameObject newRoad = _roadsPools[index].GetObject(pos, Quaternion.identity);
        newRoad.transform.SetParent(transform);
        _roads.Add(newRoad);
    }

    public void ResetLevel()
    {
        StopLevel();
        while (_roads.Count > 0)
        {
            _roads[0].SetActive(false);
            _roads.RemoveAt(0);
        }
        for (int i = 0; i < _maxRoadCount; i++)
        {
            if (i < 3)
            {
                CreateNewRoad(true);
            }
            else
            {
                CreateNewRoad();
            }
        }
    }

    public void StopLevel()
    {
        _currentSpeed = 0;
        StopAllCoroutines();
    }

    public void StartLevel()
    {
        _currentSpeed = MaxSpeed;
    }
}
