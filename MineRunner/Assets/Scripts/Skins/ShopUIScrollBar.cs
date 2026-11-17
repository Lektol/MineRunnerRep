using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIScrollBar : MonoBehaviour
{
    [SerializeField] private SkinCatalog _skinCatalog;
    [SerializeField] private SkinCard _skinCardPrefab;
    [SerializeField] private Transform _contentTransform;
    public string PreviewSkinId { get; private set; }
    public string EquippedSkinId { get; private set; }


    private void Start()
    {
        foreach(var skin in _skinCatalog.Skins)
        {
            CreateSkinCard(skin.Value);
        }   

    }

    private void CreateSkinCard(SkinDefinition skin)
    {
        SkinCard skinCard = Instantiate(_skinCardPrefab, _contentTransform);
        skinCard.Init(skin);
    }
}
