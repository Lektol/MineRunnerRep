using System;
using UnityEngine;
using System.Collections.Generic;
using YG;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }
    private int _crystals = 0;
    public int Crystals => _crystals;
    private int _crystalsBalance = 0;
    public int CrystalsBalance => _crystalsBalance;
    public List<string> OwnedSkins = new();
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
        Initialize();
    }

    private void Initialize()
    {
        _crystalsBalance = YG2.saves.BalanceCrystals;
        OwnedSkins = YG2.saves.OwnedSkins;
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
        _crystals += 1;
        OnCrystalsChanged?.Invoke(_crystals);
    }
    public void SaveProgressAndSetNull()
    {
        if(_crystals > YG2.saves.MaxCrystals)
        {
            YG2.SetLeaderboard("LeaderBoardSpike", _crystals);
            YG2.saves.MaxCrystals = _crystals;
            YG2.SaveProgress();
        }
        YG2.saves.BalanceCrystals += _crystals;
        _crystals = 0;
        YG2.SaveProgress();
        OnCrystalsChanged?.Invoke(_crystals);
    }
}
