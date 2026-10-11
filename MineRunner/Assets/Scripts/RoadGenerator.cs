using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public static RoadGenerator Instance { get; private set; }
    [SerializeField] private GameObject[] _roadPrefabs;
    private ObjectPool[] _allRoadsPools;
    private List<ObjectPool> _roadsPoolsActive;
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

        _allRoadsPools = new ObjectPool[_roadPrefabs.Length];
        _roadsPoolsActive = new List<ObjectPool>();

        for (int i = 0; i < _allRoadsPools.Length; i++)
        {
            GameObject poolObject = new GameObject($"RoadPool_{i}");
            poolObject.transform.SetParent(transform);

            ObjectPool pool = poolObject.AddComponent<ObjectPool>();

            pool.SetPrefab(_roadPrefabs[i]);
            pool.InitPool(_maxPoolSize);

            _allRoadsPools[i] = pool;
            _roadsPoolsActive.Add(pool);
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
        int index = isFirstRoad ? 0 : Random.Range(0, _roadsPoolsActive.Count - 1);

        _roadsPoolsActive[index].TryGetObject(out GameObject newRoad, Vector3.zero, Quaternion.identity);
        if (_roads.Count > 0)
        {
            try
            {
                RoadSegment newSegment = newRoad.GetComponent<RoadSegment>();
                RoadSegment previousSegment = _roads[_roads.Count - 1].GetComponent<RoadSegment>();
                newSegment.AlignTo(previousSegment.Exit);
            }

            catch
            {
                Debug.LogError(index + " Пустой: " + _roadsPoolsActive[index].IsEmpty());
            }
        }
        newRoad.transform.SetParent(transform);
        _roads.Add(newRoad);
        if (_roadsPoolsActive[index].IsEmpty())
        {
            _roadsPoolsActive.RemoveAt(index);
            UpdatePoolsList();
        }
        //GameObject newRoad = _roadsPools[index].GetObject(Vector3.zero, Quaternion.identity);
    }

    private void UpdatePoolsList()
    {
        foreach (var pool in _allRoadsPools)
        {
            if (!pool.IsEmpty() && !_roadsPoolsActive.Contains(pool)) _roadsPoolsActive.Add(pool);
        }
    }

    public void ResetLevel()
    {
        StopLevel();
        while (_roads.Count > 0)
        {
            _roads[0].SetActive(false);
            _roads.RemoveAt(0);
        }
        UpdatePoolsList();
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
