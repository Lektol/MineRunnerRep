using UnityEngine;

/// <summary>
/// Класс не относится к необычным чанкам дорог, просто нужен для тех чанков, которые могут поменять препятствие местами (было лево стало право)
/// </summary>
public class SymmetricalRoad : MonoBehaviour
{
    [SerializeField] private GameObject[] _versions;
    private GameObject _lastActiveObject;

    private void Awake()
    {
        foreach(var version in _versions)
        {
            version.SetActive(false);
        }
    }

    private void OnEnable()
    {
        int index = Random.Range(0, _versions.Length);
        _versions[index].SetActive(true);
        _lastActiveObject = _versions[index];
    }

    private void OnDisable()
    {
        if(_lastActiveObject == null) return;
        _lastActiveObject.SetActive(false);
        _lastActiveObject = null;
    }
}
