using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class SkinCard : MonoBehaviour
{
    [SerializeField] private GameObject _iconLock; 
    [SerializeField] private Image _image;
    private SkinDefinition _skinDefinition;
    public event Action<SkinDefinition> OnCardPointed;

    public void Init(SkinDefinition skin)
    {
        _skinDefinition = skin;
        if(!YG2.saves.OwnedSkins.Contains(_skinDefinition.Id)) _iconLock.SetActive(true);
        if(_skinDefinition.Sprite != null) _image.sprite = _skinDefinition.Sprite;
    }

    public void OnSkinSelect()
    {
        Debug.Log("a");
        OnCardPointed?.Invoke(_skinDefinition);
    }
}
