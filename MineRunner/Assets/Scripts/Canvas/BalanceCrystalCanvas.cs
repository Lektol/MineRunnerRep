using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BalanceCrystalCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _textBalances; 
    
    private void OnEnable()
    {
        EventManager.OnBalanceChanged += ChangeCrystalCountText;
    }

    private void OnDisable()
    {
        EventManager.OnBalanceChanged -= ChangeCrystalCountText;
    }
    private void ChangeCrystalCountText(int count)
    {
        foreach(var txt in _textBalances)
        {
            txt.text = "" + count;
        }
    }
}
