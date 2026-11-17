using System.Collections.Generic;
using UnityEngine;

public class SkinCatalog : MonoBehaviour
{
    [SerializeField] private Transform _skinsRoot;

    private readonly Dictionary<string, SkinDefinition> _skins = new();

    public IReadOnlyDictionary<string, SkinDefinition> Skins => _skins;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _skins.Clear();

        SkinDefinition[] definitions =
            _skinsRoot.GetComponentsInChildren<SkinDefinition>(true);
        foreach (SkinDefinition skin in definitions)
        {
            if (string.IsNullOrWhiteSpace(skin.Id))
            {
                Debug.LogError(
                    $"Skin {skin.name} has an empty ID.", skin);
                continue;
            }

            if (_skins.ContainsKey(skin.Id))
            {
                Debug.LogError(
                    $"Duplicate skin ID: {skin.Id}", skin);
                continue;
            }

            _skins.Add(skin.Id, skin);
        }
    }

    public bool TryGetSkin(string id, out SkinDefinition skin)
    {
        return _skins.TryGetValue(id, out skin);
    }
}