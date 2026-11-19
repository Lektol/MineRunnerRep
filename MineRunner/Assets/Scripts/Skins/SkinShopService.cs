using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class SkinShopService : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SkinCatalog _catalog;
    [SerializeField] private PlayerSkinView _playerSkinView;

    // [Header("Purchase providers")]
    // [SerializeField] private CurrencyWallet _currencyWallet;
    // [SerializeField] private RewardedAdProvider _rewardedAdProvider;
    // [SerializeField] private RealMoneyPurchaseProvider _purchaseProvider;

    [Header("Settings")]
    [SerializeField] private string _defaultSkinId = "spike";


    private readonly HashSet<string> _pendingPurchases = new();

    public event Action OnShopChanged;

    private void Start()
    {
        LoadProgress();
        RestoreRealMoneyPurchases();
    }

    private void LoadProgress()
    {
        _playerSkinView.Initialize(YG2.saves.EquippedSkinId);
    }

    public bool IsOwned(string skinId)
    {
        return YG2.saves.OwnedSkins.Contains(skinId);
    }

    public bool Purchase(string skinId)
    {
        if(!_catalog.TryGetSkin(skinId, out SkinDefinition skin)) return false;
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
        if (IsOwned(skin.Id))
            return false;

        YG2.saves.OwnedSkins.Add(skin.Id);
        YG2.SaveProgress();
        return true;
    }

    public bool Equip(string skinId)
    {
        if (!IsOwned(skinId))
            return false;

        if (!_catalog.TryGetSkin(skinId, out _))
            return false;

        if (!_playerSkinView.ApplySkin(skinId))
            return false;

        YG2.saves.EquippedSkinId = skinId;
        YG2.SaveProgress();
        return true;
    }

    private void RestoreRealMoneyPurchases()
    {
    }
}