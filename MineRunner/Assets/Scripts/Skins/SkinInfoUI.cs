using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class SkinInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    /// <summary>
    /// Кнопка использования(может быть надпись купить за ... сумму, если скин не куплен)
    /// </summary>
    [SerializeField] private TextMeshProUGUI _useButtonText;
    [SerializeField] private GameObject _crystalIcon;
    private bool _langIsRu = false;

    private void OnEnable()
    {
        EventManager.OnShopChanged += UpdateInfo;
    }

    private void OnDisable()
    {
        EventManager.OnShopChanged -= UpdateInfo;
    }

    private void Start()
    {
        _langIsRu = YG2.lang == "ru";
    }

    public void SucscribeOnSkinCards(List<SkinCard> skinCards)
    {
        foreach(var skin in skinCards)
        {
            skin.OnSelected += UpdateInfo;
        }
    }

    public void UpdateInfo(SkinDefinition skin)
    {
        SetName(skin);
        SetUseButtonText(skin);
    }

    private void SetName(SkinDefinition skin)
    {
        if(_langIsRu) _nameText.text = skin.SkinNameRU;
        else _nameText.text = skin.SkinNameEN;
    }

    private void SetUseButtonText(SkinDefinition skin)
    {
        _crystalIcon.SetActive(false);
        if(YG2.saves.OwnedSkins.Contains(skin.Id))
        {
            string PutTextRu = "";
            string PutTextEn = "";
            if(skin.Id == YG2.saves.EquippedSkinId)
            {
                PutTextRu = "Выбран";
                PutTextEn = "Equipped";
            }
            else
            {
                PutTextRu = "Выбрать";
                PutTextEn = "Equip";
            }
            if(_langIsRu) _useButtonText.text = PutTextRu;
            else _useButtonText.text = PutTextEn;
            return;
        }

        if(skin.UnlockType == SkinUnlockType.Crystals)
        {
            _crystalIcon.SetActive(true);
            _useButtonText.text = skin.Price+"";
        }
        else if(skin.UnlockType == SkinUnlockType.RewardedAd)
        {
            if(_langIsRu) _useButtonText.text = "Реклама";
            else _useButtonText.text = "Add";
        }
    }
}
