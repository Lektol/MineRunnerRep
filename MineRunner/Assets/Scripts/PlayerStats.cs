using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }
    private int _crystals;
    public int Crystals => _crystals;

    public event Action<int> OnCrystalsChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        EventManager.OnGetCrystal += AddCrystal;
    }

    void OnDisable()
    {
        EventManager.OnGetCrystal -= AddCrystal;
    }

    void AddCrystal()
    {
        _crystals += 1;
        OnCrystalsChanged?.Invoke(Crystals);
    }
}
