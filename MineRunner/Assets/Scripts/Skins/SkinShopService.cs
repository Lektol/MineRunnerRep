using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class SkinShopService : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SkinCatalog _skinCatalog;
    [SerializeField] private PlayerSkinView _playerSkinView;
    [SerializeField] private ShopUIScrollBar _shopUIScrollBar;
    [SerializeField] private SkinInfoUI _skinInfoUI;
    [Header("Sounds")]
    [SerializeField] private AudioClip _audioClipSuccess;
    [SerializeField] private AudioClip _audioClipFailure;

    private void Start()
    {
        LoadProgress();
    }

    private void LoadProgress()
    {
        _playerSkinView.Initialize(YG2.saves.EquippedSkinId);
        _skinCatalog.TryGetSkin(YG2.saves.EquippedSkinId, out SkinDefinition skin);
        _skinInfoUI.UpdateInfo(skin);
    }

    public void TakeSkinButton()
    {
        if(_shopUIScrollBar.PreviewSkinId == YG2.saves.EquippedSkinId) return;
        if(_skinCatalog.IsOwned(_shopUIScrollBar.PreviewSkinId))
        {
            Equip(_shopUIScrollBar.PreviewSkinId);
            return;
        }
        

        if(TryToPurchase(_shopUIScrollBar.PreviewSkinId))
        {
            _skinCatalog.TryGetSkin(_shopUIScrollBar.PreviewSkinId, out SkinDefinition skin);
            EventManager.ShopChanged(skin);
        }
        else
        {
            Debug.Log("Noooo");
        }
    }

    private bool TryToPurchase(string skinId)
    {
        if(!_skinCatalog.TryGetSkin(skinId, out SkinDefinition skin)) return false;
        if(skin.UnlockType == SkinUnlockType.Crystals) return PurchaseWithCrystals(skin);
        return true;
    }

    private bool PurchaseWithCrystals(SkinDefinition skin)
    {
        int balance = YG2.saves.BalanceCrystals;
        int cost = skin.Price;
        if(cost > balance) return false;
        if(TryToUnlockSkin(skin))
        {
            YG2.saves.BalanceCrystals -= cost;    
            return true;
        }
        else
        {
            return false;
        }     
    }

    private void PurchaseWithAd()
    {
    }

    private void PurchaseWithRealMoney()
    {
    }

    private bool TryToUnlockSkin(SkinDefinition skin)
    {
        if (_skinCatalog.IsOwned(skin.Id))
            return false;

        YG2.saves.OwnedSkins.Add(skin.Id);
        YG2.SaveProgress();
        return true;
    }

    public bool Equip(string skinId)
    {
        if (!_skinCatalog.IsOwned(skinId))
            return false;

        if (!_skinCatalog.TryGetSkin(skinId, out SkinDefinition skin))
            return false;

        if (!_playerSkinView.TryToApplySkin(skinId))
            return false;

        YG2.saves.EquippedSkinId = skinId;
        YG2.SaveProgress();
        _skinInfoUI.UpdateInfo(skin);
        return true;
    }
}