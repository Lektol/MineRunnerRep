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
    [Tooltip("Максимум заспавненных чанков одновременно")]
    [SerializeField] private int _maxRoadCount;
    [Tooltip("Количество чанков рельс без препядствий в начале")]
    [SerializeField] private int _countFirstRoads;
    [Tooltip("Количество рельс в каждом пуле")]
    [SerializeField] private int _maxPoolSize;
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
            pool.InitPool(_maxPoolSize);

            _roadsPools[i] = pool;
        }
    }

    private void Start()
    {
        ResetLevel();
    }

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

    private void CreateNewRoad(bool isFirstRoad = false)
    {
        int index = isFirstRoad ? 0 : Random.Range(1, _roadsPools.Length);

        GameObject newRoad = _roadsPools[index].GetObject(Vector3.zero, Quaternion.identity);

        if (_roads.Count > 0)
        {
            RoadSegment newSegment = newRoad.GetComponent<RoadSegment>();
            RoadSegment previousSegment = _roads[_roads.Count - 1].GetComponent<RoadSegment>();
            newSegment.AlignTo(previousSegment.Exit);
        }


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
            if (i < _countFirstRoads)
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
