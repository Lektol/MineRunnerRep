using System;
using UnityEngine;
using YG;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }
    public int Crystals
    {
        get{ return YG2.saves.Crystals; }
        private set 
        { 
            YG2.saves.Crystals = value; 
        }
    }

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

    private void OnEnable()
    {
        EventManager.OnGetCrystal += AddCrystal;
        EventManager.OnStartGame += SaveProgressAndSetNull;
    }

    private void OnDisable()
    {
        EventManager.OnGetCrystal -= AddCrystal;
        EventManager.OnStartGame -= SaveProgressAndSetNull;
    }

    private void AddCrystal()
    {
        Crystals += 1;
        OnCrystalsChanged?.Invoke(Crystals);
    }
    private void SaveProgressAndSetNull()
    {
        if(Crystals > YG2.saves.MaxCrystals)
        {
            YG2.SetLeaderboard("LeaderBoardSpike", Crystals);
            YG2.saves.MaxCrystals = Crystals;
            YG2.SaveProgress();
        }
        Crystals = 0;
        OnCrystalsChanged?.Invoke(Crystals);
    }
}
