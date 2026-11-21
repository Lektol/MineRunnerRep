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
    [SerializeField] private SkinInfoUI _skinInfoUI;
    [SerializeField] private PlayerSkinView _playerSkinView;
    private List<SkinCard> _skinCards;
    public string PreviewSkinId { get; private set; }
    public string EquippedSkinId { get; private set; }


    private void OnEnable()
    {
        if(_skinCards == null) return;
        SucscribeOnSkinCards();
    }

    private void OnDisable()
    {
        if(_skinCards == null) return;
        UnsucscribeOnSkinCards();
    }

    private void Start()
    {
        _skinCards = new List<SkinCard>();
        foreach(var skin in _skinCatalog.Skins)
        {
            CreateSkinCard(skin.Value);
        }
        _skinInfoUI.SucscribeOnSkinCards(_skinCards);
        _playerSkinView.SucscribeOnSkinCards(_skinCards);
        SucscribeOnSkinCards();
    }

    private void CreateSkinCard(SkinDefinition skin)
    {
        SkinCard skinCard = Instantiate(_skinCardPrefab, _contentTransform);
        skinCard.Init(skin, _skinCatalog.IsOwned(skin.Id));
        _skinCards.Add(skinCard);
    }

    private void SucscribeOnSkinCards()
    {
        foreach(var skinCard in _skinCards)
        {
            skinCard.OnSelected += SetSelectedSkinId;
        }
    }

    private void UnsucscribeOnSkinCards()
    {
        foreach(var skinCard in _skinCards)
        {
            skinCard.OnSelected -= SetSelectedSkinId;
        }
    }

    private void SetSelectedSkinId(SkinDefinition skin)
    {
        PreviewSkinId = skin.Id;
        Debug.Log(PreviewSkinId);
    }
}
