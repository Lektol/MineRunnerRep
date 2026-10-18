using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningSign : MonoBehaviour
{
    [SerializeField] private GameObject _leftCube;
    [SerializeField] private GameObject _rightCube;
    [SerializeField] private GameObject _middleCube;

    public void Init(MemoryChank.Direction dir)
    {
        switch (dir)
        {
            case MemoryChank.Direction.Left:
                _leftCube.SetActive(true);
                break;
            case MemoryChank.Direction.Right:
                _rightCube.SetActive(true);
                break;
            case MemoryChank.Direction.Middle:
                _middleCube.SetActive(true);
                break;
        }
    }

    private void OnDisable()
    {
        _leftCube.SetActive(false);
        _rightCube.SetActive(false);
        _middleCube.SetActive(false);
    }
}
