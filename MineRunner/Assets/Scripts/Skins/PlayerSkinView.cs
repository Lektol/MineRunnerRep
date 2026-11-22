using UnityEngine;
using System.Collections.Generic;
using YG;

public class PlayerSkinView : MonoBehaviour
{
    [SerializeField] private SkinCatalog _skinCatalog;

    public void Start()
    {
        TryToApplySkin(YG2.saves.EquippedSkinId);
    }

    public void SucscribeOnSkinCards(List<SkinCard> skinCards)
    {
        foreach(var skin in skinCards)
        {
            skin.OnSelected += ApplySkin;
        }
    }

    public bool TryToApplySkin(string skinId)
    {
        if (!_skinCatalog.TryGetSkin(skinId, out SkinDefinition target))
        {
            Debug.LogError($"Skin not found: {skinId}");
            return false;
        }
        foreach (SkinDefinition skin in _skinCatalog.Skins.Values)
        {
            skin.gameObject.SetActive(skin == target);
        }
        return true;
    }

    public void ApplySkin(SkinDefinition skin)
    {
        foreach (SkinDefinition Skin in _skinCatalog.Skins.Values)
        {
            Skin.gameObject.SetActive(Skin == skin);
        }
    }
}