using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }
    private int crystals;
    public int Crystals => crystals;

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
        crystals += 1;
        OnCrystalsChanged?.Invoke(Crystals);
    }
}
