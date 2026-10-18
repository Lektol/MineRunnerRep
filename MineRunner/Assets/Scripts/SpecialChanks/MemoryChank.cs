using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class MemoryChank : MonoBehaviour
{
    public enum Direction
    {
        Left,
        Right,
        Middle,
    }

    [Header("Столбы предупреждения")]
    [SerializeField] private WarningSign[] _warningSighs;

    [Header("Препятствия")]

    [SerializeField] private GameObject _barrierFakeLeftPrefab;
    [SerializeField] private GameObject _barrierFakeMiddlePrefab;
    [SerializeField] private GameObject _barrierFakeRightPrefab;
    /// <summary>
    /// 3 пустышек в заранее подготовленных позициях для барьера, чтобы не задавать координаты вручную
    /// </summary>
    [SerializeField] private GameObject[] _parentsForBarriers = new GameObject[3];
    private GameObject[] _barriers;
    private Direction[] _directions;

    private void Awake()
    {
        _directions = new Direction[3];
        _barriers = new GameObject[3];
    }

    private void OnEnable()
    {
        SetDirections();
        SetBarriers();
        SetWarningSigns();
    }

    private void OnDisable()
    {
        ClearBarriers();
    }

    private void SetDirections()
    {
        for (int i = 0; i < _directions.Length; i++)
        {
            _directions[i] = (Direction)UnityEngine.Random.Range(0, _directions.Length);
        }
    }

    private void SetBarriers()
    {
        for (int i = 0; i < _parentsForBarriers.Length; i++)
        {
            switch (_directions[i])
            {
                case Direction.Left:
                    _barriers[i] = Instantiate(_barrierFakeLeftPrefab, _parentsForBarriers[i].transform);
                    break;
                case Direction.Right:
                    _barriers[i] = Instantiate(_barrierFakeRightPrefab, _parentsForBarriers[i].transform);
                    break;
                case Direction.Middle:
                    _barriers[i] = Instantiate(_barrierFakeMiddlePrefab, _parentsForBarriers[i].transform);
                    break;
            }
            _barriers[i].transform.localPosition = Vector3.zero;
        }
    }

    private void SetWarningSigns()
    {
        for (int i = 0; i < _warningSighs.Length; i++)
        {
            _warningSighs[i].Init(_directions[i]);
        }
    }

    private void ClearBarriers()
    {
        for (int i = 0; i < _barriers.Length; i++)
        {
            if (_barriers[i] != null)
            {
                Destroy(_barriers[i]);
                _barriers[i] = null;
            }
        }
    }
}