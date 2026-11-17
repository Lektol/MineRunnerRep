using UnityEngine;

public class PlayerSkinView : MonoBehaviour
{
    [SerializeField] private SkinCatalog _skinCatalog;

    public void Initialize(string defaultSkinId)
    {
        foreach (SkinDefinition skin in _skinCatalog.Skins.Values)
        {
            skin.gameObject.SetActive(false);
        }

        ApplySkin(defaultSkinId);
    }

    public bool ApplySkin(string skinId)
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
}