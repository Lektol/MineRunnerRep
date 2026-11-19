using System;
using UnityEngine;
using System.Collections.Generic;
using YG;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    public int CollectedCrystals { get; private set; }

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
    }

    private void OnDisable()
    {
        EventManager.OnGetCrystal -= AddCrystal;
    }

    private void AddCrystal()
    {
        CollectedCrystals++;
        OnCrystalsChanged?.Invoke(CollectedCrystals);
    }

    public void ResetRunStats()
    {
        CollectedCrystals = 0;
        OnCrystalsChanged?.Invoke(CollectedCrystals);
    }

    public void SaveProgressAndSetNull()
    {
        if(CollectedCrystals > YG2.saves.MaxCrystals)
        {
            YG2.SetLeaderboard("LeaderBoardSpike", CollectedCrystals);
            YG2.saves.MaxCrystals = CollectedCrystals;
            YG2.SaveProgress();
        }
        YG2.saves.BalanceCrystals += CollectedCrystals;
        CollectedCrystals = 0;
        YG2.SaveProgress();
        OnCrystalsChanged?.Invoke(CollectedCrystals);
    }
}
