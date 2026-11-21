using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkinCard : MonoBehaviour
{
    [SerializeField] private GameObject _iconLock; 
    [SerializeField] private Image _image;
    private SkinDefinition _skinDefinition;
    public event Action<SkinDefinition> OnSelected;


    private void OnEnable()
    {
       EventManager.OnShopChanged += SetLock; 
    }

    private void OnDisable()
    {
        EventManager.OnShopChanged -= SetLock; 
    }
    public void Init(SkinDefinition skin, bool isOwn)
    {
        _skinDefinition = skin;
        _iconLock.SetActive(!isOwn);
        if(_skinDefinition.Sprite != null) _image.sprite = _skinDefinition.Sprite;
    }

    public void OnSkinSelect()
    {
        OnSelected?.Invoke(_skinDefinition);
    }

    private void SetLock(SkinDefinition skin)
    {
        if(skin == _skinDefinition) _iconLock.SetActive(false);
    }
}
